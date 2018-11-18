using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiroProgram_cs
{
    internal class Node
    {
        private Node link;
        public int row { get; set; }
        public int col { get; set; }
        internal Node Link { get; set; }
        internal Node() { }
        internal Node(int r, int c) { row = r; col = c; link = null; }
        internal void display()
        {
            Console.Write($"({row}, {col}) ");
        }
    }
}
