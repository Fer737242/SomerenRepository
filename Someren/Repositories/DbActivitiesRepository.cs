﻿using Microsoft.Data.SqlClient;
using Someren.Models;
using System.Collections.Generic;

namespace Someren.Repositories
{
    public class DbActivitiesRepository : IActivitiesRepository
    {
        private readonly string _connectionString;

        public DbActivitiesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("projectdatabase");
        }

        private Activities ReadActivities(SqlDataReader reader)
        {
            int activityId = (int)reader["activityID"];
            string activityName = (string)reader["activityName"];
            TimeSpan startTime = (TimeSpan)reader["startTime"];
            DateTime startDate = (DateTime)reader["startDate"];

            return new Activities(activityId, activityName, startTime, startDate);
        }

        public List<Activities> GetAll()
        {
            List<Activities> activities = new List<Activities>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT activityID, activityName, startTime, startDate FROM ACTIVITIES";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
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

        public Activities GetByActivityId(int activityId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT activityID, activityName, startTime, startDate FROM ACTIVITIES WHERE activityID = @activityID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@activityID", activityId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return ReadActivities(reader);
                }
                return null;
            }
        }

        public void Add(Activities activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO ACTIVITIES (activityName, startTime, startDate) 
                                 VALUES (@activityName, @startTime, @startDate);
                                 SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@activityName", activity.ActivityName);
                command.Parameters.AddWithValue("@startTime", activity.StartTime);
                command.Parameters.AddWithValue("@startDate", activity.StartDate);

                connection.Open();
                activity.ActivityID = Convert.ToInt32(command.ExecuteScalar());
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

                connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
        }

        public void Delete(Activities activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM ACTIVITIES WHERE activityID = @activityID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@activityID", activity.ActivityID);

                connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records deleted!");
            }
        }
    }
}
