using Microsoft.Data.SqlClient;
using Someren.Models;

namespace Someren.Repositories
{
    public class DbStudentDormitoriesRepository : IStudentDormitoriesRepository
    {
        private readonly string? _connectionString;

        public DbStudentDormitoriesRepository(IConfiguration configuration)
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

            return new Student(student_n, first_name, last_name, phone_n, classN, voucherCount);
        }

        private Room ReadRoom(SqlDataReader reader)
        {
            string roomNumber = (string)reader["room_number"];
            string type = (string)reader["type"];
            int size = (int)reader["size"];

            return new Room(roomNumber, type, size);
        }

        private StudentDormitory ReadStudentDormitory(SqlDataReader reader)
        {
            string studentDormitoryNumber = (string)reader["studentDormitory_number"];
            int studentNumber = (int)reader["student_n"];

            return new StudentDormitory(studentDormitoryNumber, studentNumber);
        }

        public void Add(StudentDormitory studentDormitory)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Student_Dormitory (room_number, student_number) VALUES (@roomNumber, @studentNumber)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@roomNumber", studentDormitory.RoomNumber);
                command.Parameters.AddWithValue("@studentNumber", studentDormitory.StudentNumber);

                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Student> GetStudentsByRoomNumber(string roomNumber)
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT STUDENT.Student_n, STUDENT.first_name, STUDENT.last_name, STUDENT.phone_n, STUDENT.classN, STUDENT.VoucherCount FROM STUDENT INNER JOIN Student_Dormitory ON STUDENT.Student_n = Student_Dormitory.student_number WHERE Student_Dormitory.room_number = @roomNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@roomNumber", roomNumber);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Student student = ReadStudent(reader);
                    students.Add(student);
                }
                reader.Close();
            }

            return students;
        }

        public List<Student> GetStudentsWithoutARoom()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT STUDENT.student_n, STUDENT.first_name, STUDENT.last_name, STUDENT.phone_n, STUDENT.classN, STUDENT.VoucherCount FROM STUDENT LEFT JOIN Student_Dormitory ON STUDENT.Student_n = Student_Dormitory.student_number WHERE student_dormitory.room_number IS NULL";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Student student = ReadStudent(reader);
                    students.Add(student);
                }
                reader.Close();
            }

            return students;
        }

        public Room GetRoomByStudentNumber(int studentNumber)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT ROOM.room_number, ROOM.type, ROOM.size FROM ROOM, student_dormitory WHERE student_dormitory.student_number = @studentNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@studentNumber", studentNumber);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadRoom(reader);
                }
                else
                {
                    return null;
                }
            }

        }

        public void DeleteBYStudentNumber(int studentNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"DELETE FROM student_dormitory WHERE student_number = @studentNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@studentNumber", studentNumber);

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records deleted!");
            }
        }
    }
}

