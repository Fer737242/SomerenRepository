namespace Someren.Models
{
    public class StudentDormitory
    {
            public string RoomNumber { get; set; }
            public int StudentNumber { get; set; }

        public StudentDormitory() { }

        public StudentDormitory(string roomNumber, int studentNumber)
        {
            RoomNumber = roomNumber;
            StudentNumber = studentNumber;
        }

    }
}
