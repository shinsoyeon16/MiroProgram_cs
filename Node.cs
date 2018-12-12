using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiroProgram_cs
{
    /// <summary>
    /// 좌표와 연결리스트 구현을 위한 노드
    /// </summary>
    internal class Node
    {
        public int row { get; set; }
        public int col { get; set; }
        internal Node link { get; set; }
        internal Node() { }
        internal Node(int r, int c) { row = r; col = c; link = null; }
        internal void display()
        {
            Console.Write($"({row}, {col}) ");
        }
    }
}
