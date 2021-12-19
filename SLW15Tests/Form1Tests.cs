using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Graphs.Tests
{
    [TestClass()]
    public class Form1Tests
    {
        [TestMethod()]
        public void SearchTest()
        {
            Form1 form = new Form1();
            string path = @"C:\Users\Евгений\source\repos\SLW15\SLW15Tests\G.grf";
            StreamReader sr = new StreamReader(path);
            string data, ex = "";
            int i;
            List<int> e;
            while ((data = sr.ReadLine()) != null)
            {
                i = 2;
                ex = "";
                string id = data[0].ToString();
                int x;
                while (data[i] != ',')
                {
                    ex += data[i];
                    i++;
                }
                i++;
                x = Convert.ToInt32(ex);
                ex = "";
                int y;
                while (data[i] != ',')
                {
                    ex += data[i];
                    i++;
                }
                i++;
                y = Convert.ToInt32(ex);
                e = new List<int>();            
                while (data[i].ToString() != id)
                {
                    e.Add(data[i] - 48);
                    i += 2;
                }
                form._graph.LoadNode(Convert.ToInt32(id), x, y, id.ToString(), e);
            }
            List<int> expected = new List<int>();
            int size = form._graph._nodes.Count;
            form._graph._isUsed = new bool[size];
            form._graph._tin = new int[size];
            form._graph._tup = new int[size];
            form._graph._timer = 0;
            form._artPoints = new List<int>();
            expected.Add(1);
            expected.Add(4);
            form.Search(0);
            CollectionAssert.AreEqual(expected, form._artPoints);
            Console.WriteLine("Test passed!");
        }
    }
}