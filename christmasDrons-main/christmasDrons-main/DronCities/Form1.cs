using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using DronCities.Assets;
using System.Runtime.InteropServices;
using System.Threading;

namespace DronCities
{

	public partial class Form1 : Form
	{
		
		private OpenFileDialog ofd;
		public Country Russia;
		public FindMinDistance MinDistance;

		public Form1()
		{
			
			InitializeComponent();
			//WindowState = FormWindowState.Maximized;
			//TopMost = true;
			
		}

		private void button1_Click(object sender, EventArgs e)
		{

			button3.Visible = false;
			ofd = new OpenFileDialog();
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				
				var parser = new TextFieldParser(ofd.FileName,Encoding.Default);
				var result = Parse.ParseCVSFile(parser);
				string RES = "";
				//result.Encoding
				//byte[] bytes = Encoding.Default.GetBytes(result);
				//result = Encoding.UTF8.GetString(bytes);
				
				int i = 0;
				int count = 1;
				string[] textResult = new string[5];
				foreach(var item in result)
				{
					//textResult[i] += count +") "+ item + Environment.NewLine;
					textResult[i] += item + ",";
					i++;
					if (i == 5)
					{
						i = 0;
						count += 1;
					}
					GoProgressBar(1, 10000);

				}
				
				richTextBox2.Text += textResult[0];
				richTextBox3.Text += textResult[1];
				richTextBox4.Text += textResult[2];
				richTextBox5.Text += textResult[3];
				richTextBox6.Text += textResult[4];

				var Test = new Country(textResult[0], textResult[1], textResult[2], textResult[3], textResult[4]);

				button1.Visible = false;
				button2.Visible = true;

				
			}
			
		}

		void GoProgressBar(int arg, int arg_lengt)
		{
			int a = (arg_lengt / 100) * arg;
			Convert.ToInt32(a);
			if(progressBar1.Value + a > 100) progressBar1.Value = 100 - a;
			progressBar1.Value += a;
		}

		
		private void button2_Click(object sender, EventArgs e)
		{
			HideAllStartForms();
			pictureBox1.Visible = true;
			button3.Visible = true;
			Russia = new Country(richTextBox2.Text, richTextBox3.Text, richTextBox4.Text, richTextBox5.Text, richTextBox6.Text);
			button3.Visible = true;
			Draw(Russia);
			button3.Visible = true;
			//Thread.Sleep(3000);
			button3.Visible = true;

		}

