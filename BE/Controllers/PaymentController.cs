//using BE.Data;
//using BE.Dtos.Requests;
//using BE.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Cryptography;
//using System.Text;
//using System.Text.Json;

//namespace BE.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PaymentController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly IConfiguration _config;

//        public PaymentController(ApplicationDbContext context, IConfiguration config)
//        {
//            _context = context;
//            _config = config;
//        }

//        [HttpPost("momo")]
//        public async Task<IActionResult> PayWithMoMo([FromBody] PaymentRequest model)
//        {
//            if (model.amount <= 0)
//                return BadRequest(new { message = "Amount must be greater than zero." });

//            // 1️⃣ Tạo Payment trước
//            var payment = new Payment
//            {
//                UserId = model.userId,
//                CourseId = model.courseId,
//                Amount = model.amount,
//                OrderId = Guid.NewGuid().ToString(),
//                IsPaid = false,
//                CreatedAt = DateTime.Now
//            };

//            _context.Payments.Add(payment);
//            await _context.SaveChangesAsync();

//            // 2️⃣ Chuẩn bị request gửi đến MoMo
//            string endpoint = _config["MoMo:Endpoint"];
//            string partnerCode = _config["MoMo:PartnerCode"];
//            string accessKey = _config["MoMo:AccessKey"];
//            string secretKey = _config["MoMo:SecretKey"];
//            string returnUrl = _config["MoMo:ReturnUrl"];
//            string notifyUrl = _config["MoMo:NotifyUrl"];

//            string requestId = Guid.NewGuid().ToString();
//            string orderInfo = $"Thanh toán khóa học {payment.CourseId}";
//            string amount = ((long)payment.Amount).ToString(); // MoMo chỉ nhận integer
//            string extraData = "";

//            // 3️⃣ Tạo raw signature CHUẨN MoMo V2
//            // Trong action PayWithMoMo
//            string rawHash =
//                $"accessKey={accessKey}" +
//                $"&amount={amount}" +
//                $"&extraData={extraData}" +
//                $"&ipnUrl={notifyUrl}" +           // ← PHẢI ĐẶT TRƯỚC orderId
//                $"&orderId={payment.OrderId}" +
//                $"&orderInfo={orderInfo}" +
//                $"&partnerCode={partnerCode}" +
//                $"&redirectUrl={returnUrl}" +
//                $"&requestId={requestId}" +
//                $"&requestType=captureWallet";     // ← không cần & ở cuối

//            string signature = SignHmacSHA256(rawHash, secretKey);

//            var momoRequest = new
//            {
//                partnerCode,
//                accessKey,
//                requestId,
//                amount,
//                orderId = payment.OrderId,
//                orderInfo,
//                redirectUrl = returnUrl,
//                ipnUrl = notifyUrl,
//                requestType = "captureWallet",
//                extraData,
//                signature
//            };

//            using var client = new HttpClient();
//            var resp = await client.PostAsJsonAsync(endpoint, momoRequest);
//            var json = await resp.Content.ReadAsStringAsync();

//            // 4️⃣ Đọc response
//            using var doc = JsonDocument.Parse(json);
//            if (!doc.RootElement.TryGetProperty("payUrl", out var url))
//                return BadRequest(new { message = "MoMo error", response = json });

//            return Ok(new { payUrl = url.GetString() });
//        }
//        [HttpPost("momo-callback")]
//        public async Task<IActionResult> MoMoCallback([FromBody] JsonElement data)
//        {
//            try
//            {
//                string orderId = data.GetProperty("orderId").GetString()!;
//                int resultCode = data.GetProperty("resultCode").GetInt32();

//                var payment = await _context.Payments
//                    .FirstOrDefaultAsync(p => p.OrderId == orderId);

//                if (payment == null)
//                    return NotFound("Payment not found: " + orderId);

//                if (resultCode == 0)
//                {
//                    payment.IsPaid = true;

//                    var registration = await _context.Registrations
//                        .FirstOrDefaultAsync(r => r.UserId == payment.UserId && r.CourseId == payment.CourseId);

//                    if (registration != null)
//                    {
//                        registration.Status = 1; // Đánh dấu đã thanh toán
//                    }

//                    await _context.SaveChangesAsync(); 
//                }

//                return Ok(new { message = "Callback processed", orderId });
//            }
//            catch (Exception ex)
//            {
//                // LOG LỖI ĐỂ BIẾT TẠI SAO KHÔNG SAVE
//                Console.WriteLine("MO MO CALLBACK ERROR: " + ex.Message);
//                return StatusCode(500, new { error = ex.Message });
//            }
//        }


//        private string SignHmacSHA256(string message, string key)
//        {
//            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
//            return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(message))).Replace("-", "").ToLower();
//        }
//    }
//}

