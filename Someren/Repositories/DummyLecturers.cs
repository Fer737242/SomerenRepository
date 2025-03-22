//using Someren.Repositories;
//using Someren.Models;

//namespace WhatsUp.Repositories
//{
//    public class DummyLecturers : ILecturersRepository
//    {
//        private readonly List<Lecturer> lecturers; // Declare 

//        public DummyLecturers()
//        {
//            // Initialize 
//            lecturers = new List<Lecturer>
//            {
//               new Lecturer(1, "John", "Doe", "123-456-7890", 30),
//                new Lecturer(2, "Jane", "Smith", "234-567-8901", 28),
//                new Lecturer(3, "Mike", "Johnson", "345-678-9012", 35),
//                new Lecturer(4, "Emily", "Davis", "456-789-0123", 26),
//                new Lecturer(5, "Robert", "Wilson", "567-890-1234", 40)
//            };
//        }

//        public List<Lecturer> GetAll()
//        {
//            return lecturers; // Return 
//        }

//        public Lecturer? GetByEmployeeN(int employee_n)
//        {
//            return lecturers.FirstOrDefault(x => x.EmployeeN == employee_n);
//        }

//        public void Add(Lecturer lecturer)
//        {
//            throw new NotImplementedException();
//        }

//        public void Delete(Lecturer lecturer)
//        {
//            throw new NotImplementedException();
//        }

//        public void Update(Lecturer lecturer)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
