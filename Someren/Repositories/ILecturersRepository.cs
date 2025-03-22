using Someren.Models;

namespace Someren.Repositories
{
    public interface ILecturersRepository
    {
        List<Lecturer> GetAll();
        Lecturer? GetByEmployeeN(int employee_n);
        void Add(Lecturer Lecturer);
        void Update(Lecturer Lecturer);
        void Delete(Lecturer Lecturer);
    }
}
