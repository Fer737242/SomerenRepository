using Someren.Models;
using System.Collections.Generic;

namespace Someren.Repositories
{
    public interface IActivitiesRepository
    {
        List<Activities> GetAll();
        void Add(Activities activity);
        void Update(Activities activity);
        Activities GetByActivityId(int activityId);
        void Delete(Activities activity);
    }
}
