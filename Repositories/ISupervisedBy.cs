using Someren.Models;

namespace Someren.Repositories
{
    public interface ISupervisedBy
    {
        //Lecturer? GetByEmployeeId(int employeeId);
        List<Lecturer> GetSupervisorByActivityId(int activityId);
        void AddSupervisor(int activityId, int employeeId);
        void DeleteSupervisor(int activityId, int employeeId);
    }
}
