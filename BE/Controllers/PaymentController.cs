using BE.Data;
using BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        public PaymentController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _client = new HttpClient();
        }

        // 1️⃣ Lấy danh sách khóa học chưa thanh toán của học viên
        [HttpGet("unpaid/{studentId}")]
        public async Task<IActionResult> GetUnpaidCourses(string studentId)
        {
            var unpaid = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .Where(e => e.Student.StudentCode == studentId)
                .Where(e => !_context.Payments.Any(p => p.EnrollmentId == e.Id && p.Status == 1))
                .Select(e => new
                {
                    e.Id,
                    e.Course.Title,
                    e.Course.Fee
                })
                .ToListAsync();

            return Ok(unpaid);
        }

        // 2️⃣ Gửi yêu cầu tạo thanh toán MoMo
        [HttpPost("create-momo")]
        public async Task<IActionResult> CreateMoMo([FromBody] MoMoPaymentRequest req)
        {
            var momo = _config.GetSection("MoMo");
            string endpoint = momo["Endpoint"];
            string partnerCode = momo["PartnerCode"];
            string accessKey = momo["AccessKey"];
            string secretKey = momo["SecretKey"];
            string returnUrl = momo["ReturnUrl"];
            string notifyUrl = momo["NotifyUrl"];
            string requestType = "captureWallet";

            string rawHash =
                $"accessKey={accessKey}&amount={req.amount}&extraData=&ipnUrl={notifyUrl}&orderId={req.orderId}&orderInfo={req.orderInfo}&partnerCode={partnerCode}&redirectUrl={returnUrl}&requestId={req.orderId}&requestType={requestType}";

            string signature = SignSHA256(rawHash, secretKey);

            var message = new
            {
                partnerCode,
                accessKey,
                requestId = req.orderId,
                amount = req.amount.ToString(),
                orderId = req.orderId,
                orderInfo = req.orderInfo,
                redirectUrl = returnUrl,
                ipnUrl = notifyUrl,
                extraData = "",
                requestType,
                signature
            };

            var response = await _client.PostAsJsonAsync(endpoint, message);
            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();

            if (result != null && result.ContainsKey("payUrl"))
                return Ok(result["payUrl"]);

            return BadRequest(result);
        }

        // 3️⃣ MoMo gửi callback về đây sau khi thanh toán
        [HttpPost("momo-callback")]
        public async Task<IActionResult> MoMoCallback([FromBody] MoMoCallbackRequest data)
        {
            var secretKey = _config["MoMo:SecretKey"];

            string rawHash =
                $"accessKey={_config["MoMo:AccessKey"]}&amount={data.amount}&extraData={data.extraData}&message={data.message}&orderId={data.orderId}&orderInfo={data.orderInfo}&orderType={data.orderType}&partnerCode={data.partnerCode}&payType={data.payType}&requestId={data.requestId}&responseTime={data.responseTime}&resultCode={data.resultCode}&transId={data.transId}";
            string signature = SignSHA256(rawHash, secretKey);

            if (signature != data.signature)
                return BadRequest("Chữ ký không hợp lệ");

            if (data.resultCode == 0)
            {
                int enrollmentId = int.Parse(data.orderId);
                var payment = new Payment
                {
                    EnrollmentId = enrollmentId,
                    Amount = decimal.Parse(data.amount),
                    PaymentDate = DateTime.Now,
                    Method = "MoMo",
                    Status = 1,
                    TransactionId = data.transId.ToString(),
                    Description = data.orderInfo
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                return Ok("Thanh toán thành công");
            }

            return Ok("Thanh toán thất bại");
        }

        private static string SignSHA256(string data, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(dataBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    // Model nhận dữ liệu
    public class MoMoPaymentRequest
    {
        public string orderId { get; set; }
        public decimal amount { get; set; }
        public string orderInfo { get; set; }
    }

    public class MoMoCallbackRequest
    {
        public string partnerCode { get; set; }
        public string orderId { get; set; }
        public string requestId { get; set; }
        public string amount { get; set; }
        public string orderInfo { get; set; }
        public string orderType { get; set; }
        public long transId { get; set; }
        public int resultCode { get; set; }
        public string message { get; set; }
        public string payType { get; set; }
        public string extraData { get; set; }
        public long responseTime { get; set; }
        public string signature { get; set; }
    }
}
