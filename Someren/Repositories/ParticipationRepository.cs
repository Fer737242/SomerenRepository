using Microsoft.Data.SqlClient;
using Someren.Models;
using System.Collections.Generic;

namespace Someren.Repositories
{
    public class ParticipationRepository : IParticipationRepository
    {
        private readonly string? _connectionString;

        public ParticipationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("projectdatabase");
        }

        public void AddParticipation(int activityId, int studentId)
        {
            if (studentId <= 0)
            {
                throw new Exception("Invalid student ID provided.");
            }

            using (var conn = new SqlConnection(_connectionString))
            {
                var checkCmd = new SqlCommand("SELECT COUNT(*) FROM STUDENT WHERE student_n = @Student_n", conn);
                checkCmd.Parameters.AddWithValue("@Student_n", studentId);
                conn.Open();
                int count = (int)checkCmd.ExecuteScalar();
                conn.Close();

                if (count == 0)
                {
                    throw new Exception($"Student with student_n {studentId} does not exist.");
                }

                var cmd = new SqlCommand("INSERT INTO PARTICIPANTS (ActivityID, student_n) VALUES (@ActivityID, @Student_n)", conn);
                cmd.Parameters.AddWithValue("@ActivityID", activityId);
                cmd.Parameters.AddWithValue("@Student_n", studentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveParticipation(int activityId, int studentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("DELETE FROM PARTICIPANTS WHERE ActivityID = @ActivityID AND student_n = @Student_n", conn);
                cmd.Parameters.AddWithValue("@ActivityID", activityId);
                cmd.Parameters.AddWithValue("@Student_n", studentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Student> GetStudentsByActivityId(int activityId)
        {
            var students = new List<Student>();

            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(
                    @"SELECT s.Student_n, s.First_name, s.Last_name, s.Phone_n, s.ClassN
                      FROM Student s
                      INNER JOIN PARTICIPANTS p ON s.student_n = p.student_n
                      WHERE p.ActivityID = @ActivityID", conn);

                cmd.Parameters.AddWithValue("@ActivityID", activityId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var student = new Student
                        {
                            Student_n = reader.GetInt32(reader.GetOrdinal("Student_n")),
                            First_name = reader.GetString(reader.GetOrdinal("First_name")),
                            Last_name = reader.GetString(reader.GetOrdinal("Last_name")),
                            Phone_n = reader.GetInt32(reader.GetOrdinal("Phone_n")),
                            ClassN = reader.GetInt32(reader.GetOrdinal("ClassN"))
                        };
                        students.Add(student);
                    }
                }
            }
            return students;
        }
    }
}
