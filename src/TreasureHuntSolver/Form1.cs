﻿using System.Security.Cryptography;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using BFS;
using DFS;
using System.Reflection.Emit;
using System.Threading;
using System.IO;

namespace WinForm
{
    public partial class Form1 : Form
    {
        // attribute + initialization
        private int row = 0;
        private int col = 0;
        private static char[,] map = new char[0, 0];
        private int[] start = new int[2];
        private string path = "";
        private static int[,] mapInt = new int[0, 0];
        private static List<Tuple<int, int>> goalStates = new List<Tuple<int, int>>();
        private double duration = 0;
        private static PictureBox[,] imageMatrix = new PictureBox[0, 0];

        public Form1()
        {
            InitializeComponent();
        }

        private System.Windows.Forms.OpenFileDialog openFile1 = new OpenFileDialog();

        private void openFileBtn_Click(object sender, EventArgs e)
        {
            this.openFile1.InitialDirectory = @"C:\";
            this.openFile1.Title = "Search File";

            this.openFile1.CheckFileExists = true;
            this.openFile1.CheckPathExists = true;

            this.openFile1.DefaultExt = "txt";
            this.openFile1.Filter = "txt files (*.txt)|*.txt";
            this.openFile1.FilterIndex = 2;
            this.openFile1.RestoreDirectory = true;

            this.openFile1.ReadOnlyChecked = true;
            this.openFile1.ShowReadOnly = true;

            if (this.openFile1.ShowDialog() == DialogResult.OK)
            {
                fileNamePlace.Text = openFile1.FileName;
            }
        }

        private void searchButtton_Click(object sender, EventArgs e)
        {
            path = "";
            if (DFSbtn.Checked)
            {
                Console.WriteLine("search with DFS");

                //if (TSPcheckBox.Checked)
                //{
                //    Console.WriteLine("TSP");
                //    goalStates.Add(new Tuple<int, int>(start[0], start[1]));
                //    goalStates.Add(new Tuple<int, int>(start[0], start[1]));
                //}

                Stopwatch stopwatch = new Stopwatch();
                DFSMazeSolver solver = new DFSMazeSolver(mapInt, goalStates, start);

                stopwatch.Start();
                solver.Solve(start[0], start[1], path, Decimal.ToInt32(sleepInputBox.Value));
                stopwatch.Stop();

                string routetext = "";

                if (solver.path == "")
                {
                    routetext = "Route not found";
                }
                else
                {
                    path = solver.path;
                    for (int i = 0; i < path.Length; i++)
                    {
                        routetext += path[i];
                        if (i != path.Length - 1)
                        {
                            routetext += " - ";
                        }
                    }
                }

                if (routetext.Length > 73)
                {
                    Font newSizeFont = new Font(routeText.Font.FontFamily, 9, routeText.Font.Style);
                    routeText.Font = newSizeFont;
                    routeText.Location = new Point(695, 603);
                }
                else
                {
                    Font SizeFont = new Font(routeText.Font.FontFamily, 18, routeText.Font.Style);
                    routeText.Font = SizeFont;
                    routeText.Location = new Point(695, 595);
                }

                routeText.Text = routetext;
                stepsText.Text = path.Length.ToString();
                nodesText.Text = solver.nodeCount.ToString();
                routeText.Refresh();

                TimeSpan elapsed = stopwatch.Elapsed;
                this.duration = elapsed.TotalMilliseconds / 1000;
                timeText.Text = this.duration.ToString();
                timeText.Text += " s";
            }

            if (dfsMultivisitbtn.Checked)
            {
                Console.WriteLine("search with DFS Multivisit");

                Stopwatch stopwatch = new Stopwatch();
                DFSMazeSolver solver = new DFSMazeSolver(mapInt, goalStates, start);

                stopwatch.Start();
                solver.SolveMultivisit(start[0], start[1], path, Decimal.ToInt32(sleepInputBox.Value));
                stopwatch.Stop();

                string routetext = "";
                path = solver.path;

                for (int i = 0; i < path.Length; i++)
                {
                    routetext += path[i];
                    if (i != path.Length - 1)
                    {
                        routetext += " - ";
                    }
                }


                if (routetext.Length > 73)
                {
                    Font newSizeFont = new Font(routeText.Font.FontFamily, 9, routeText.Font.Style);
                    routeText.Font = newSizeFont;
                    routeText.Location = new Point(695, 603);
                }
                else
                {
                    Font SizeFont = new Font(routeText.Font.FontFamily, 18, routeText.Font.Style);
                    routeText.Font = SizeFont;
                    routeText.Location = new Point(695, 595);
                }

                routeText.Text = routetext;
                stepsText.Text = path.Length.ToString();
                nodesText.Text = solver.nodeCount.ToString();
                routeText.Refresh();

                TimeSpan elapsed = stopwatch.Elapsed;
                this.duration = elapsed.TotalMilliseconds / 1000;
                timeText.Text = this.duration.ToString();
                timeText.Text += " s";
            }

            if (BFSbtn.Checked == true)
            {
                Console.WriteLine("search with BFS");

                Stopwatch stopwatch = new Stopwatch();
                List<Tuple<int, int>> simpulHidup = new List<Tuple<int, int>>();
                simpulHidup.Add(new Tuple<int, int>(start[0], start[1]));
                BFSMazeSolver solver = new BFSMazeSolver(mapInt, goalStates);

                
                if (TSPcheckBox.Checked == true)
                {
                    string[] treasurePath = new string[goalStates.Count+1];
                    stopwatch.Start();
                    solver.Solve(simpulHidup[0], ref treasurePath, ref simpulHidup, true, Decimal.ToInt32(sleepInputBox.Value));
                    stopwatch.Stop();
                }
                else
                {
                    string[] treasurePath = new string[goalStates.Count];
                    stopwatch.Start();
                    solver.Solve(simpulHidup[0], ref treasurePath, ref simpulHidup, false, Decimal.ToInt32(sleepInputBox.Value));
                    stopwatch.Stop();
                }
                path = solver.path;
                
                string routetext = "";
                for (int i = 0; i < path.Length; i++)
                {
                    routetext += path[i];
                    if (i != path.Length - 1)
                    {
                        routetext += " - ";
                    }
                }

                if (routetext.Length > 73)
                {
                    Font newSizeFont = new Font(routeText.Font.FontFamily, 9, routeText.Font.Style);
                    routeText.Font = newSizeFont;
                    routeText.Location = new Point(695, 603);
                }
                else
                {
                    Font SizeFont = new Font(routeText.Font.FontFamily, 18, routeText.Font.Style);
                    routeText.Font = SizeFont;
                    routeText.Location = new Point(695, 595);
                }

                routeText.Text = routetext;
                stepsText.Text = path.Length.ToString();
                nodesText.Text = solver.nodeCount.ToString();
                routeText.Refresh();


                TimeSpan elapsed = stopwatch.Elapsed;
                timeText.Text = (elapsed.TotalMilliseconds / 1000).ToString();
                timeText.Text += " s";

            }
        }


