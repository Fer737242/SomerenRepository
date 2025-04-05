using System;
using Microsoft.Data.SqlClient;
using Someren.Models;

namespace Someren.Repositories
{
    public class DbStudentsRepository : IStudentsRepository
    {
        private readonly string? _connectionString;

        public DbStudentsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("projectdatabase");
        }

        private Student ReadStudent(SqlDataReader reader)
        {
            int student_n = (int)reader["Student_n"];
            string first_name = (string)reader["first_name"];
            string last_name = (string)reader["last_name"];
            int phone_n = (int)reader["phone_n"];
            int classN = (int)reader["classN"];
            //string isDeleted = (string)reader["IsDeleted"];

            return new Student(student_n, first_name, last_name, phone_n, classN); //isDeleted);
        }

        public List<Student> GetAll()
        {
            List<Student> Student = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Student_n, first_name, last_name, phone_n, classN FROM STUDENT ORDER BY last_name ASC";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Student Students = ReadStudent(reader);
                    Student.Add(Students);
                }
                reader.Close();
            }
            return Student;
        }

        public void Add(Student Student)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO STUDENT (first_name, last_name, phone_n, classN) 
                VALUES (@first_name, @last_name, @phone_n, @classN);";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@first_name", Student.First_name);
                command.Parameters.AddWithValue("@last_name", Student.Last_name);
                command.Parameters.AddWithValue("@phone_n", Student.Phone_n);
                command.Parameters.AddWithValue("@classN", Student.ClassN);


                //command.Parameters.AddWithValue("@IsDeleted", user.IsDeleted);

                connection.Open();
                object result = command.ExecuteScalar();  // Get new EmployeeN
                if (result != null)
                {
                    Student.Student_n = Convert.ToInt32(result);
                }
            }
        }

        public void Update(Student Student)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE STUDENT SET first_name = @first_name, last_name = @last_name, phone_n = @phone_n WHERE Student_n = @Student_n";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Student_n", Student.Student_n);
                command.Parameters.AddWithValue("@first_name", Student.First_name);
                command.Parameters.AddWithValue("@last_name", Student.Last_name);
                command.Parameters.AddWithValue("@phone_n", Student.Phone_n);
                //command.Parameters.AddWithValue("@IsDeleted", user.IsDeleted);

                command.Connection.Open();
                command.ExecuteNonQuery();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
        }

        public Student GetBystudent_n(int student_n)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT student_n, first_name, last_name, phone_n, ClassN FROM STUDENT WHERE student_n = @Student_n";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Student_n", student_n);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadStudent(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        public void Delete(Student Student)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"DELETE FROM STUDENT WHERE Student_n= @Student_n";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Student_n", Student.Student_n);

                command.Connection.Open();
                command.ExecuteNonQuery();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records deleted!");
            }
        }


    }
}

