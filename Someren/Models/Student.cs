using System;
namespace Someren.Models
{
    public class Student
    {
        public int Student_n { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public int Phone_n { get; set; }
        public int ClassN { get; set; }
        public int VoucherCount { get; set; }


        public Student(int student_n, string first_name, string last_name, int phone_n, int classN, int voucherCount) // string age, string isDeleted)
        {
            Student_n = student_n;
            First_name = first_name;
            Last_name = last_name;
            Phone_n = phone_n;
            ClassN = classN;
            VoucherCount = voucherCount;
            //Age = age;
            //IsDeleted = isDeleted;
        }
        public Student()
        {
            VoucherCount = 1; // Default voucher count
        }

    }
}

