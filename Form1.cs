using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace windowsForms
{
    public partial class Form1 : Form
    {
        
        private List<Country> world = new List<Country>();
        private Boolean plotSet = false;
        private City pickedTown;
        private string charAreaName = "ChartArea1";
        public Form1()
        {
            InitializeComponent();
            GetConString();
            Country netherlands = new Country("Netherlands", 45294, 17m);
            Country usAndA = new Country("USA", 57466, 343m);
            Country spain = new Country("Spain", 26528, 46m);
            City amsterdam = new City("Amsterdam", 10, 10, 1351578);
            City boston = new City("Boston", 12, 12, 687584);
            City barcelona = new City("Barcelona", 13, 13, 1600000);
            netherlands.addCity(amsterdam);
            usAndA.addCity(boston);
            spain.addCity(barcelona);
            world.Add(netherlands);
            world.Add(usAndA);
            world.Add(spain);
            pickedTown = boston;
            RePlot();
            
        }

        private void GetConString()
        {
            string myPth = Directory.GetCurrentDirectory();
            string FixedPath = Path.GetFullPath(Path.Combine(myPth, @"..\..\..\string.txt"));
            if (File.Exists(FixedPath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(FixedPath))
                    {
                        string line = sr.ReadLine();
                        ConnString.Conn = line;
                    }
                }
                catch (Exception e)
                {

                }
            }
            else
            {
                throw new System.IO.FileNotFoundException("Config File is missing");
            }
            
            
        }

        public void SetTitle()
        {
            this.Text = pickedTown.CityName;
        }

        // Function for quit label
        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        /*
         * Method for seting what type of plot to have.
         */
        private void TogglePlot(object sender, EventArgs e)
        {
            plotSet = (sender as Button).Text == "Scatter" ? true : false;

            RePlot();
        }
        /*
         * Method plot for calling the selected chart type plotter.
         */
        private void RePlot()
        {
            if (plotSet)
            {
                HisPlot(pickedTown);
            }
            else
            {
                ScatPlot(pickedTown);
            }
            SetTitle();
        }

        /*
         * Private method ScatPlot (Scatter HisPlot)
         * Takes a City and updaes the chartPlot with the town data.
         */
        private void ScatPlot(City town)
        {
            chartPlot.Series.Clear();
            chartPlot.Series.Add(town.CityName);
            chartPlot.Name = "Test";
            chartPlot.Series[town.CityName].ChartType = SeriesChartType.Column;
            // var filterPrice = town.getAcc().Select(x => x.Price).Where(y => y >)
            int slice = 100;
            var lookUp = town.getAcc().ToLookup(x => (int)x.Price / slice);
            var smallest = lookUp.Min(x => x.Key);
            var largest = lookUp.Max(x => x.Key);
            var Bucketed = from X in Enumerable.Range(smallest, largest - smallest + 1)
                           select new
                           {
                               Range = String.Format("{0}-{1}", X * slice, (X + 1) * slice),
                               Count = lookUp[X].Count(),
                           };
            foreach(var X in Bucketed)
            {
                chartPlot.Series[town.CityName].Points.AddXY(X.Range, X.Count);
            }
            /*
            var groupedData = town.getAcc().GroupBy(x => new { x.Price }).Select(group => new
            {
                Bucket = group.Key.Price,
                Count = group.Count(c => c.Price > 0)
            }).OrderBy(ob => ob.Bucket).Where(i => i.Count > 2);
           // var Buckets = town.getAcc().GroupBy(y => ranges.)
            foreach (var X in groupedData)
            {
                chartPlot.Series[town.CityName].Points.AddXY(X.Count, X.Bucket);
            }
            */
            chartPlot.ChartAreas[charAreaName].AxisX.IsMarginVisible = false;
            chartPlot.Series[town.CityName].ChartArea = charAreaName;
            chartPlot.ChartAreas[charAreaName].RecalculateAxesScale();
            chartPlot.ChartAreas[charAreaName].AxisY.Title = "Fequency";
            chartPlot.ChartAreas[charAreaName].AxisX.Title = "Price";
            chartPlot.Series[town.CityName].IsVisibleInLegend = false;
        }
        /*
         * Private method for doing the histogram plot
         * Takes a City and updaes the chartPlot with the town data.
         */
        private void HisPlot(City town)
        {
            chartPlot.Series.Clear();
            Series Minimum = chartPlot.Series.Add(town.CityName);
            chartPlot.Series[town.CityName].ChartType = SeriesChartType.Point;

            var data = town.getAcc().Select(room => new { room.Price, room.Overall_satisfaction }).Where(sat => sat.Overall_satisfaction > 3);

            foreach (var X in data)
            {
                chartPlot.Series[town.CityName].Points.AddXY(X.Price, X.Overall_satisfaction);
            }

            chartPlot.Series[town.CityName].ChartArea = charAreaName;
            chartPlot.ChartAreas[charAreaName].AxisX.Title = "Price";
            chartPlot.ChartAreas[charAreaName].AxisY.Title = "Raiting";
            chartPlot.Series[town.CityName].IsVisibleInLegend = false;
        }
        /*
         * Private method to update the current City.
         * Reads the value of a button and trys to find it in the world list.
         * If it is missing nothing happens.
         */
        private void CityPicker(object sender, EventArgs e)
        {
            var lander = world.Select(x => new { Country = x.Cities });

            foreach (Country count in world)
            {
                var towns = count.Cities;
                foreach (City t in towns)
                {
                    if (t.CityName == (sender as Button).Text)
                    {
                        pickedTown = t;
                        RePlot();
                    }
                }
            }
        }
    }
}