        public static void outputRoute(int i, int j, int visitCount, int sleepTime)
        {
            Console.WriteLine("update gui color");
            Application.DoEvents();
            if (visitCount != 0)
            {
                if (map[i, j] == 'R')
                {
                    imageMatrix[i, j].BackColor = Color.FromArgb(0, (255 - visitCount * 20), 0);
                }
                else if (map[i, j] == 'T')
                {
                    imageMatrix[i, j].BackColor = Color.FromArgb(0, (255 - visitCount * 20), (255 - visitCount * 20));
                }
                else if (map[i, j] == 'K')
                {
                    imageMatrix[i, j].BackColor = Color.FromArgb((255 - visitCount * 20), 0, (255 - visitCount * 20));
                }
            }
            else
            {
                if (map[i, j] == 'R')
                {
                    imageMatrix[i, j].BackColor = Color.White;
                }
                else if (map[i, j] == 'T')
                {
                    imageMatrix[i, j].BackColor = Color.BlueViolet;
                }
            }
            Thread.Sleep(sleepTime);
        }

        private System.Windows.Forms.Label errorText = new System.Windows.Forms.Label();

        private void visualBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Map();
            }
            catch (Exception err)
            {
                Console.WriteLine("input tidak valid");
                Console.WriteLine($"Error : Invalid Data Input");
                errorText.Text = "INPUT FILE INVALID!";
                Font newFont = new Font(errorText.Font.FontFamily, 50, errorText.Font.Style);
                errorText.Font = newFont;
                errorText.Location = new Point(638, 300);
                errorText.Size = new Size(700, 80);
                errorText.BackColor = Color.Black;
                errorText.ForeColor = Color.Red;
                errorText.Anchor = AnchorStyles.Top;
                errorText.Anchor = AnchorStyles.Bottom;
                errorText.BringToFront();
                Controls.Add(errorText);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            this.openFile1.InitialDirectory = @"C:\";
            this.openFile1.Title = "Search File";

            this.openFile1.CheckFileExists = true;
            this.openFile1.CheckPathExists = true;

            this.openFile1.DefaultExt = "txt";
            this.openFile1.Filter = "txt files (*.txt)|*.txt";
            this.openFile1.FilterIndex = 2;
            this.openFile1.RestoreDirectory = true;

            this.openFile1.ReadOnlyChecked = true;
            this.openFile1.ShowReadOnly = true;
        }

