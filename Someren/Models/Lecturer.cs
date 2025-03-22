namespace Someren.Models
{
    public class Lecturer
    {
        public int EmployeeN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneN { get; set; }
        public string Age { get; set; }
        //public string IsDeleted { get; set; }

        public Lecturer(int employeeN, string firstName, string lastName, string phoneN, string age) //string isDeleted)
        {
            EmployeeN = employeeN;
            FirstName = firstName;
            LastName = lastName;
            PhoneN = phoneN;
            Age = age;
            //IsDeleted = isDeleted;
        }
        public Lecturer() { }
    }
}
