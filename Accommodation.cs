using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windowsForms
{
    class Accommodation
    {
        private int room_id;
        private int host_id;
        private string room_type;
        private string borough;
        private string neighbourhood;
        private int reviews;
        private double overall_satisfaction;
        private int accomodates;
        private int bedrooms;
        private int price;
        private int minstay;
        private double latitude;
        private double longitude;
        private string last_modifified;

        public Accommodation(int room, int host, string type, string boro, string neigh, int rev, double sat, int acc, int bed, int pri, int min, double lat, double lon, string mod)
        {
            room_id = room;
            host_id = host;
            room_type = type;
            borough = boro;
            neighbourhood = neigh;
            reviews = rev;
            overall_satisfaction = sat;
            accomodates = acc;
            bedrooms = bed;
            price = pri;
            minstay = min;
            latitude = lat;
            longitude = lon;
            last_modifified = mod;
        }

        public int Room_id { get => room_id; set => room_id = value; }
        public int Host_id { get => host_id; set => host_id = value; }
        public string Room_type { get => room_type; set => room_type = value; }
        public string Borough { get => borough; set => borough = value; }
        public string Neighbourhood { get => neighbourhood; set => neighbourhood = value; }
        public int Reviews { get => reviews; set => reviews = value; }
        public double Overall_satisfaction { get => overall_satisfaction; set => overall_satisfaction = value; }
        public int Bedrooms { get => bedrooms; set => bedrooms = value; }
        public int Price { get => price; set => price = value; }
        public int Minstay { get => minstay; set => minstay = value; }
        public double Latitude { get => latitude; set => latitude = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public string Last_modifified { get => last_modifified; set => last_modifified = value; }
        public int Accomodates { get => accomodates; set => accomodates = value; }
    }
}
