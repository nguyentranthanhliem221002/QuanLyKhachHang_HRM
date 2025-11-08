using Microsoft.AspNetCore.Mvc;
using FE.Services;
using FE.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FE.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;
        private readonly CourseService _courseService;

        public PaymentController(PaymentService paymentService, CourseService courseService)
        {
            _paymentService = paymentService;
            _courseService = courseService;
        }

        // 🔹 1. Hiển thị danh sách khóa học chưa thanh toán
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string studentId = User.Identity?.Name ?? "STU001"; // gán tạm cho test
            var courses = await _courseService.GetUnpaidCoursesForStudent(studentId);
            return View(courses);
        }

        // 🔹 2. Gửi yêu cầu thanh toán MoMo cho từng khóa học
        [HttpPost]
        public async Task<IActionResult> PayWithMoMo(int courseId, decimal amount)
        {
            string orderId = courseId.ToString(); // để BE dễ xác định khóa học nào
            string orderInfo = $"Thanh toán khóa học #{courseId}";

            // Gọi qua PaymentService để tạo URL thanh toán
            var payUrl = await _paymentService.CreateMoMoPayment(orderId, amount, orderInfo);

            return Redirect(payUrl); // chuyển hướng sang MoMo
        }

        // 🔹 3. Trang kết quả sau khi MoMo redirect về
        [HttpGet]
        public IActionResult Confirm(string orderId, string resultCode)
        {
            ViewBag.OrderId = orderId;

            if (resultCode == "0")
                ViewBag.Message = "✅ Thanh toán thành công!";
            else
                ViewBag.Message = "❌ Thanh toán thất bại hoặc bị hủy.";

            return View();
        }
    }
}
