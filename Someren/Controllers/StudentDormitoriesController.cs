using Microsoft.AspNetCore.Mvc;
using Someren.Models;
using Someren.Repositories;

namespace Someren.Controllers
{
    public class StudentDormitoriesController : Controller
    {
        private readonly IStudentDormitoriesRepository _studentDormitoriesRepository;
        private readonly IRoomsRepository _roomsRepository;

        public StudentDormitoriesController(IConfiguration configuration)
        {
            _studentDormitoriesRepository = new DbStudentDormitoriesRepository(configuration);
            _roomsRepository = new DbRoomsRepository(configuration);
        }

        


        [HttpGet]
        public ActionResult Details(string? roomNumber)
        {
            if (roomNumber == null)
            {
                return NotFound();
            }
            Room? room = _roomsRepository.GetByRoomNumber(roomNumber);
            List<Student> dormitoryStudents = _studentDormitoriesRepository.GetStudentsByRoomNumber(roomNumber);
            List<Student> studentsWithoutRoom = _studentDormitoriesRepository.GetStudentsWithoutARoom();

            var viewModel = new DormitoryDetailsViewModel
            {
                Room = room,
                DormitoryStudents = dormitoryStudents,
                StudentsWithoutRoom = studentsWithoutRoom
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult RemoveStudent(string roomNumber, int studentNumber)
        {

            try
            {
                _studentDormitoriesRepository.DeleteBYStudentNumber(studentNumber);
            }
            catch (Exception ex)
            {
                
            }

            return RedirectToAction("Details", new { roomNumber = roomNumber });
        }

        [HttpGet]
        public ActionResult AddStudent(string roomNumber, int studentNumber)
        {

            try
            {
                StudentDormitory studentDormitory = new StudentDormitory(roomNumber, studentNumber);
                _studentDormitoriesRepository.Add(studentDormitory);
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Details", new { roomNumber = roomNumber });
        }
    }
}
