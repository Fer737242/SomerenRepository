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
                Console.WriteLine($"DEBUG: Starting order process for student {student_n}.");

                // Retrieve student data
                var student = _studentsRepository.GetBystudent_n(student_n);
                if (student == null)
                {
                    Console.WriteLine("ERROR: Student not found.");
                    TempData["Error"] = "Student not found.";
                    return RedirectToAction("Order");
                }
                Console.WriteLine($"DEBUG: Retrieved student {student_n} with {student.VoucherCount} vouchers.");

                // Calculate total vouchers required
                int totalVouchersRequired = Drinks.Values.Sum();
                Console.WriteLine($"DEBUG: Total vouchers required: {totalVouchersRequired}");

                if (student.VoucherCount < totalVouchersRequired)
                {
                    Console.WriteLine("ERROR: Not enough vouchers.");
                    TempData["Error"] = $"Not enough vouchers. Current: {student.VoucherCount}, Required: {totalVouchersRequired}";
                    return RedirectToAction("Order");
                }

                // Deduct vouchers
                Console.WriteLine($"DEBUG: Attempting to deduct {totalVouchersRequired} vouchers for student {student_n}.");
                if (!_studentsRepository.DeductVoucher(student_n, totalVouchersRequired))
                {
                    Console.WriteLine("ERROR: Failed to deduct vouchers.");
                    TempData["Error"] = "Failed to deduct vouchers. Please try again.";
                    return RedirectToAction("Order");
                }
                Console.WriteLine($"DEBUG: Successfully deducted vouchers for student {student_n}.");

                // Refresh student data
                Console.WriteLine($"DEBUG: Refreshing student data for student {student_n}.");
                student = _studentsRepository.GetBystudent_n(student_n);
                if (student == null)
                {
                    Console.WriteLine("ERROR: Failed to refresh student data.");
                    TempData["Error"] = "Failed to refresh student data after voucher deduction.";
                    return RedirectToAction("Order");
                }
                Console.WriteLine($"DEBUG: After deduction, student {student_n} has {student.VoucherCount} vouchers remaining.");

                // Set success messages
                TempData["Success"] = $"Order placed successfully! Remaining vouchers: {student.VoucherCount}";
                TempData["OrderSummary"] = Drinks;
                TempData["RemainingVouchers"] = student.VoucherCount;

                return RedirectToAction("Index", "Students");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: An unexpected error occurred: {ex.Message}");
                TempData["Error"] = $"An unexpected error occurred: {ex.Message}";
                return RedirectToAction("Order");
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