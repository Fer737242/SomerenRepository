namespace Someren.Models
{
    public class Room
    {
            public string RoomNumber { get; set; }
            public int Size { get; set; }
            public string Type { get; set; } // "Student" or "Lecturer"

        public Room() { }

        public Room(string roomNumber, string type, int size)
        {
            RoomNumber = roomNumber;
            Size = size;
            Type = type;
        }

    }
}
