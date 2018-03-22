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
/*
select max(price) as max, min(price) as min, AVG(price) as avg,count(*) as amount from amsterdam
select max(price) as max, min(price) as min, AVG(price) as avg,count(*) as amount from boston
select max(price) as max, min(price) as min, AVG(price) as avg,count(*) as amount from barcelona
In city MaxPrice is set to 500
 */
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
            PopWorld world2 = new PopWorld();

            Country netherlands = new Country("Netherlands", 45294, 17m);
            Country usAndA = new Country("USA", 57466, 343m);
            Country spain = new Country("Spain", 26528, 46m);
            City amsterdam = new City("Amsterdam", 10, 10, 1351578);
            City boston = new City("Boston", 12, 12, 687584);
            City barcelona = new City("Barcelona", 13, 13, 1600000);
            netherlands.addCity(amsterdam);
            usAndA.addCity(boston);
            spain.addCity(barcelona); // Första city kan läggas till med hjälp av konstrukturn och sen används add funktion för att lägga ytterligare cities. //KIARASH//
            /*
            bindingSource1.Add(boston);
            bindingSource1.Add(barcelona);
            bindingSource1.Add(amsterdam);
            world.Add(netherlands);
            world.Add(usAndA);
            world.Add(spain);
            */
            world = world2.world; // bra att samla alla Country i en World lista  //KIARASH//

            foreach (Country C in world)
            {
                foreach(City Ci in C.Cities)
                {
                    bindingSource1.Add(Ci);
                    pickedTown = Ci;
                }
            }

            RePlot();
            BoxPlot();
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
                    MessageBox.Show(e.ToString());
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
                ScatPlot(pickedTown);
            }
            else
            {
                HistPlot(pickedTown);
            }
            SetTitle();
        }

        /*
         * Private method for doing the histogram plot
         * Takes a City and updaes the chartPlot with the town data.
         */
        private void HistPlot(City town)
        {
            chartPlot.Series.Clear();
            chartPlot.Series.Add(town.CityName);
            chartPlot.Series[town.CityName].ChartType = SeriesChartType.Column;
            // var filterPrice = town.getAcc().Select(x => x.Price).Where(y => y >)
            int slice = 100;
            var filtered = town.getAcc().Where(x => x.Room_type == "Private room");
            var refactored = filtered.Select(y => new { rest = y.Price % slice, y.Price }).Select(z => new { bucket = z.Price - z.rest });
            var tes = refactored.GroupBy(g => new { g.bucket }).Select(group => new
            {
                Bucket = group.Key.bucket,
                Count = group.Count()
            }).OrderBy(ob => ob.Bucket);
            // .Select(y => new { y.Price - (y.Price % slice) });
            var lookUp = filtered.ToLookup(x => (int)x.Price / slice);
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

            chartPlot.ChartAreas[charAreaName].AxisX.MajorGrid.LineWidth = 0;
            chartPlot.ChartAreas[charAreaName].AxisX.IsMarginVisible = false;
            chartPlot.Series[town.CityName].ChartArea = charAreaName;
            chartPlot.ChartAreas[charAreaName].RecalculateAxesScale();
            chartPlot.ChartAreas[charAreaName].AxisY.Title = "Fequency";
            chartPlot.ChartAreas[charAreaName].AxisX.Title = "Price";
            chartPlot.Legends[0].IsDockedInsideChartArea = true;
            chartPlot.Legends[0].DockedToChartArea = charAreaName;
        }

        /*
         * Private method ScatPlot (Scatter HisPlot)
         * Takes a City and updaes the chartPlot with the town data.
         */
        private void ScatPlot(City town)
        {
            chartPlot.Series.Clear();
            Series Minimum = chartPlot.Series.Add(town.CityName);
            chartPlot.Series[town.CityName].ChartType = SeriesChartType.Point;

            var data = town.getAcc().Select(room => new { room.Price, room.Overall_satisfaction }).Where(sat => sat.Overall_satisfaction < 4.5);

            foreach (var X in data)
            {
                chartPlot.Series[town.CityName].Points.AddXY(X.Price, X.Overall_satisfaction);
            }

            chartPlot.Series[town.CityName].ChartArea = charAreaName;
            chartPlot.ChartAreas[charAreaName].AxisX.Title = "Price";
            chartPlot.ChartAreas[charAreaName].AxisY.Title = "Raiting";
            chartPlot.Legends[0].IsDockedInsideChartArea = true;
            chartPlot.Legends[0].DockedToChartArea = charAreaName;
        }
        
        private void BoxPlot()
        {
            chartPlot.Series.Clear();
            Series Box = chartPlot.Series.Add(pickedTown.CityName);
            Box.ChartType = SeriesChartType.BoxPlot;
            Box["MinPixelPointWidth"] = "15";
            Box["MaxPixelPointWidth"] = "25";
            // chartPlot.Series[pickedTown.CityName].ChartType = SeriesChartType.BoxPlot;
            var data = pickedTown.getAcc().Select(S => new { S.Overall_satisfaction, S.Price }).OrderBy(o => o.Overall_satisfaction) ;
            var grpi = from Point in data
                       group Point by Point.Overall_satisfaction into GO
                       select new
                       {
                           Price = GO
                       };

            DataPoint dp = new DataPoint();
            Series ds = new Series("Boxplot");

            foreach(var point in data)
            {
                chartPlot.Series[pickedTown.CityName].Points.Add(new DataPoint(point.Overall_satisfaction, point.Price)); // AddXY(point.Overall_satisfaction, point.Price);
            }
            chartPlot.Series[pickedTown.CityName].BorderWidth = 3;
            // chartPlot.
            
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

        private void comboBoxSelectCity(object sender, EventArgs e)
        {
            pickedTown = comboBox1.SelectedItem as City;
            RePlot();
        }
    }
} // Fint skriven kod och bra att det finns kommentarer som förklarar vad metoden gör  //KIARASH//
