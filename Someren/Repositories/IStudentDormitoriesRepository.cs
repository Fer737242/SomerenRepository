using Someren.Models;

namespace Someren.Repositories
{
    public interface IStudentDormitoriesRepository
    {
        List<Student> GetStudentsByRoomNumber(string roomNumber);
        public List<Student> GetStudentsWithoutARoom();
        Room? GetRoomByStudentNumber(int studentNumber);
        void Add(StudentDormitory studentDormitory);
        void DeleteBYStudentNumber(int studentNumber);
    }
}
