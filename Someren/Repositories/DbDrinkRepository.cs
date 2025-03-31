using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Someren.Models;

namespace Someren.Repositories
{
    public class DbDrinkRepository : IDrinkRepository
    {
        private readonly string? _connectionString;

        public DbDrinkRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("projectdatabase");
        }

        public List<Drink> GetAll()
        {
            List<Drink> drinks = new List<Drink>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT type, Drink_Name, alcoholic, price, voucher FROM DRINK ORDER BY type ASC";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Drink drink = new Drink(
                        Convert.ToInt32(reader["type"]),
                        reader["Drink_Name"].ToString(),
                        reader["alcoholic"].ToString(),
                        Convert.ToDecimal(reader["price"]),
                        Convert.ToInt32(reader["voucher"])
                    );
                    drinks.Add(drink);
                }
                reader.Close();
            }

            return drinks;
        }

        public Drink GetBytype(int type)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT type, Drink_Name, alcoholic, price, voucher FROM DRINK WHERE type = @type";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@type", Convert.ToInt32(type)); // Ensure this matches the parameter type

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Drink(
                        Convert.ToInt32(reader["type"]),
                        reader["Drink_Name"].ToString(),
                        reader["alcoholic"].ToString(),
                        Convert.ToDecimal(reader["price"]),
                        Convert.ToInt32(reader["voucher"])
                    );
                }
                else
                {
                    return null; // Return null if no drink is found with the specified type
                }
            }
        }

    }
}
