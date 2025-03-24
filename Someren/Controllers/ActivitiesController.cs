using Microsoft.AspNetCore.Mvc;
using Someren.Models;
using Someren.Repositories;

namespace Someren.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly IActivitiesRepository _activitiesRepository;

        public ActivitiesController(IConfiguration configuration)
        {
            _activitiesRepository = new DbActivitiesRepository(configuration);
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
    }
}
