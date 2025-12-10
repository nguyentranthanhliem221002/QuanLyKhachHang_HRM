using FE.Models;
using FE.Models.Requests;
using System.Net.Http.Json;

namespace FE.Services
{
    public class PaymentService
    {
        private readonly HttpClient _http;

        public PaymentService(HttpClient http)
        {
            _http = http;
        }

        private class MoMoResponse
        {
            public string payUrl { get; set; }
        }

        public async Task<string?> PayWithMoMoAsync(PaymentRequest model)
        {
            try
            {
                var request = new
                {
                    userId = model.UserId,
                    courseId = model.CourseId,
                    amount = model.Amount
                };

                var response = await _http.PostAsJsonAsync("api/payment/momo", request);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Lỗi MoMo: " + error);
                    return null;
                }

                var result = await response.Content.ReadFromJsonAsync<MoMoResponse>();
                return result?.payUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine("PayWithMoMoAsync exception: " + ex.Message);
                return null;
            }
        }

        public async Task<List<PaymentRequest>> GetHistoryAsync(Guid userId)
        {
            var response = await _http.GetFromJsonAsync<List<PaymentRequest>>($"api/payment/history/{userId}");
            return response ?? new List<PaymentRequest>();
        }

        public async Task<PaymentRequest?> UpdatePaymentStatusAsync(string orderId, string resultCode)
        {
            try
            {
                var request = new
                {
                    orderId,
                    resultCode
                };

                var response = await _http.PostAsJsonAsync("api/payment/momo-callback", request);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Lỗi cập nhật payment: " + error);
                    return null;
                }

                // Backend trả về PaymentRequest đã update
                var updatedPayment = await response.Content.ReadFromJsonAsync<PaymentRequest>();
                return updatedPayment;
            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdatePaymentStatusAsync exception: " + ex.Message);
                return null;
            }
        }

        public async Task<PaymentViewModel?> GetPaymentDetailsAsync(Guid courseId, Guid userId)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<PaymentViewModel>($"api/payment/details/{courseId}/{userId}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPaymentDetailsAsync exception: " + ex.Message);
                return null;
            }
        }
        //public async Task<PaymentViewModel?> GetPaymentDetailsAsync(string orderId, Guid userId)
        //{
        //    try
        //    {
        //        var response = await _http.GetFromJsonAsync<PaymentViewModel>($"api/payment/details/{orderId}/{userId}");
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("GetPaymentDetailsAsync exception: " + ex.Message);
        //        return null;
        //    }
        //}

        public async Task<List<PaymentViewModel>> GetAllPaymentsAsync()
        {
            return await _http.GetFromJsonAsync<List<PaymentViewModel>>("api/payment/list");
        }


    }
}
