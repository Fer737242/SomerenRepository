using LinqToDB.Tools;
using Microsoft.Data.SqlClient;
using Someren.Models;
using System.Collections.Generic;

namespace Someren.Repositories
{
    public class DbSupervisedBy : ISupervisedBy
    {
        private readonly string? _connectionString;

        public DbSupervisedBy(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("projectdatabase");
        }

        public void AddSupervisor(int activityId, int employeeId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                // Check if the employee_n exists in the LECTURER table
                var checkCmd = new SqlCommand("SELECT COUNT(*) FROM LECTURER WHERE employee_n = @employee_n", conn);
                checkCmd.Parameters.AddWithValue("@employee_n", employeeId);
                conn.Open();
                int count = (int)checkCmd.ExecuteScalar();
                conn.Close();

                if (count == 0)
                {
                    throw new Exception($"Lecturer with employee_n {employeeId} does not exist.");
                }

                // Insert into SUPERVISOR table
                var cmd = new SqlCommand("INSERT INTO SUPERVISOR (ActivityID, employee_n) VALUES (@ActivityID, @employee_n)", conn);
                cmd.Parameters.AddWithValue("@ActivityID", activityId);
                cmd.Parameters.AddWithValue("@employee_n", employeeId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public void DeleteSupervisor(int activityId, int employeeId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("DELETE FROM SUPERVISOR WHERE ActivityID = @ActivityID AND employee_n = @employee_n", conn);
                cmd.Parameters.AddWithValue("@ActivityID", activityId);
                cmd.Parameters.AddWithValue("@employee_n", employeeId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Lecturer> GetSupervisorByActivityId(int activityId)
        {
            var lecturers = new List<Lecturer>();

            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(
                    @"SELECT l.*
                      FROM SUPERVISOR s
                      INNER JOIN LECTURER l ON s.employee_n = l.employee_n
                      WHERE s.ActivityID = @ActivityID", conn);
                cmd.Parameters.AddWithValue("@ActivityID", activityId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var lecturer = new Lecturer
                        {
                            EmployeeN = reader.GetInt32(reader.GetOrdinal("employee_n")),
                            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                            LastName = reader.GetString(reader.GetOrdinal("last_name")),
                            PhoneN = reader.GetString(reader.GetOrdinal("phone_n")),
                            Age = reader.GetString(reader.GetOrdinal("age")),
                        };
                        lecturers.Add(lecturer);
                    }
                }
            }

            return lecturers;
        }
    }
}
