using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiroProgram_cs
{
    class MainApp
    {
        static void Main(string[] args)
        {
            Maze.LoadMaze("Maze_orign1.txt");
            Maze.EscapeMaze();
        }
    }
}
