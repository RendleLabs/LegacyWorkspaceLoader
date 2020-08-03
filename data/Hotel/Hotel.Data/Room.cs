using System.Runtime.Serialization;

namespace Hotel.Data
{
    [DataContract]
    public class Room
    {
        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public int Floor { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal Price { get; set; }
    }
}
