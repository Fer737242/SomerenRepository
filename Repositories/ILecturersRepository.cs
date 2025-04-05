using Someren.Models;

namespace Someren.Repositories
{
    public interface ILecturersRepository
    {
        List<Lecturer> GetAll();
        Lecturer? GetByEmployeeN(int employeeId);
        void Add(Lecturer lecturer);
        void Update(Lecturer lecturer);
        void Delete(Lecturer lecturer);
    }
}
