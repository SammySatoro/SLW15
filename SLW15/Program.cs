using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Graphs
{
    public static class Match
    {
        public static double degtorad(double deg)
        {
            return deg * Math.PI / 180;
        }

        public static double radtodeg(double rad)
        {
            return rad / Math.PI * 180;
        }

        public static double lengthdir_x(double len, double dir)
        {
            return len * Math.Cos(degtorad(dir));
        }

        public static double lengthdirY(double len, double dir)
        {
            return len * Math.Sin(degtorad(dir)) * (-1);
        }

        public static double pointDirection(int x1, int y1, int x2, int y2) 
        {
            return 180 - radtodeg(Math.Atan2(y1 - y2, x1 - x2));
        }

        public static double pointDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }
    }

    public class Graph
    {       
        public class Node
        {
            public int _id;
            public int _active;
            public int _prev;
            public int _chk;
            public int _x;
            public int _y;
            public string _name;
            public List<int> _edges;

            public void AddEdge(int id)
            {
                if (!_edges.Contains(id)) _edges.Add(id);
            }

            public void RemoveEdge(int id)
            {
                _edges.Remove(id);
            }
        };

        public List<Node> _nodes = new List<Node>();
        private int _maxid = 0;
        public int _x = 0;
        public int _y = 0;
        public int _sz = 32;
        public bool[] _isUsed;
        public int _timer;
        public int[] _tin;
        public int[] _tup;

        public void AddNode(string name)
        {
            bool find = false;
            int id = 0;
            for (int i = 0; i < _maxid; i++)
            {
                bool exist = false;
                foreach (Node nd in _nodes)
                {
                    if (nd._id == i)
                    {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                {
                    id = i;
                    find = true;
                    break;
                }
            }
            if (!find)
            {
                id = _maxid;
                _maxid++;
            }
            Node n = new Node();
            n._id = id;
            n._active = 0;
            n._prev = -1;
            n._chk = -1;
            n._x = _x;
            n._y = _y;            
            if (name != "")
                n._name = name;
            else
                n._name = id.ToString();
            n._edges = new List<int>();
            _nodes.Add(n);
            _nodes.Sort((x, y) => x._id.CompareTo(y._id));
        }

        public void RemoveNode(int id)
        {
            Node n = null;
            foreach (Node nd in _nodes)
            {
                nd._edges.Remove(id);
                if (nd._id == id)
                {
                    n = nd;
                }
            }
            _nodes.Remove(n);
        }

        public void LoadNode(int id, int x, int y, string name, List<int> e)
        {
            Node n = new Node();
            if (_maxid <= id)
                _maxid = id + 1;
            n._id = id;
            n._active = 0;
            n._prev = -1;
            n._chk = -1;
            n._x = x;
            n._y = y;
            if (name != "")
                n._name = name;
            else
                n._name = id.ToString();
            n._edges = e;
            _nodes.Add(n);
            _nodes.Sort((xx, yy) => xx._id.CompareTo(yy._id));
        }
    }
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}