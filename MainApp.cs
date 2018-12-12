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
            Maze.LoadMaze("../../miro/Maze_2.txt");
            Maze.EscapeMaze();
        }
    }
}
