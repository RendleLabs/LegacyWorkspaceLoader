using System;
using System.Collections.Generic;
using System.Linq;
using Hotel.Data;
using Hotel.Database;

namespace Hotel
{
    public class HotelService : IHotelService
    {
        public IList<Room> GetAvailableRooms(DateTimeOffset checkInDate, DateTimeOffset checkOutDate)
        {
            var data = new RoomData();
            return data.GetAvailableRooms(checkInDate, checkOutDate).ToArray();
        }

        public Room GetRoom(int number)
        {
            var data = new RoomData();
            return data.GetRoom(number);
        }

        public Room[] GetRooms(int[] numbers)
        {
            var data = new RoomData();
            return data.GetRooms(numbers);
        }

        public IEnumerable<Room> AllRooms() => new RoomData().AllRooms();
    }
}
