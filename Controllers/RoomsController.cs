using Microsoft.AspNetCore.Mvc;
using Someren.Models;
using Someren.Repositories;

namespace Someren.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomsRepository _roomsRepository;

        public RoomsController(IConfiguration configuration)
        {
            _roomsRepository = new DbRoomsRepository(configuration);
        }
        public IActionResult Index()
        {
            List<Room> rooms = _roomsRepository.GetAll();
            return View(rooms);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Room room)
        {

            try
            {
                _roomsRepository.Add(room);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(room);
            }
        }
        [HttpGet]
        public ActionResult Edit(string? roomNumber)
        {
            if (roomNumber == null)
            {
                return NotFound();
            }

            Room? room = _roomsRepository.GetByRoomNumber(roomNumber);
            return View(room);
        }
        [HttpPost]
        public ActionResult Edit(Room room)
        {
            try
            {
                _roomsRepository.Update(room);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(room);
            }
        }


        [HttpGet]
        public ActionResult Delete(string? roomNumber)
        {
            if (roomNumber == null)
            {
                return NotFound();
            }
            Room? room = _roomsRepository.GetByRoomNumber(roomNumber);
            return View(room);
        }

        [HttpPost]
        public ActionResult Delete(Room room)
        {
            try
            {
                _roomsRepository.Delete(room);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(room);
            }
        }
    }
}
