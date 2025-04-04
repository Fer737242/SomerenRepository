using Microsoft.AspNetCore.Mvc;
using Someren.Models;
using Someren.Repositories;

namespace Someren.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly IActivitiesRepository _activitiesRepository;
        private readonly IParticipationRepository _participationRepository;
        private readonly IStudentsRepository _studentsRepository;

        public ActivitiesController(IActivitiesRepository activitiesRepository, IParticipationRepository participationRepository, IStudentsRepository studentRepository)
        {
            this._activitiesRepository = activitiesRepository;
            this._participationRepository = participationRepository;
            this._studentsRepository = studentRepository;
        }
        public IActionResult Index()
        {
            List<Activities> activities = _activitiesRepository.GetAll();
            return View(activities);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Activities());
        }

        [HttpPost]
        public ActionResult Create(Activities activity)
        {
            try
            {
                _activitiesRepository.Add(activity);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(activity);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Activities activity = _activitiesRepository.GetByActivityId(id.Value);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        [HttpPost]
        public ActionResult Edit(Activities activity)
        {
            try
            {
                _activitiesRepository.Update(activity);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(activity);
            }
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Activities activity = _activitiesRepository.GetByActivityId(id.Value);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Activities activity = _activitiesRepository.GetByActivityId(id);
                if (activity == null)
                {
                    return NotFound();
                }

                _activitiesRepository.Delete(activity);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }

        public IActionResult ManageParticipants(int activityId)
        {
            var activity = _activitiesRepository.GetByActivityId(activityId);
            if (activity == null)
            {
                return NotFound();
            }

            var participatingStudents = _participationRepository.GetStudentsByActivityId(activityId);
            var allStudents = _studentsRepository.GetAll();
            var nonParticipatingStudents = allStudents.Except(participatingStudents).ToList();

            var viewModel = new ManageParticipantsViewModel
            {
                Activity = activity,
                ParticipatingStudents = participatingStudents,
                NonParticipatingStudents = nonParticipatingStudents
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddParticipant(int activityId, int studentId)
        {
            if (studentId <= 0)
            {
                return BadRequest("Invalid student ID.");
            }

            _participationRepository.AddParticipation(activityId, studentId);

            return RedirectToAction("ManageParticipants", new { activityId = activityId });
        }

        public IActionResult RemoveParticipant(int activityId, int studentId)
        {
            if (studentId <= 0)
            {
                return BadRequest("Invalid student ID.");
            }
            _participationRepository.RemoveParticipation(activityId, studentId);
            return RedirectToAction("ManageParticipants", new { activityId = activityId });
        }
    }
}