        private int[,] ConvertMapToInt(char[,] map, int row, int col, int[] start)
        {
            mapInt = new int[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (map[i, j] == 'X')
                    {
                        mapInt[i, j] = 1;
                    }
                    else
                    {
                        if (map[i, j] == 'K')
                        {
                            start[0] = i;
                            start[1] = j;
                        }
                        else if (map[i, j] == 'T')
                        {
                            goalStates.Add(new Tuple<int, int>(i, j));
                        }
                        mapInt[i, j] = 0;
                    }

                }
            }
            return mapInt;
        }

        private char[,] ConvertMap(string textFile, int row, int col)
        {
            // Read a text file line by line.
            string[] lines = System.IO.File.ReadAllLines(textFile);

            this.row = lines.Length;
            this.col = lines[0].Split(' ').Length;

            int k;

            char[,] map = new char[row, col];

            for (int i = 0; i < row; i++)
            {
                k = 0;
                for (int j = 0; j < col; j++)
                {
                    if (lines[i][k] == ' ')
                    {
                        k++;
                    }

                    if (lines[i][k] != 'R' && lines[i][k] != 'T' && lines[i][k] != 'T' && lines[i][k] != 'K' && lines[i][k] != 'X') {
                        throw new Exception();
                    }

                    map[i, j] = lines[i][k];
                    Console.Write(map[i, j]);
                    k++;
                }

                Console.Write("\n");
            }

            return map;
        }

        private void Map()
        {
            string[] lines = System.IO.File.ReadAllLines(openFile1.FileName);

            //Kalau udah ada input file, hapus dulu semua data
            if (this.row != 0)
            {
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        Controls.Remove(imageMatrix[i, j]);
                    }
                }
                Controls.Remove(errorText);
                this.row = 0;
                this.col = 0;
                this.duration = 0;

                map = new char[0, 0];
                start = new int[2];
                path = "";
                mapInt = new int[0, 0];
                goalStates = new List<Tuple<int, int>>();
                routeText.Text = "";
                timeText.Text = "0,00 ms";
                nodesText.Text = "0";
                stepsText.Text = "0";
            }

            this.row = lines.Length;
            this.col = lines[0].Split(' ').Length;
            map = ConvertMap(openFile1.FileName, this.row, this.col);

            PictureBox backGroundMap = new PictureBox();
            backGroundMap.BackColor = Color.Black;
            backGroundMap.Location = new Point(590, 124);
            backGroundMap.Size = new Size(880, 460);
            Controls.Add(backGroundMap);
            backGroundMap.SendToBack();

            int[,] MapInt = ConvertMapToInt(map, this.row, this.col, this.start);

            imageMatrix = new PictureBox[this.row, this.col];

            for (int i = 0; i < this.row; i++)
            {
                for (int j = 0; j < this.col; j++)
                {
                    var pictureBox = new PictureBox();

                    pictureBox.Location = new Point((j * 50) + 738, (i * 50) + 150);
                    pictureBox.Size = new Size(46, 46);

                    if (this.row > 8)
                    {
                        pictureBox.Location = new Point((j * 30) + 738, (i * 30) + 150);
                        pictureBox.Size = new Size(25, 25);
                    }


                    if (map[i, j] == 'K')
                    {
                        pictureBox.BackColor = Color.Red;
                    }
                    else if (map[i, j] == 'R')
                    {
                        pictureBox.BackColor = Color.White;
                    }
                    else if (map[i, j] == 'T')
                    {
                        pictureBox.BackColor = Color.BlueViolet;
                    }
                    else if (map[i, j] == 'X')
                    {
                        pictureBox.BackColor = Color.Black;
                    }

                    // set other properties of the picture box as needed

                    imageMatrix[i, j] = pictureBox;
                    Controls.Add(imageMatrix[i, j]);
                    imageMatrix[i, j].BringToFront();
                }
            }
        }

        private void outputLabel_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