		private void Draw(Country country)
		{
			//Graphics graph = pictureBox1.CreateGraphics();
			Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
			Graphics graph = Graphics.FromImage(bmp);
			List<Rectangle> rect = new List<Rectangle>();
            //Rectangle rect = ;
            for (int i = 0; i < country.Cities.Count; i++)
            {
                rect.Add(new Rectangle(Convert.ToInt32(Math.Round(country.Cities[i].y * 10)) - 150, Convert.ToInt32(Math.Round(country.Cities[i].x * 10)) - 150, 2, 2));
            }


            graph.DrawRectangle(Pens.Red, new Rectangle(Convert.ToInt32(Math.Round(country.StartPoint.y * 10)) - 150, Convert.ToInt32(Math.Round(country.StartPoint.x * 10)) - 150, 2, 2));
            graph.FillRectangle(
                new SolidBrush(Color.FromArgb(255, country.StartPoint.color.R, country.StartPoint.color.G, country.StartPoint.color.B)),
                new Rectangle(Convert.ToInt32(Math.Round(country.StartPoint.y * 10)) - 150, Convert.ToInt32(Math.Round(country.StartPoint.x * 10)) - 150, 2, 2)
            );


            MinDistance = new FindMinDistance(country);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////

            double Shit = FindMinDistance.FindDistance(country.Cities[2], country.Cities[3]);

            double countDistance = 0;
            //double res = FindMinDistance.FindDistance(Russia.Cities[0], Russia.Cities[1]);
            int countOfVis = 0;
            City start = Russia.StartPoint;

            double FUCKINBIGDISTANCE = 0;

            while (countOfVis != MinDistance.LeftUp.Count)
            {
                City closerCity = FindMinDistance.FindAllDistance(start, MinDistance.LeftUp);
                double distance = FindMinDistance.FindDistance(start, closerCity);
                countDistance += distance;
                if (FUCKINBIGDISTANCE < distance) FUCKINBIGDISTANCE = distance;

                start = closerCity;
                start.Visit = true;
                start.color.R = 0;
                start.color.G = 0;
                start.color.B = 0;
                countOfVis++;
            }


            City longerCity = FindMinDistance.FindAllDistanceMAX(Russia.StartPoint, MinDistance.LeftDown);
            longerCity.color.R = 0;
            longerCity.color.G = 0;
            longerCity.color.B = 0;
            longerCity.Visit = true;
            countDistance += FindMinDistance.FindDistance(start, longerCity);
            start = longerCity;

            countOfVis = 0;
            while (countOfVis != MinDistance.LeftDown.Count - 1)
            {

                City closerCity = FindMinDistance.FindAllDistance(start, MinDistance.LeftDown);
                double distance = FindMinDistance.FindDistance(start, closerCity);
                countDistance += distance;

                start = closerCity;
                start.Visit = true;
                start.color.R = 0;
                start.color.G = 0;
                start.color.B = 0;
                countOfVis++;
            }
            countDistance += FindMinDistance.FindDistance(start, Russia.StartPoint);

            double FirstDronDistance = countDistance * 82;




            countDistance = 0;
            //double res = FindMinDistance.FindDistance(Russia.Cities[0], Russia.Cities[1]);
            countOfVis = 0;
            start = Russia.StartPoint;

            while (countOfVis != MinDistance.RightUp.Count)
            {
                City closerCity = FindMinDistance.FindAllDistance(start, MinDistance.RightUp);
                double distance = FindMinDistance.FindDistance(start, closerCity);
                countDistance += distance;

                start = closerCity;
                start.Visit = true;
                start.color.R = 0;
                start.color.G = 0;
                start.color.B = 0;
                countOfVis++;
            }


            longerCity = FindMinDistance.FindAllDistanceMAX(Russia.StartPoint, MinDistance.RightDown);
            longerCity.color.R = 0;
            longerCity.color.G = 0;
            longerCity.color.B = 0;
            longerCity.Visit = true;
            countDistance += FindMinDistance.FindDistance(start, longerCity);
            start = longerCity;

            countOfVis = 0;
            while (countOfVis != MinDistance.RightDown.Count - 1)
            {

                City closerCity = FindMinDistance.FindAllDistance(start, MinDistance.RightDown);
                double distance = FindMinDistance.FindDistance(start, closerCity);
                countDistance += distance;

                start = closerCity;
                start.Visit = true;
                start.color.R = 0;
                start.color.G = 0;
                start.color.B = 0;
                countOfVis++;
            }
            countDistance += FindMinDistance.FindDistance(start, Russia.StartPoint);

            double SecondDronDistance = countDistance * 82;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////





            DrawSide(graph, MinDistance);
            for (int i = 0; i < country.Cities.Count; i++)
            {
                try
                {
                    graph.DrawRectangle(new Pen(Color.FromArgb(255, 192, 255, 255)), rect[i]);
                    graph.FillRectangle(new SolidBrush(Color.FromArgb(255, country.Cities[i].color.R, country.Cities[i].color.G, country.Cities[i].color.B)), rect[i]);

                }
                catch (NullReferenceException)
                {
                    continue;
                }
            }

            pictureBox1.Image = bmp;



            textBox2.Text = $"Слева : {MinDistance.LeftDown.Count} городов, справа : {MinDistance.RightDown.Count} городов!";
			textBox2.Visible = true;
			
		}

		private void DrawSide(Graphics graph, FindMinDistance MinDistance)
		{
			//int AllCities = MinDistance.LeftDown.Count + MinDistance.RightDown.Count + MinDistance.LeftUp.Count + MinDistance.RightUp.Count - 1;
			List<Rectangle> rectLeftDown = new List<Rectangle>();
			List<Rectangle> rectLeftUp = new List<Rectangle>();
			List<Rectangle> rectRightDown = new List<Rectangle>();
			List<Rectangle> rectRightUp = new List<Rectangle>();
			///////////////////////////////////////////////////////
			for (int i = 0; i < MinDistance.LeftDown.Count; i++)
			{
				rectLeftDown.Add(new Rectangle(Convert.ToInt32(Math.Round(MinDistance.LeftDown[i].y * 10)) - 150, Convert.ToInt32(Math.Round(MinDistance.LeftDown[i].x * 10)) - 50, 2, 2));
			}
			for (int i = 0; i < MinDistance.LeftUp.Count; i++)
			{
				rectLeftUp.Add(new Rectangle(Convert.ToInt32(Math.Round(MinDistance.LeftUp[i].y * 10)) - 150, Convert.ToInt32(Math.Round(MinDistance.LeftUp[i].x * 10)) - 50, 2, 2));
			}
			for (int i = 0; i < MinDistance.RightDown.Count; i++)
			{
				rectRightDown.Add(new Rectangle(Convert.ToInt32(Math.Round(MinDistance.RightDown[i].y * 10)) - 150, Convert.ToInt32(Math.Round(MinDistance.RightDown[i].x * 10)) - 50, 2, 2));
			}
			for (int i = 0; i < MinDistance.RightUp.Count; i++)
			{
				rectRightUp.Add(new Rectangle(Convert.ToInt32(Math.Round(MinDistance.RightUp[i].y * 10)) - 150, Convert.ToInt32(Math.Round(MinDistance.RightUp[i].x * 10)) - 50, 2, 2));
			}
			////////////////////////////////////////////////////////
			
			
			
		}

