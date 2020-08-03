using System;
using System.Collections.Generic;
using System.Linq;
using Hotel.Data;

namespace Hotel.Database
{
    public interface IRoomData
    {
        IEnumerable<Room> GetAvailableRooms(DateTimeOffset checkInDate, DateTimeOffset checkOutDate);
        Room GetRoom(int number);
        Room[] GetRooms(int[] numbers);
        IEnumerable<Room> AllRooms();
    }

    public class RoomData : IRoomData
    {
        private static readonly Room[] Rooms = {

            new Room
            {
                Number = 101,
                Floor = 1,
                Price = 50m
            },

            new Room
            {
                Number = 237,
                Floor = 2,
                Price = 10m
            },

            new Room
            {
                Number = 1408,
                Floor = 14,
                Price = 1m
            },
        };

        public IEnumerable<Room> AllRooms() => Rooms;

        public IEnumerable<Room> GetAvailableRooms(DateTimeOffset checkInDate, DateTimeOffset checkOutDate)
        {
            return Rooms.AsEnumerable();
        }

        public Room GetRoom(int number)
        {
            return Rooms.FirstOrDefault(r => r.Number == number);
        }

        public Room[] GetRooms(int[] numbers)
        {
            return Rooms.Where(r => numbers.Contains(r.Number)).ToArray();
        }
    }
}
