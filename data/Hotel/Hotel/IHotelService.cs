using System;
using System.Collections.Generic;
using System.ServiceModel;
using Hotel.Data;

namespace Hotel
{
    [ServiceContract]
    public interface IHotelService
    {
        [OperationContract]
        IList<Room> GetAvailableRooms(DateTimeOffset checkInDate, DateTimeOffset checkOutDate);

        [OperationContract]
        Room GetRoom(int number);

        [OperationContract]
        Room[] GetRooms(int[] numbers);

        [OperationContract]
        IEnumerable<Room> AllRooms();
    }
}
