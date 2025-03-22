using Microsoft.AspNetCore.Mvc;
using Someren.Models;
using Someren.Repositories;

namespace Someren.Controllers
{
    public class LecturersController : Controller
    {
        private readonly ILecturersRepository _lecturersRepository;

        public LecturersController(IConfiguration configuration)
        {
            _lecturersRepository = new DbLecturersRepository(configuration);
        }
        public IActionResult Index()
        {
            List<Lecturer> lecturers = _lecturersRepository.GetAll();
            return View(lecturers);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Lecturer lecturer)
        {

            try
            {
                _lecturersRepository.Add(lecturer);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(lecturer);
            }
        }
        [HttpGet]
        public ActionResult Edit(int? EmployeeN)
        {
            if (EmployeeN == null)
            {
                return NotFound();
            }

            Lecturer? lecturer = _lecturersRepository.GetByEmployeeN((int)EmployeeN);
            return View(lecturer);
        }
        [HttpPost]
        public ActionResult Edit(Lecturer lecturer)
        {
            try
            {
                _lecturersRepository.Update(lecturer);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(lecturer);
            }
        }
        [HttpGet]
        public ActionResult Delete(int? EmployeeN)
        {
            if (EmployeeN == null)
            {
                return NotFound();
            }
            Lecturer? lecturer = _lecturersRepository.GetByEmployeeN((int)EmployeeN);
            return View(lecturer);
        }
        [HttpPost]
        public ActionResult Delete(Lecturer lecturer)
        {
            try
            {
                _lecturersRepository.Delete(lecturer);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(lecturer);
            }
        }
    }
}