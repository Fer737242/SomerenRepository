namespace Someren.Models
{
    public class SupervisedByViewModel
    {
        public Activities Activity { get; set; }
        public List<Lecturer> SupervisingLecturers { get; set; }
        public List<Lecturer> NonSupervisingLecturers { get; set; }
    }
}
