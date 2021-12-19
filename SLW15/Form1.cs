using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Graphs
{
    public partial class Form1 : Form
    {
        public Graph _graph = new Graph();
        public int _drag = -1;
        public int _drage = -1;
        public int _dx1 = 0;
        public int _dy1 = 0;
        public int _dx2 = 0;
        public int _dy2 = 0;
        public bool _act = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            pictureBox1.Width = this.Width - 16;
            pictureBox1.Height = this.Height - pictureBox1.Location.Y - 39;
            _graph._x = pictureBox1.Width / 2;
            _graph._y = pictureBox1.Height / 2;

            Bitmap buffer = new Bitmap(Width, Height);
            Graphics gfx = Graphics.FromImage(buffer);

            SolidBrush myBrush = new SolidBrush(Color.Black);
            SolidBrush myBrush2 = new SolidBrush(Color.White);
            Pen myPen = new Pen(Color.Black);
            Pen myPen2 = new Pen(Color.Green);

            gfx.Clear(Color.White);
            myPen2.Color = Color.Green;
            myBrush2.Color = Color.Green;
            foreach (Graph.Node n in _graph._nodes)
            {
                foreach (int eg in n._edges)
                {
                    foreach (Graph.Node m in _graph._nodes)
                    {
                        if (m._id == eg)
                        {
                            double a = Match.pointDirection(n._x, n._y, m._x, m._y);
                            double dist = Match.pointDistance(n._x, n._y, m._x, m._y);
                            gfx.DrawLine(myPen2,
                                new Point(n._x + (int)Match.lengthdir_x(_graph._sz / 2, a), n._y + (int)Match.lengthdirY(_graph._sz / 2, a)),
                                new Point(n._x + (int)Match.lengthdir_x(dist - (_graph._sz / 2), a),
                                n._y + (int)Match.lengthdirY(dist - (_graph._sz / 2), a)));
                            gfx.FillEllipse(myBrush2,
                                new Rectangle(n._x + (int)Match.lengthdir_x(dist - (_graph._sz / 2), a) - 4,
                                n._y + (int)Match.lengthdirY(dist - (_graph._sz / 2), a) - 4, 8, 8));
                        }
                    }
                }
            }
            foreach (Graph.Node n in _graph._nodes)
            {
                myBrush2.Color = Color.White;
                if (n._active == 1)
                    myBrush2.Color = Color.LightBlue;
                if (n._active == 2)
                    myBrush2.Color = Color.LightGray;
                if (n._active == 3)
                    myBrush2.Color = Color.Orange;
                if (n._active == 4)
                    myBrush2.Color = Color.DarkRed;
                gfx.FillEllipse(myBrush2, new Rectangle(n._x - _graph._sz / 2, n._y - _graph._sz / 2, _graph._sz, _graph._sz));
                gfx.DrawEllipse(myPen, new Rectangle(n._x - _graph._sz / 2, n._y - _graph._sz / 2, _graph._sz, _graph._sz));
                gfx.DrawString(n._name, new Font("Arial", 8, FontStyle.Regular), myBrush, new PointF(n._x - _graph._sz / 3, n._y - 10));
            }
            if (_drage != -1)
            {
                myBrush2.Color = Color.Green;
                if (checkBox2.Checked)
                {
                    myPen2.Color = Color.Red;
                    myBrush2.Color = Color.Red;
                }
                double a1 = Match.pointDirection(_dx1, _dy1, _dx2, _dy2);
                double dist1 = Match.pointDistance(_dx1, _dy1, _dx2, _dy2);
                gfx.DrawLine(myPen2,
                    new Point(_dx1 + (int)Match.lengthdir_x(_graph._sz / 2, a1), _dy1 + (int)Match.lengthdirY(_graph._sz / 2, a1)),
                    new Point(_dx1 + (int)Match.lengthdir_x(dist1, a1), _dy1 + (int)Match.lengthdirY(dist1, a1)));
                gfx.FillEllipse(myBrush2,
                    new Rectangle(_dx1 + (int)Match.lengthdir_x(dist1, a1) - 4, _dy1 + (int)Match.lengthdirY(dist1, a1) - 4, 8, 8));
            }

            pictureBox1.Image = buffer;
            myBrush.Dispose();
            myBrush2.Dispose();
            myPen.Dispose();
            myPen2.Dispose();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!_act)
                _graph.AddNode(textBox1.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer2.Enabled)
            {
                button3.Text = "Pause";
            }
            else
            {
                button3.Text = "Continue";
            }
            if (_act)
            {
                button2.Text = "Stop";
                button3.Enabled = true;
                button1.Enabled = false;
                radioButton2.Enabled = false;
                progressBar1.Visible = true;
                button5.Enabled = false;
            }
            else
            {
                button2.Text = "DFS";
                button3.Text = "";
                button3.Enabled = false;
                button1.Enabled = true;
                radioButton2.Enabled = true;
                progressBar1.Visible = false;
                button5.Enabled = true;
            }
            if (_graph._nodes.Count == 0 || _act)
            {
                button4.Enabled = false;
            }
            else
            {
                button4.Enabled = true;
            }

            for (int i = 0; i < _graph._nodes.Count; i++)
            {
                for (int j = 0; j < _graph._nodes.Count; j++)
                {
                    if (i != j)
                    {
                        double dist = Match.pointDistance(_graph._nodes[i]._x, _graph._nodes[i]._y, _graph._nodes[j]._x, _graph._nodes[j]._y);
                        int sz_in = 10;
                        if (dist <= (_graph._sz + sz_in))
                        {
                            var rand = new Random();
                            if (_graph._nodes[i]._x == _graph._nodes[j]._x)
                            {
                                if (rand.Next(2) == 1)
                                    _graph._nodes[i]._x += 1;
                                else
                                    _graph._nodes[i]._x -= 1;
                            }
                            if (_graph._nodes[i]._y == _graph._nodes[j]._y)
                            {
                                if (rand.Next(2) == 1)
                                    _graph._nodes[i]._y += 1;
                                else
                                    _graph._nodes[i]._y -= 1;
                            }                          
                            if (_graph._nodes[i]._x < _graph._nodes[j]._x)
                            {
                                _graph._nodes[i]._x -= (int)(_graph._sz + sz_in - dist);
                                _graph._nodes[j]._x += (int)(_graph._sz + sz_in - dist);
                            }
                            else
                            {
                                _graph._nodes[i]._x += (int)(_graph._sz + sz_in - dist);
                                _graph._nodes[j]._x -= (int)(_graph._sz + sz_in - dist);
                            }
                            if (_graph._nodes[i]._y < _graph._nodes[j]._y)
                            {
                                _graph._nodes[i]._y -= (int)(_graph._sz + sz_in - dist);
                                _graph._nodes[j]._y += (int)(_graph._sz + sz_in - dist);
                            }
                            else
                            {
                                _graph._nodes[i]._y += (int)(_graph._sz + sz_in - dist);
                                _graph._nodes[j]._y -= (int)(_graph._sz + sz_in - dist);
                            }
                        }
                    }
                }
                if (_graph._nodes[i]._x - _graph._sz / 2 < 0) _graph._nodes[i]._x = _graph._sz / 2;
                if (_graph._nodes[i]._y - _graph._sz / 2 < 0) _graph._nodes[i]._y = _graph._sz / 2;
                if (_graph._nodes[i]._x + _graph._sz / 2 > pictureBox1.Width) _graph._nodes[i]._x = pictureBox1.Width - _graph._sz / 2 - 1;
                if (_graph._nodes[i]._y + _graph._sz / 2 > pictureBox1.Height) _graph._nodes[i]._y = pictureBox1.Height - _graph._sz / 2 - 1;                
            }
            Refresh();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_drag != -1)
            {
                foreach (Graph.Node n in _graph._nodes)
                {
                    if (_drag == n._id)
                    {
                        n._x = e.X;
                        n._y = e.Y;
                        break;
                    }
                }
            }
            if (_drage != -1)
            {
                foreach (Graph.Node n in _graph._nodes)
                {
                    if (_drage == n._id)
                    {
                        _dx1 = n._x;
                        _dy1 = n._y;
                        _dx2 = e.X;
                        _dy2 = e.Y;
                        break;
                    }
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _drage = -1;
                if (_drag == -1)
                {
                    foreach (Graph.Node n in _graph._nodes)
                    {
                        if (Match.pointDistance(n._x, n._y, e.X, e.Y) < _graph._sz / 2)
                        {
                            _drag = n._id;
                            n._x = e.X;
                            n._y = e.Y;
                            break;
                        }
                    }
                }
            }
            if (!_act)
            {
                if (e.Button == MouseButtons.Middle)
                {
                    _drag = -1;
                    _drage = -1;
                    foreach (Graph.Node n in _graph._nodes)
                    {
                        if (Match.pointDistance(n._x, n._y, e.X, e.Y) < _graph._sz / 2)
                        { 
                            _graph.RemoveNode(n._id);
                            break;
                        }
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    _drag = -1;
                    _dx1 = 0;
                    _dy1 = 0;
                    _dx2 = 0;
                    _dy2 = 0;
                    foreach (Graph.Node n in _graph._nodes)
                    {
                        if (Match.pointDistance(n._x, n._y, e.X, e.Y) < _graph._sz / 2)
                        {
                            _drage = n._id;
                            _dx1 = n._x;
                            _dy1 = n._y;
                            _dx2 = e.X;
                            _dy2 = e.Y;
                            break;
                        }
                    }
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _drag = -1;
            if (e.Button == MouseButtons.Right)
            {
                if (_drage != -1)
                {
                    foreach (Graph.Node n in _graph._nodes)
                    {
                        if (Match.pointDistance(n._x, n._y, e.X, e.Y) < _graph._sz / 2)
                        {
                            if (n._id != _drage)
                            {
                                foreach (Graph.Node m in _graph._nodes)
                                {
                                    if (m._id == _drage)
                                    {
                                        if (checkBox2.Checked)
                                        {
                                            m.RemoveEdge(n._id);
                                            if (!checkBox1.Checked)
                                                n.RemoveEdge(m._id);
                                        }
                                        else
                                        {
                                            m.AddEdge(n._id);
                                            if (!checkBox1.Checked)
                                                n.AddEdge(m._id);
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                _drage = -1;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {       
            if (radioButton2.Checked)
            {                
                bool fa = false;
                for (int i = 0; i < _graph._nodes.Count; i++)
                {
                    
                    if (_graph._nodes[i]._active == 1)
                    {
                        if (_graph._nodes[i]._chk == -1)
                        {
                           
                        }
                        bool st = false;
                        while (!st && (_graph._nodes[i]._chk < _graph._nodes[i]._edges.Count - 1))
                        {
                            _graph._nodes[i]._chk++;
                            foreach (Graph.Node m in _graph._nodes)
                            {
                                if (m._id == _graph._nodes[i]._edges[_graph._nodes[i]._chk])
                                {
                                    if (m._active == 0)
                                    {
                                        m._active = 1;
                                        m._prev = _graph._nodes[i]._id;
                                        _graph._nodes[i]._active = 3;
                                        st = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (!(_graph._nodes[i]._chk < _graph._nodes[i]._edges.Count - 1))
                        {
                            bool noa = true;
                            foreach (Graph.Node m in _graph._nodes)
                            {
                                if (_graph._nodes[i]._edges.Contains(m._id) && m._active == 1)
                                    noa = false;
                            }
                            if (noa)
                            {                            
                                for (int j = 0; j < _artPoints.Count; j++)
                                {
                                    if (_graph._nodes[i]._id == _artPoints[j])
                                    {                                        
                                        _graph._nodes[i]._active = 4;
                                        textBox2.Text += $"{_graph._nodes[i]._id} ";
                                    }
                                        
                                }
                                if (_graph._nodes[i]._active != 4)
                                {
                                    _graph._nodes[i]._active = 2;
                                } 
                                if (_graph._nodes[i]._prev != -1)
                                {
                                    foreach (Graph.Node m in _graph._nodes)
                                    {
                                        if (m._id == _graph._nodes[i]._prev)
                                            m._active = 1;
                                    }
                                }
                            }
                        }
                        fa = true;
                        break;
                    }
                }
                if (!fa)
                {
                    fa = false;
                    for (int i = 0; i < _graph._nodes.Count; i++)
                    {
                        if (_graph._nodes[i]._active == 0)                                                        
                        {
                            _graph._nodes[i]._active = 1;
                            fa = true;
                            break;
                        }
                    }
                    if (!fa)
                    {
                        foreach (Graph.Node n in _graph._nodes)
                            n._active = 0;
                        timer2.Stop();
                        _act = false;                        
                    }
                    if (_isExists == false)
                        MessageBox.Show("Articulation points do not exist");
                }                
            }
            
            int k = 0;
            foreach (Graph.Node n in _graph._nodes)
                if (n._active == 2) k += 1;
            progressBar1.Maximum = _graph._nodes.Count;
            if (progressBar1.Value != k)
                progressBar1.PerformStep();          
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            _artPoints = new List<int>();
            _isExists = false;
            _artPoints.Clear();
            textBox2.Text = "";
            int size = _graph._nodes.Count;
            _graph._isUsed = new bool[size];
            _graph._tin = new int[size];
            _graph._tup = new int[size];
            _graph._timer = 0;
            Search(0);
            _drag = -1;
            _drage = -1;
            foreach (Graph.Node n in _graph._nodes)
            {
                n._active = 0;
                n._prev = -1;
                n._chk = -1;
            }
            if (!timer2.Enabled && !_act)
            {
                if (_graph._nodes.Count > 0)
                {
                    _graph._nodes[0]._active = 1;
                    timer2.Start();
                    _act = true;
                    progressBar1.Value = 0;                  
                }
            }
            else
            {
                timer2.Stop();
                _act = false;
            }    
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _drag = -1;
            _drage = -1;
            if (!timer2.Enabled)
            {
                if (_graph._nodes.Count > 0)
                {
                    bool a = true;
                    foreach (Graph.Node n in _graph._nodes)
                        if (n._active != 0) a = false;
                    if (a)
                    {
                        _graph._nodes[0]._active = 1;
                    }
                    timer2.Start();
                    _act = true;
                }
            }
            else
            {
                timer2.Stop();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_graph._nodes.Count != 0)
                saveFileDialog1.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (File.Exists(saveFileDialog1.FileName))
                File.Delete(saveFileDialog1.FileName);
            FileStream F = File.OpenWrite(saveFileDialog1.FileName);
            foreach (Graph.Node n in _graph._nodes)
            {
                string S = "";
                S += n._id.ToString() + ",";
                S += n._x.ToString() + ",";
                S += n._y.ToString() + ",";
                if (n._edges.Count != 0)
                {
                    foreach (int eg in n._edges)
                    {
                        S += eg.ToString() + ";";
                    }
                    S = S.Remove(S.Length - 1, 1);
                }
                S += "," + n._name + "\n";
                byte[] info = new UTF8Encoding(true).GetBytes(S);
                F.Write(info, 0, info.Length);
            }
            F.SetLength(F.Length - 1);
            F.Close();
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _graph._nodes.Clear();
            StreamReader F = File.OpenText(openFileDialog1.FileName);
            while (!F.EndOfStream)
            {
                string S = F.ReadLine();
                string[] SS = S.Split(',');
                List<int> L = new List<int>();
                if (SS[3] != "")
                {
                    string[] SSE = SS[3].Split(';');
                    foreach (string eg in SSE)
                        L.Add(int.Parse(eg));
                }
                _graph.LoadNode(int.Parse(SS[0]), int.Parse(SS[1]), int.Parse(SS[2]), SS[4], L);
            }
            F.Close();
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private int Min(int a, int b)
        {
            if (a < b) return a;
            else if (a > b) return b;
            else return a;
        }
        private bool _isExists = false;
        public List<int> _artPoints;
        public void Search(int vertex, int p = -1)
        {
            try
            {
                _graph._isUsed[vertex] = true;
                _graph._tin[vertex] = _graph._timer++;
                _graph._tup[vertex] = _graph._tin[vertex];
                int children = 0;
                for (int i = 0; i < _graph._nodes[vertex]._edges.Count; i++)
                {
                    int to = _graph._nodes[vertex]._edges[i];
                    if (to == p) continue;
                    if (_graph._isUsed[to])
                        _graph._tup[vertex] = Min(_graph._tup[vertex], _graph._tin[to]);
                    else
                    {
                        Search(to, vertex);
                        _graph._tup[vertex] = Min(_graph._tup[vertex], _graph._tup[to]);
                        if (_graph._tup[to] >= _graph._tin[vertex] && p != -1)
                        {
                            if (_isExists == false) _isExists = true;
                            if (!_artPoints.Contains(_graph._nodes[vertex]._id))
                                _artPoints.Add(_graph._nodes[vertex]._id);
                           //_graph._nodes[vertex]._active = 4;
                        }
                        ++children;
                    }
                }
                if (p == -1 && children > 1)
                {
                    if (_isExists == false) _isExists = true;
                    if (!_artPoints.Contains(_graph._nodes[vertex]._id))
                        _artPoints.Add(_graph._nodes[vertex]._id);
                    //_graph._nodes[vertex]._active = 4;
                }
            }
            catch
            {
                MessageBox.Show("Graph does not exist. " +
                    "Add at least one vertex");
            }           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}