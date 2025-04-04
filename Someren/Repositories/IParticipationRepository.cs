using Someren.Models;

namespace Someren.Repositories
{
    public interface IParticipationRepository
    {
        void AddParticipation(int activityId, int studentId);
        void RemoveParticipation(int activityId, int studentId);
        List<Student> GetStudentsByActivityId(int activityId);
    }
}