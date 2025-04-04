using Microsoft.AspNetCore.Mvc;
using Someren.Models;
using Someren.Repositories;

namespace Someren.Controllers
{
    public class DrinksController : Controller
    {
        private readonly IDrinkRepository _drinkRepository;
        private readonly IStudentsRepository _studentsRepository;

        public DrinksController(IDrinkRepository drinkRepository, IStudentsRepository studentsRepository)
        {
            _drinkRepository = drinkRepository;
            _studentsRepository = studentsRepository;
        }

        public IActionResult Index()
        {
            List<Drink> drinks = _drinkRepository.GetAll();
            return View(drinks);
        }

        [HttpGet]
        public IActionResult Order()
        {
            var model = new OrderViewModel
            {
                Students = _studentsRepository.GetAll(),
                Drinks = _drinkRepository.GetAll()
            };
            return View(model);
        }

     
        [HttpPost]
        public IActionResult Order(int student_n, Dictionary<int, int> Drinks)
        {
            try
            {
                // Validate input
                if (student_n <= 0 && Drinks == null && Drinks.Count == 0)
                {
                    return Json(new { success = false, message = "Invalid student number or no drinks selected." });
                }

                // Retrieve student data
                var student = _studentsRepository.GetBystudent_n(student_n);

                if (student == null)
                {
                    return Json(new { success = false, message = $"Student with ID {student_n} not found." });
                }

                // Calculate total vouchers required
                int totalVouchersRequired = Drinks.Values.Sum();

                // Check if student has enough vouchers
                if (student.VoucherCount < totalVouchersRequired)
                {
                    return Json(new { success = false, message = $"Insufficient vouchers: Required {totalVouchersRequired}, but only {student.VoucherCount} available." });
                }

                // Attempt to deduct vouchers
                bool isVoucherDeducted = _studentsRepository.DeductVoucher(student_n, totalVouchersRequired);
                if (!isVoucherDeducted)
                {
                    return Json(new { success = false, message = $"Voucher deduction failed for student {student_n}." });
                }

                // Refresh student data after voucher deduction
                student = _studentsRepository.GetBystudent_n(student_n);

                // Return a success response in JSON format
                return Json(new
                {
                    success = true,
                    message = $"Order placed successfully! Remaining vouchers: {student.VoucherCount}",
                    orderSummary = Drinks,
                    remainingVouchers = student.VoucherCount
                });
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine($"STACK TRACE: {ex.StackTrace}");

                // Return the error as a JSON response
                return Json(new { success = false, message = $"An unexpected error occurred: {ex.Message}" });
            }
        }
        [HttpPost]
        public IActionResult AddVoucher(int student_n, int count)
        {
            var student = _studentsRepository.GetBystudent_n(student_n);
            if (student == null)
            {
                TempData["Error"] = "Student not found.";
                return RedirectToAction("Order");
            }

            _studentsRepository.AddVoucher(student_n, count);

            TempData["Success"] = $"{count} vouchers added successfully!";

            return RedirectToAction("Order");
        }
    }
}