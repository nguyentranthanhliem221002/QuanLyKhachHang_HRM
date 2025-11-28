using FE.Models;
using FE.Models.Requests;
using FE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize(Roles = "Student")]
public class PaymentController : Controller
{
    private readonly PaymentService _paymentService;
    private readonly CourseService _courseService;

    public PaymentController(PaymentService paymentService, CourseService courseService)
    {
        _paymentService = paymentService;
        _courseService = courseService;
    }

    // Trang thanh toán
    public async Task<IActionResult> Index(Guid id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        if (course == null) return NotFound();

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

        var model = new PaymentRequest
        {
            CourseId = id,
            UserId = Guid.Parse(userIdClaim),
            Amount = course.Fee
        };

        return View(model);
    }

    // Tạo đơn thanh toán và redirect sang MoMo
    [HttpPost]
    public async Task<IActionResult> Pay(PaymentRequest model)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

        model.UserId = Guid.Parse(userIdClaim);

        var payUrl = await _paymentService.PayWithMoMoAsync(model);
        if (string.IsNullOrEmpty(payUrl))
        {
            TempData["Error"] = "Không thể tạo đơn thanh toán. Thử lại!";
            return RedirectToAction("Index", new { id = model.CourseId });
        }

        return Redirect(payUrl);
    }

    // Nhận callback từ MoMo (ReturnUrl)
    [AllowAnonymous]
    public async Task<IActionResult> MoMoReturn(string orderId, string resultCode, Guid courseId, Guid userId)
    {
        // Gọi backend để cập nhật trạng thái payment
        var updatedPayment = await _paymentService.UpdatePaymentStatusAsync(orderId, resultCode);

        if (updatedPayment != null && updatedPayment.IsPaid)
            TempData["Success"] = "Thanh toán MoMo thành công!";
        else
            TempData["Error"] = "Thanh toán thất bại hoặc đang xử lý!";

        return RedirectToAction("MyCourse", "Student");
    }
    public async Task<IActionResult> Details(Guid id)
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var payment = await _paymentService.GetPaymentDetailsAsync(id, userId);
        if (payment == null)
            return NotFound();

        return View(payment);
    }
    //public async Task<IActionResult> Details(string orderId)
    //{
    //    var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
    //    var payment = await _paymentService.GetPaymentDetailsAsync(orderId, userId);
    //    if (payment == null)
    //        return NotFound();

    //    return View(payment);
    //}

    public async Task<IActionResult> History()
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var payments = await _paymentService.GetHistoryAsync(userId);

        // Map sang PaymentViewModel nếu muốn hiển thị thêm CourseTitle, StartDate, EndDate
        var model = new List<PaymentViewModel>();
        foreach (var p in payments)
        {
            var course = await _courseService.GetCourseByIdAsync(p.CourseId);
            if (course != null)
            {
                model.Add(new PaymentViewModel
                {
                    UserId = p.UserId,
                    CourseId = p.CourseId,
                    CourseTitle = course.Title,
                    SubjectName = course.SubjectName,
                    Fee = p.Amount,
                    IsPaid = p.IsPaid,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate,
                    TransactionId = p.OrderId,
                    PaymentDate = DateTime.Now 
                });
            }
        }

        return View(model);
    }

}
