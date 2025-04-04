namespace Someren.Models
{
    public class ManageParticipantsViewModel
    {
        public Activities Activity { get; set; }
        public List<Student> ParticipatingStudents { get; set; }
        public List<Student> NonParticipatingStudents { get; set; }
    }

}
