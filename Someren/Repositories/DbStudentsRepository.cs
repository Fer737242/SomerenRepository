using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Someren.Models;

namespace Someren.Repositories
{
    public class DbStudentsRepository : IStudentsRepository
    {
        private readonly string? _connectionString;
        private int voucherCount;

        public DbStudentsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("projectdatabase");
        }

        private Student ReadStudent(SqlDataReader reader)
        {
            int student_n = (int)reader["student_n"];
            string first_name = (string)reader["first_name"];
            string last_name = (string)reader["last_name"];
            int phone_n = (int)reader["phone_n"];
            int classN = (int)reader["classN"];
            int voucherCount = (int)reader["VoucherCount"];
            //string isDeleted = (string)reader["IsDeleted"];

            return new Student(student_n, first_name, last_name, phone_n, classN, voucherCount); //isDeleted);
        }

        public List<Student> GetAll()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Use the correct column name: student_n
                string query = "SELECT student_n, first_name, last_name, phone_n, classN, VoucherCount FROM STUDENT ORDER BY last_name ASC";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        Student_n = (int)reader["student_n"], // Use the correct column name
                        First_name = reader["first_name"].ToString(),
                        Last_name = reader["last_name"].ToString(),
                        Phone_n = (int)reader["phone_n"],
                        ClassN = (int)reader["classN"],
                        VoucherCount = (int)reader["VoucherCount"]
                    });
                }
                reader.Close();
            }

            return students;
        }

        public void Add(Student Student)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO STUDENT (first_name, last_name, phone_n, classN, VoucherCount) 
                    VALUES (@first_name, @last_name, @phone_n, @classN, 1);";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@first_name", Student.First_name);
                command.Parameters.AddWithValue("@last_name", Student.Last_name);
                command.Parameters.AddWithValue("@phone_n", Student.Phone_n);
                command.Parameters.AddWithValue("@classN", Student.ClassN);

                connection.Open();
                object result = command.ExecuteScalar();
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
                string query = "UPDATE STUDENT SET first_name = @first_name, last_name = @last_name, phone_n = @phone_n WHERE student_n = @student_n";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@student_n", Student.Student_n);
                command.Parameters.AddWithValue("@first_name", Student.First_name);
                command.Parameters.AddWithValue("@last_name", Student.Last_name);
                command.Parameters.AddWithValue("@phone_n", Student.Phone_n);

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
        }

        public void Delete(Student Student)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"DELETE FROM STUDENT WHERE student_n= @student_n";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@student_n", Student.Student_n);

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records deleted!");
            }
        }

        public void AddVoucher(int student_n, int count)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Use the correct column name: student_n
                string query = "UPDATE STUDENT SET VoucherCount = VoucherCount + @Count WHERE student_n = @student_n";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@student_n", student_n);
                command.Parameters.AddWithValue("@Count", count);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new Exception("Failed to add vouchers.");
                }
            }
        }

        public bool DeductVoucher(int student_n, int count)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"
                        UPDATE STUDENT
                        SET VoucherCount = VoucherCount - @count
                        WHERE student_n = @student_n AND VoucherCount >= @count;
                    ";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@count", count);
                    command.Parameters.AddWithValue("@student_n", student_n);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"DEBUG: DeductVoucher - Rows affected: {rowsAffected}");
                    return rowsAffected > 0; // Return true if the update was successful
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: An exception occurred in DeductVoucher. Details: {ex.Message}");
                return false;
            }
        }

        public Student GetBystudent_n(int student_n)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"
                        SELECT student_n, first_name, last_name, phone_n, classN, VoucherCount 
                        FROM STUDENT 
                        WHERE student_n = @student_n;
                    ";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@student_n", student_n);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        Console.WriteLine($"DEBUG: Successfully retrieved student data for student {student_n}.");
                        return new Student
                        {
                            Student_n = (int)reader["student_n"],
                            First_name = reader["first_name"].ToString(),
                            Last_name = reader["last_name"].ToString(),
                            Phone_n = (int)reader["phone_n"],
                            ClassN = (int)reader["classN"],
                            VoucherCount = (int)reader["VoucherCount"]
                        };
                    }
                    else
                    {
                        Console.WriteLine($"ERROR: Student with ID {student_n} not found.");
                        return null; // Return null instead of throwing an exception
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"ERROR: A database-related exception occurred in GetBystudent_n. Details: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: An exception occurred in GetBystudent_n. Details: {ex.Message}");
                throw;
            }
        }

    }
}

