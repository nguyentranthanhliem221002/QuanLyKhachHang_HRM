using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace FE.Services
{
    public class PaymentService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        public PaymentService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }

        /// <summary>
        /// Tạo yêu cầu thanh toán MoMo (sandbox)
        /// </summary>
        public async Task<string> CreateMoMoPayment(string orderId, decimal amount, string orderInfo)
        {
            var momo = _config.GetSection("MoMo");

            string endpoint = momo["Endpoint"] ?? "https://test-payment.momo.vn/v2/gateway/api/create";
            string partnerCode = momo["PartnerCode"];
            string accessKey = momo["AccessKey"];
            string secretKey = momo["SecretKey"];
            string returnUrl = momo["ReturnUrl"];
            string notifyUrl = momo["NotifyUrl"];
            string requestType = "captureWallet";

            // 🔹 Chuỗi cần ký (signature)
            string rawHash =
                $"accessKey={accessKey}&amount={amount}&extraData=&ipnUrl={notifyUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={returnUrl}&requestId={orderId}&requestType={requestType}";

            string signature = SignSHA256(rawHash, secretKey);

            var requestBody = new
            {
                partnerCode,
                accessKey,
                requestId = orderId,
                amount = amount.ToString(),
                orderId,
                orderInfo,
                redirectUrl = returnUrl,
                ipnUrl = notifyUrl,
                extraData = "",
                requestType,
                signature
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 🔹 Gửi request sang MoMo
            var response = await _client.PostAsync(endpoint, content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"MoMo API lỗi ({response.StatusCode}): {result}");

            // 🔹 Parse kết quả trả về
            dynamic data = JsonConvert.DeserializeObject(result);

            // ✅ Trả về payUrl nếu có
            if (data != null && data.payUrl != null)
            {
                return data.payUrl.ToString();
            }

            throw new Exception($"Không nhận được payUrl từ MoMo. Phản hồi: {result}");
        }

        /// <summary>
        /// Hàm ký SHA256 cho dữ liệu gửi đi MoMo
        /// </summary>
        private string SignSHA256(string data, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(dataBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