		public delegate City FindMinSide_(City city, List<City> Side);
		private void button3_Click(object sender, EventArgs e)
		{


            







            //double countDistance = 0;
            //double res = FindMinDistance.FindDistance(Russia.Cities[0], Russia.Cities[1]);
            //int countOfVis = 0;
            //City start = Russia.StartPoint;

            ////FindMinSide_ = FindMinDistance.FindUpMinDistance;









            //         double countDistance = 0;
            //         //double res = FindMinDistance.FindDistance(Russia.Cities[0], Russia.Cities[1]);
            //         int countOfVis = 0;
            //         City start = Russia.StartPoint;

            //         double FUCKINBIGDISTANCE = 0;

            //         while (countOfVis != MinDistance.LeftDown.Count)
            //         {
            //             City closerCity = FindMinDistance.FindAllDistance(start, MinDistance.LeftDown);
            //             double distance = FindMinDistance.FindDistance(start, closerCity);
            //             countDistance += distance;
            //             if (FUCKINBIGDISTANCE < distance) FUCKINBIGDISTANCE = distance;

            //             start = closerCity;
            //             start.Visit = true;
            //             start.color.R = 0;
            //             start.color.G = 0;
            //             start.color.B = 0;
            //             countOfVis++;
            //         }


            //         City longerCity = FindMinDistance.FindAllDistanceMAX(Russia.StartPoint, MinDistance.LeftUp);
            //         longerCity.color.R = 0;
            //         longerCity.color.G = 0;
            //         longerCity.color.B = 0;
            //         longerCity.Visit = true;
            //         countDistance += FindMinDistance.FindDistance(start, longerCity);
            //         start = longerCity;

            //         countOfVis = 0;
            //         while (countOfVis != MinDistance.LeftUp.Count - 1)
            //         {

            //             City closerCity = FindMinDistance.FindAllDistance(start, MinDistance.LeftUp);
            //             double distance = FindMinDistance.FindDistance(start, closerCity);
            //             countDistance += distance;

            //             start = closerCity;
            //             start.Visit = true;
            //             start.color.R = 0;
            //             start.color.G = 0;
            //             start.color.B = 0;
            //             countOfVis++;
            //         }
            //         countDistance += FindMinDistance.FindDistance(start, Russia.StartPoint);

            //         double FirstDronDistance = countDistance * 111;




            //         countDistance = 0;
            //         //double res = FindMinDistance.FindDistance(Russia.Cities[0], Russia.Cities[1]);
            //         countOfVis = 0;
            //         start = Russia.StartPoint;

            //         while (countOfVis != MinDistance.RightDown.Count)
            //         {
            //             City closerCity = FindMinDistance.FindAllDistance(start, MinDistance.RightDown);
            //             double distance = FindMinDistance.FindDistance(start, closerCity);
            //             countDistance += distance;

            //             start = closerCity;
            //             start.Visit = true;
            //             start.color.R = 0;
            //             start.color.G = 0;
            //             start.color.B = 0;
            //             countOfVis++;
            //         }


            //         longerCity = FindMinDistance.FindAllDistanceMAX(Russia.StartPoint, MinDistance.RightUp);
            //         longerCity.color.R = 0;
            //         longerCity.color.G = 0;
            //         longerCity.color.B = 0;
            //         longerCity.Visit = true;
            //         countDistance += FindMinDistance.FindDistance(start, longerCity);
            //         start = longerCity;

            //         countOfVis = 0;
            //         while (countOfVis != MinDistance.RightUp.Count - 1)
            //         {

            //             City closerCity = FindMinDistance.FindAllDistance(start, MinDistance.RightUp);
            //             double distance = FindMinDistance.FindDistance(start, closerCity);
            //             countDistance += distance;

            //             start = closerCity;
            //             start.Visit = true;
            //             start.color.R = 0;
            //             start.color.G = 0;
            //             start.color.B = 0;
            //             countOfVis++;
            //         }
            //         countDistance += FindMinDistance.FindDistance(start, Russia.StartPoint);

            //         double SecondDronDistance = countDistance * 111;

        }
    }
}
