using Microsoft.Data.SqlClient;
using Someren.Models;

namespace Someren.Repositories
{
    public class DbLecturersRepository : ILecturersRepository
    {
        private readonly string? _connectionString;

        public DbLecturersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("projectdatabase");
        }

        private Lecturer ReadLecturer(SqlDataReader reader)
        {
            int employeeN = (int)reader["employee_n"];
            string first_name = (string)reader["first_name"];
            string last_name = (string)reader["last_name"];
            string phone_n = (string)reader["phone_n"];
            string age = (string)reader["age"];
            //string isDeleted = (string)reader["IsDeleted"];

            return new Lecturer(employeeN, first_name, last_name, phone_n, age); //isDeleted);
        }

        public List<Lecturer> GetAll()
        {
            List<Lecturer> lecturers = new List<Lecturer>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT employee_n, first_name, last_name, phone_n, age FROM LECTURER ORDER BY last_name ASC";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Lecturer lecturer = ReadLecturer(reader);
                    lecturers.Add(lecturer);
                }
                reader.Close();
            }
            return lecturers;
        }

        public void Add(Lecturer lecturer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO LECTURER (first_name, last_name, phone_n, age) VALUES (@first_name, @last_name, @phone_n, @age);
            SELECT SCOPE_IDENTITY();";  // Returns the new EmployeeN

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@first_name", lecturer.FirstName);
                command.Parameters.AddWithValue("@last_name", lecturer.LastName);
                command.Parameters.AddWithValue("@phone_n", lecturer.PhoneN);
                command.Parameters.AddWithValue("@age", lecturer.Age);

                connection.Open();
                object result = command.ExecuteScalar();  // Get new EmployeeN
                if (result != null)
                {
                    lecturer.EmployeeN = Convert.ToInt32(result);
                }
            }
        }


        public void Update(Lecturer lecturer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE LECTURER SET first_name = @first_name, last_name = @last_name, phone_n = @phone_n, age = @age WHERE employee_n = @employee_n";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@employee_n", lecturer.EmployeeN);
                command.Parameters.AddWithValue("@first_name", lecturer.FirstName);
                command.Parameters.AddWithValue("@last_name", lecturer.LastName);
                command.Parameters.AddWithValue("@phone_n", lecturer.PhoneN);
                command.Parameters.AddWithValue("@age", lecturer.Age);
                //command.Parameters.AddWithValue("@IsDeleted", user.IsDeleted);

                command.Connection.Open();
                command.ExecuteNonQuery();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
        }

        public Lecturer GetByEmployeeN(int employee_n)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT employee_n, first_name, last_name, phone_n, age FROM LECTURER WHERE employee_n = @employee_n";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@employee_n", employee_n);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadLecturer(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        public void Delete(Lecturer lecturer)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"DELETE FROM LECTURER WHERE employee_n= @employee_n";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@employee_n", lecturer.EmployeeN);

                command.Connection.Open();
                command.ExecuteNonQuery();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records deleted!");
            }
        }
    }
}
