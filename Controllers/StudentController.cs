using Microsoft.AspNetCore.Mvc;
using Someren.Models;
using Someren.Repositories;

namespace Someren.Controllers {

    public class StudentsController : Controller
    {
        private readonly IStudentsRepository _studentsRepository;

        public StudentsController(IConfiguration configuration)
        {
            _studentsRepository = new DbStudentsRepository(configuration);
        }

        //public StudentsController(IStudentsRepository studentsRepository)
        //{
        //    _studentsRepository = studentsRepository;
        //}

        public IActionResult Index()
        {
            List<Student> students = _studentsRepository.GetAll();
            return View(students);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                _studentsRepository.Add(student);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the student.");
                return View(student);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? Student_n)
        {
            if (Student_n == null)
            {
                return NotFound();
            }

            Student? student = _studentsRepository.GetBystudent_n((int)Student_n);
            return View(student);
        }

        [HttpPost]
        public ActionResult Edit(Student student)
        {
            try
            {
                _studentsRepository.Update(student);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(student);
            }
        }

        [HttpGet]
        public ActionResult Delete(int? Student_n)
        {
            if (Student_n == null)
            {
                return NotFound();
            }

            Student? student = _studentsRepository.GetBystudent_n((int)Student_n);
            return View(student);
        }

        [HttpPost]
        public ActionResult Delete(Student student)
        {
            try
            {
                _studentsRepository.Delete(student);
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return View(student);
            }
        }
    }
}

