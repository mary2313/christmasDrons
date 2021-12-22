using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DronCities.Assets
{

	public class CityColor
    {
		public byte R;
		public byte G;
		public byte B;

		public CityColor(byte R,byte G,byte B)
        {
			this.R = R;
			this.G = G;
			this.B = B;

        }
    }
	public class City
	{
		public int index { get; private set; }
		public string name { get; private set; }
		public double x { get; private set; }
		public double y { get; private set; }
		public int population { get; private set; }
		public CityColor color;
		public bool Visit = false;
		public City()
		{

		}

		public City(string index, string name, string x, string y, string Population, CityColor color) 
		{
			NumberFormatInfo provider = new NumberFormatInfo();

			

			provider.NumberDecimalSeparator = ".";
			provider.NumberGroupSeparator = ".";
			try
			{
				this.index = Convert.ToInt32(index);
				this.name = name;
				this.x = Convert.ToDouble(x, provider);
				this.y = Convert.ToDouble(y, provider);
				this.population = Convert.ToInt32(Population);
				this.color = color;
			}
			catch (System.FormatException)
			{
				return;
			}
			
		}
	}

	public class Country
	{
		public List<City> Cities = new List<City>();
		public City StartPoint;
		public Country(string Indexes,string Names, string x_s, string y_s, string Populations)
		{
			
			
			string[] SIndexes = Indexes.Split(',');
			string[] SNames = Names.Split(',');
			string[] Sx_s = x_s.Split(',');
			string[] Sy_s = y_s.Split(',');
			string[] SPopulations = Populations.Split(',');

			for(int i = 1; i < SIndexes.Length - 1; i++)
			{
				
				if(SIndexes[i] == "162390")
				{
					StartPoint = new City(SIndexes[i], SNames[i], Sx_s[i], Sy_s[i], SPopulations[i],new CityColor(255,0,0));

					continue;
				}
				if(SIndexes[i] != null && SNames[i] != null && Sx_s[i] != null && Sy_s[i] != null && SPopulations[i] != null )
					Cities.Add(new City(SIndexes[i], SNames[i], Sx_s[i], Sy_s[i], SPopulations[i], new CityColor(0,0,0)));
				
			}
			
		}

		
	}
}