using BE.Data;
using BE.Dtos;
using BE.Dtos.Requests;
using BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public PaymentController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("momo")]
        public async Task<IActionResult> PayWithMoMo([FromBody] PaymentRequest model)
        {
            if (model.amount <= 0)
                return BadRequest(new { message = "Amount must be greater than zero." });

            var payment = new Payment
            {
                UserId = model.userId,
                CourseId = model.courseId,
                Amount = model.amount,
                OrderId = Guid.NewGuid().ToString(),
                IsPaid = false,
                CreatedAt = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Chuẩn bị MoMo request
            string endpoint = _config["MoMo:Endpoint"];
            string partnerCode = _config["MoMo:PartnerCode"];
            string accessKey = _config["MoMo:AccessKey"];
            string secretKey = _config["MoMo:SecretKey"];
            string returnUrl = _config["MoMo:ReturnUrl"];
            string notifyUrl = _config["MoMo:NotifyUrl"];

            string requestId = Guid.NewGuid().ToString();
            string orderInfo = $"Thanh toán khóa học {payment.CourseId}";
            string amount = ((long)payment.Amount).ToString();
            string extraData = "";

            string rawHash =
                $"accessKey={accessKey}" +
                $"&amount={amount}" +
                $"&extraData={extraData}" +
                $"&ipnUrl={notifyUrl}" +
                $"&orderId={payment.OrderId}" +
                $"&orderInfo={orderInfo}" +
                $"&partnerCode={partnerCode}" +
                $"&redirectUrl={returnUrl}" +
                $"&requestId={requestId}" +
                $"&requestType=captureWallet";

            string signature = SignHmacSHA256(rawHash, secretKey);

            var momoRequest = new
            {
                partnerCode,
                accessKey,
                requestId,
                amount,
                orderId = payment.OrderId,
                orderInfo,
                redirectUrl = returnUrl,
                ipnUrl = notifyUrl,
                requestType = "captureWallet",
                extraData,
                signature
            };

            using var client = new HttpClient();
            var resp = await client.PostAsJsonAsync(endpoint, momoRequest);
            var json = await resp.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("payUrl", out var url))
                return BadRequest(new { message = "MoMo error", response = json });

            return Ok(new { payUrl = url.GetString() });
        }

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetPaymentHistory(Guid userId)
        {
            var payments = await _context.Payments
                .Where(p => p.UserId == userId)
                .Select(p => new PaymentRequest
                {
                    userId = p.UserId,
                    courseId = p.CourseId,
                    amount = p.Amount,
                    isPaid = p.IsPaid
                })
                .ToListAsync();

            return Ok(payments);
        }

        [HttpPost("momo-callback")]
        public async Task<IActionResult> MoMoCallback([FromBody] JsonElement data)
        {
            try
            {
                string orderId = data.GetProperty("orderId").GetString()!;
                int resultCode = int.Parse(data.GetProperty("resultCode").GetString()!);

                var payment = await _context.Payments
                    .Include(p => p.Course)
                    .FirstOrDefaultAsync(p => p.OrderId == orderId);

                if (payment == null)
                    return NotFound("Payment not found");

                if (resultCode == 0 && !payment.IsPaid)
                {
                    payment.IsPaid = true;

                    var registration = await _context.Registrations
                        .FirstOrDefaultAsync(r => r.UserId == payment.UserId && r.CourseId == payment.CourseId);

                    if (registration != null)
                        registration.Status = 1;

                    await _context.SaveChangesAsync();
                }

                // Trả PaymentRequest để FE cập nhật UI
                return Ok(new PaymentRequest
                {
                    userId = payment.UserId,
                    courseId = payment.CourseId,
                    amount = payment.Amount,
                    isPaid = payment.IsPaid
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet("details/{courseId}/{userId}")]
        public async Task<IActionResult> GetPaymentDetails(Guid courseId, Guid userId)
        {
            var payment = await _context.Payments
                .Include(p => p.Course)
                .FirstOrDefaultAsync(p => p.CourseId == courseId && p.UserId == userId);

            if (payment == null)
                return NotFound();

            var model = new PaymentDTO
            {
                UserId = payment.UserId,
                CourseId = payment.CourseId,
                CourseTitle = payment.Course.Title,
                SubjectName = payment.Course.SubjectName,
                Fee = payment.Amount,
                IsPaid = payment.IsPaid,
                PaymentDate = payment.CreatedAt,
                TransactionId = payment.OrderId,
                StartDate = payment.Course.StartDate,
                EndDate = payment.Course.EndDate
            };

            return Ok(model);
        }

        //[HttpGet("details/{orderId}/{userId}")]
        //public async Task<IActionResult> GetPaymentDetails(string orderId, Guid userId)
        //{
        //    var payment = await _context.Payments
        //        .Include(p => p.Course)
        //        .FirstOrDefaultAsync(p => p.OrderId == orderId && p.UserId == userId);

        //    if (payment == null)
        //        return NotFound();

        //    var model = new PaymentDTO
        //    {
        //        UserId = payment.UserId,
        //        CourseId = payment.CourseId,
        //        CourseTitle = payment.Course.Title,
        //        SubjectName = payment.Course.SubjectName,
        //        Fee = payment.Amount,
        //        IsPaid = payment.IsPaid,
        //        PaymentDate = payment.CreatedAt,
        //        TransactionId = payment.OrderId,
        //        StartDate = payment.Course.StartDate,
        //        EndDate = payment.Course.EndDate
        //    };

        //    return Ok(model);
        //}

        private string SignHmacSHA256(string message, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(message))).Replace("-", "").ToLower();
        }
    }
}
