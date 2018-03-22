using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windowsForms
{
    class City
    {
        private string cityName;
        private int population;
        private int avgIncome;
        private int turistYear;
        private List<Accommodation> rooms = new List<Accommodation>();
        private int countAccom = 0;
        private double avgCost = 0;

        public City(String name, int avg, int turist, int pop)
        {
            CityName = name;
            AvgIncome = avg;
            TuristYear = turist;
            Population = pop;
            getRooms();
            CountAccom = rooms.Count;
            AvgCost = rooms.Average(x => x.Price); 
        }

        public string CityName { get => cityName; set => cityName = value; }
        public int Population { get => population; set => population = value; }
        public int AvgIncome { get => avgIncome; set => avgIncome = value; }
        public int TuristYear { get => turistYear; set => turistYear = value; }
        public int CountAccom { get => countAccom; set => countAccom = value; }
        public double AvgCost { get => avgCost; set => avgCost = value; }

        public List<Accommodation> getAcc()
        {
            return rooms;
        }

        private void addRoom(Accommodation room)
        {
            rooms.Add(room);
        }
        /*
         * This is the main function that speaks with the SQL server.
         * It is the bread n butter of the setup task.
         * 
         */
        public void getRooms()
        {
            string queryString = $"SELECT * FROM {CityName} where price < 500;";
            using (SqlConnection connection = new SqlConnection(ConnString.Conn))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        int room = int.Parse(reader["room_id"].ToString());
                        int host = int.Parse(reader["host_id"].ToString());   // Host
                        string borough = reader["borough"].ToString();
                        string type = reader["room_type"].ToString();               // type
                        string neighbourhood = reader["neighborhood"].ToString();             // neighbourhood
                        int reviews = int.Parse(reader["reviews"].ToString());              // reviews
                        double overall_satisfaction = double.TryParse(reader["overall_satisfaction"].ToString(), out double value) ? value : 0;// double.Parse(reader["overall_satisfaction"].ToString();
                        int accommodates = int.Parse(reader["accommodates"].ToString());    //accomodates
                        int bedrooms = int.TryParse(reader["bedrooms"].ToString(),out int bd) ? bd : 0;
                        int price = int.Parse(reader["price"].ToString());   // price
                        int minstay = int.TryParse(reader["minstay"].ToString(), out int ms) ? ms : 0; // minstay    
                        double latitude = double.Parse(reader["latitude"].ToString());    //latitude  
                        double longitude = double.Parse(reader["longitude"].ToString());   // longitude  
                        string last_modified = reader["last_modified"].ToString();                //last_modifified
                        Accommodation space = new Accommodation(room, host, type, borough, neighbourhood, reviews, overall_satisfaction, accommodates, bedrooms, price, minstay, latitude, longitude, last_modified);

                        addRoom(space);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }
    }
}
