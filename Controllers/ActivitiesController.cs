﻿using Microsoft.AspNetCore.Mvc;
using Someren.Models;
using Someren.Repositories;

namespace Someren.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly IActivitiesRepository _activitiesRepository;
        private readonly ISupervisedBy _supervisedByRepository;
        private readonly ILecturersRepository _lecturersRepository;

        public ActivitiesController(IActivitiesRepository activitiesRepository, ISupervisedBy supervisedByRepository, ILecturersRepository lecturersRepository)
        {
            _activitiesRepository = activitiesRepository;
            _supervisedByRepository = supervisedByRepository;
            _lecturersRepository = lecturersRepository;
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

        public IActionResult SupervisedBy(int id)
        {
            var activity = _activitiesRepository.GetByActivityId(id);
            if (activity == null)
            {
                return NotFound();
            }

            var supervisingLecturers = _supervisedByRepository.GetSupervisorByActivityId(id);
            var allLecturers = _lecturersRepository.GetAll();

            var supervisingLecturersIds = new HashSet<int>(supervisingLecturers.Select(s => s.EmployeeN));
            var nonSupervisingLecturers = allLecturers
                .Where(s => !supervisingLecturersIds.Contains(s.EmployeeN))
                .ToList();

            var viewModel = new SupervisedByViewModel
            {
                Activity = activity,
                SupervisingLecturers = supervisingLecturers,
                NonSupervisingLecturers = nonSupervisingLecturers
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddSupervisor(int activityId, int employeeId)
        {
            try
            {
                _supervisedByRepository.AddSupervisor(activityId, employeeId);

                var lecturer = _lecturersRepository.GetByEmployeeN(employeeId);
                if (lecturer != null)
                {
                    TempData["SuccessMessage"] = $"{lecturer.FirstName} {lecturer.LastName} has been added to the activity.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("SupervisedBy", new { id = activityId });
        }

        [HttpPost]
        public IActionResult DeleteSupervisor(int activityId, int employeeId)
        {
            _supervisedByRepository.DeleteSupervisor(activityId, employeeId);

            var lecturer = _lecturersRepository.GetByEmployeeN(employeeId);
            if (lecturer != null)
            {
                TempData["SuccessMessage"] = $"{lecturer.FirstName} {lecturer.LastName} has been deleted from the activity.";
            }

            return RedirectToAction("SupervisedBy", new { id = activityId });
        }
    }
}
