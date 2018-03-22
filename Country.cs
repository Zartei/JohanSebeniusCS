using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windowsForms
{
    class Country
    {
        private string name;
        private decimal population;
        private int gdp;
        private List<City> cities = new List<City>();

        public Country(String Name, int Cap, decimal pop)
        {
            this.Name = Name;
            Gdp = Cap;
            Population = pop;
        }// med denna Konstruction kan man skapa Country objekt utan city. //KIARASH//


        public decimal Population { get => population; set => population = value; }
        public string Name { get => name; set => name = value; }
        public int Gdp { get => gdp; set => gdp = value; }
        public List<City> Cities { get => cities; }

        public List<City> getTowns()
        {
            return cities;
        }
        public void addCity(City city)
        {
            cities.Add(city);
        }// behöver man skapa en method när man kan väl använda .add() direkt? //KIARASH//
    }
}
