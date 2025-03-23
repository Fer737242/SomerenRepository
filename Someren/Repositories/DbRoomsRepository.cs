using Microsoft.Data.SqlClient;
using Someren.Models;

namespace Someren.Repositories
{
    public class DbRoomsRepository : IRoomsRepository
    {
        private readonly string? _connectionString;

        public DbRoomsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("projectdatabase");
        }

        private Room ReadRoom(SqlDataReader reader)
        {
            string roomNumber = (string)reader["room_number"];
            string type = (string)reader["type"];
            int size = (int)reader["size"];

            return new Room(roomNumber, type, size);
        }

        public List<Room> GetAll()
        {
            List<Room> rooms = new List<Room>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT room_number, type, size FROM ROOM";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Room Room = ReadRoom(reader);
                    rooms.Add(Room);
                }
                reader.Close();
            }
            return rooms;
        }

        public void Add(Room Room)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO ROOM (room_number, type, size) VALUES (@roomNumber, @type, @size)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@roomNumber", Room.RoomNumber);
                command.Parameters.AddWithValue("@type", Room.Type);
                command.Parameters.AddWithValue("@size", Room.Size);
              
                //command.Parameters.AddWithValue("@IsDeleted", user.IsDeleted);

                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(Room room)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE ROOM SET type = @type, size = @size WHERE room_number = @roomNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@roomNumber", room.RoomNumber);
                command.Parameters.AddWithValue("@type", room.Type);
                command.Parameters.AddWithValue("@size", room.Size);

                command.Connection.Open();
                command.ExecuteNonQuery();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
        }

        public Room GetByRoomNumber(string roomNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT room_number, type, size FROM ROOM WHERE room_number = @roomNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@roomNumber", roomNumber);

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

        public void Delete(Room Room)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"DELETE FROM ROOM WHERE room_number = @roomNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@roomNumber", Room.RoomNumber);

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records deleted!");
            }
        }
    }
}

