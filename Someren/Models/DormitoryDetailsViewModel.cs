namespace Someren.Models
{
    public class DormitoryDetailsViewModel
    {
        public Room? Room { get; set; }
        public List<Student>? DormitoryStudents { get; set; }
        public List<Student> StudentsWithoutRoom { get; set; }

    }
}
