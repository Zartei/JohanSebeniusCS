using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windowsForms
{
    class PopWorld
    {
		public List<Country> world = new List<Country>();
        public List<int> CC = new List<int>();
        public PopWorld()
        {
            SetCountries();
            SetCity();
        }

        /*
         * Method for fetching all countries in the database.
         */
        private void SetCountries()
        {
            string queryString = $"SELECT * FROM fact.Country;";
            using (SqlConnection connection = new SqlConnection(ConnString.Conn))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        int cc = int.TryParse(reader["id"].ToString(), out int z) ? z : 0;
                        CC.Add(cc);
                        string name = reader["CountryName"].ToString();
                        int GDP = int.TryParse(reader["GDP"].ToString(), out int x) ? x : 0;
                        decimal pop = decimal.TryParse(reader["CountryPop"].ToString(), out decimal y) ? y : 0;
                        Country tempCount = new Country(name, GDP, pop);
                        world.Add(tempCount);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            Country test = world[0];
        }

        private void SetCity()
        {
            foreach(Country C in world)
            {
                String queryString = $"SELECT CityName as C, avgIncome as A, TuristCount as T, pop as P FROm fact.Town where CountryKey = (SELECT id FROM fact.Country where CountryName = '{C.Name}');";
                using(SqlConnection connection = new SqlConnection(ConnString.Conn))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            var test = reader[0].ToString();
                            int avgIn = int.TryParse(reader["A"].ToString(), out int avg) ? avg : 0;
                            int turist = int.TryParse(reader["T"].ToString(), out int TC) ? TC : 0;
                            int pop = int.TryParse(reader["P"].ToString(), out int p) ? p : 0;
                            String cityName = reader["C"].ToString();
                            City temp = new City(cityName, avgIn, turist, pop);
                            C.addCity(temp);
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
}
