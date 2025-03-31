using System;
using Someren.Models;

namespace Someren.Repositories
{
    public interface IStudentsRepository
    {
        List<Student> GetAll();
        Student? GetBystudent_n(int student_n);
        void Add(Student Student);
        void Update(Student Student);
        void Delete(Student Student);
        bool DeductVoucher(int student_n, int quantity);       
        void AddVoucher(int student_n, int count);
    }
}
