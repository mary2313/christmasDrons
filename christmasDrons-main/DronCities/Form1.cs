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
				string[] richtextResult = new string[5];
				foreach (var item in result)
				{
					//textResult[i] += count +") "+ item + Environment.NewLine;
					textResult[i] += item + ",";
					richtextResult[i] += item + "\r\n";
					i++;
					if (i == 5)
					{
						i = 0;
						count += 1;
					}
					GoProgressBar(1, 10000);

				}

				Russia = new Country(textResult[0], textResult[1], textResult[2], textResult[3], textResult[4]);
				
				richTextBox2.Text = richtextResult[0];
				richTextBox3.Text = richtextResult[1];
				richTextBox4.Text = richtextResult[2];
				richTextBox5.Text = richtextResult[3];
				richTextBox6.Text = richtextResult[4];

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
            for (int i = 0; i < country.Cities.Count -1; i++)
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
			///
			/// 
			///Cюда вставлять код для проверки прохода всех городов, если все черные, то всё ОК
			///
			/// 
			//////////////////////////////////////////////////////////////////////////////////////////////////////////

			DrawSide(graph, MinDistance);
            for (int i = 0; i < country.Cities.Count -1; i++)
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



            textBox2.Text = $"Слева : {MinDistance.leftSideOfMap.Count} городов, справа : {MinDistance.rightSideOfMap.Count} городов!";
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

		private void DFS(City city, List<double> distance, int j)
        {
			city.Visit = true;
			int countOfVisited = 0;
			for(int i = 0; i < MinDistance.rightSideOfMap.Count; i++)
            {
				if(MinDistance.rightSideOfMap[i].Visit == false && FindMinDistance.FindDistance(city, MinDistance.rightSideOfMap[i]) < 30)
                {
					distance[j] += FindMinDistance.FindDistance(city, MinDistance.rightSideOfMap[i]);
					DFS(MinDistance.rightSideOfMap[i], distance, j);
					
				}
                else
                {
                    for (int ii = 0; ii < MinDistance.rightSideOfMap.Count; ii++)
                    {
                        if (MinDistance.rightSideOfMap[ii].Visit == false)
                        {
                            countOfVisited++;
                        }
                        if (countOfVisited != 0)
                        {
                            MinDistance.rightSideOfMap[i].Visit = false;

							distance.Add(distance[j - 1] - FindMinDistance.FindDistance(city, MinDistance.rightSideOfMap[i]));
                        }

					}
                }
            }
        }
		
		private void button3_Click(object sender, EventArgs e)
		{

            ////bool[] vis = new bool[Russia.Cities.Count];
            //List<double> distance = new List<double>();
            //distance.Add(0);
            //distance.Add(1);
            //int j = 1;
            //DFS(Russia.StartPoint, distance, j);

            /////def DFS(v):
            ////		print(v + 1)

            ////vis[v] = 1

            ////for i in range(len(G)):

            ////	if (G[v][i] == 1 and vis[i] == 0):
            ////           DFS(i)


            ////public Country Russia;
            ////public FindMinDistance MinDistance;







            var twentyClosedCities = FindMinDistance.FindListClosedCities(Russia.StartPoint, MinDistance.leftSideOfMap);

            double countDistance = 0;
            int countOfVis = 0;
            City start = Russia.StartPoint;
            var MinCountOfDistance = double.MaxValue;
            for (int i = 0; i < 30; i++)
            {
                countDistance += FindMinDistance.FindDistance(start, twentyClosedCities[i]);
                start = twentyClosedCities[i];
                while (countOfVis != MinDistance.leftSideOfMap.Count)
                {
                    City closerCity = FindMinDistance.FindAllDistance(start, MinDistance.leftSideOfMap);
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
                if (countDistance < MinCountOfDistance)
                {
                    MinCountOfDistance = countDistance;
                }
            }
            textBox3.Text += "Дистанция первого дрона : " + MinCountOfDistance * 111 + "км";
            var AllCountOfDistance = MinCountOfDistance;

            twentyClosedCities = FindMinDistance.FindListClosedCities(Russia.StartPoint, MinDistance.rightSideOfMap);

            countDistance = 0;
            countOfVis = 0;
            start = Russia.StartPoint;
            MinCountOfDistance = double.MaxValue;
            for (int i = 0; i < 30; i++)
            {
                countDistance += FindMinDistance.FindDistance(start, twentyClosedCities[i]);
                start = twentyClosedCities[i];
                while (countOfVis != MinDistance.rightSideOfMap.Count)
                {
                    City closerCity = FindMinDistance.FindAllDistance(start, MinDistance.rightSideOfMap);
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
                if (countDistance < MinCountOfDistance)
                {
                    MinCountOfDistance = countDistance;
                }
            }
            textBox3.Text += "\r\nДистанция второго дрона : " + MinCountOfDistance * 111 + "км";
            AllCountOfDistance += MinCountOfDistance;
            textBox3.Text += "\r\nДистанция в сумме : " + AllCountOfDistance * 111 + "км";
            textBox3.Visible = true;
            button3.Visible = false;





            //double countDistance = 0;
            //double res = FindMinDistance.FindDistance(Russia.Cities[0], Russia.Cities[1]);
            //int countOfVis = 0;
            //City start = Russia.StartPoint;

            //FindMinSide_ = FindMinDistance.FindUpMinDistance;









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

        private void richTextBox2_TextChanged(object sender, EventArgs e)
		{

		}

		private void richTextBox3_TextChanged(object sender, EventArgs e)
		{

		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{

		}

		private void richTextBox5_TextChanged(object sender, EventArgs e)
		{

		}

		private void richTextBox6_TextChanged(object sender, EventArgs e)
		{

		}

		private void richTextBox4_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
