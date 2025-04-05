namespace Someren.Models
{
    public class SupervisedBy
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EmployeeN { get; set; }

        public SupervisedBy(string firstName, string lastName, int employeeN) 
        {
            FirstName = firstName;
            LastName = lastName;
            EmployeeN = employeeN;
        }
        public SupervisedBy() { }
    }
}
