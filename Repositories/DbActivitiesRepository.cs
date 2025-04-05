using Microsoft.Data.SqlClient;
using Someren.Models;
using System.Diagnostics;

namespace Someren.Repositories
{
    public class DbActivitiesRepository : IActivitiesRepository
    {
        private readonly string? _connectionString;

        public DbActivitiesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("projectdatabase");
        }

        private Activities ReadActivities(SqlDataReader reader)
        {
            int activityId = (int)reader["ActivityID"];
            string activityName = (string)reader["ActivityName"];

            TimeSpan startTime = (TimeSpan)reader["StartTime"];
            DateTime startDate = (DateTime)reader["StartDate"];

            return new Activities(activityId, activityName, startTime, startDate);
        }


        public List<Activities> GetAll()
        {
            List<Activities> activities = new List<Activities>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT ActivityID, ActivityName, StartTime, StartDate FROM ACTIVITIES";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Activities activity = ReadActivities(reader);
                    activities.Add(activity);
                }
                reader.Close();
            }
            return activities;
        }

        public void Add(Activities activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO ACTIVITIES (activityName, startTime, startDate) 
                         VALUES (@activityName, @startTime, @startDate);
                         SELECT SCOPE_IDENTITY();"; // Haalt het ID van de nieuw toegevoegde record op

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@activityName", activity.ActivityName);
                command.Parameters.AddWithValue("@startTime", activity.StartTime);
                command.Parameters.AddWithValue("@startDate", activity.StartDate);

                command.Connection.Open();
                activity.ActivityID = Convert.ToInt32(command.ExecuteScalar()); // Haal het gegenereerde ID correct op
            }
        }




        public void Update(Activities activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE ACTIVITIES SET activityName = @activityName, startTime = @startTime, startDate = @startDate WHERE activityID = @activityID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@activityID", activity.ActivityID);
                command.Parameters.AddWithValue("@activityName", activity.ActivityName);
                command.Parameters.AddWithValue("@startTime", activity.StartTime);
                command.Parameters.AddWithValue("@startDate", activity.StartDate);

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
        }

        public Activities GetByActivityId(int activityId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT activityID, activityName, startTime, startDate FROM ACTIVITIES WHERE activityID = @activityID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@activityID", activityId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadActivities(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        public void Delete(Activities activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM ACTIVITIES WHERE activityID = @activityID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@activityID", activity.ActivityID);

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records deleted!");
            }
        }
    }
}