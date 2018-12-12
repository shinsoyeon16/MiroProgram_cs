using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MiroProgram_cs
{
    /// <summary>
    /// 미로탐색을 위한 클래스
    /// </summary>
    public class Maze
    {
        #region Field

        /// <summary>
        ///  어디에서든 접근할 수 있도록 static으로 선언
        /// </summary>

        public static int MAZE_ROW_SIZE = 0;
        public static int MAZE_COL_SIZE = 0;

        public static char[][] MAZE;

        #endregion

        #region Maze관련함수

        /// <summary>
        /// 파일에 0과1로 저장된 미로와 행,열 정보를 읽어와 2차원배열에 저장하는 함수
        /// </summary>
        /// <param name="filename">미로가 저장된 URI의 정보를 담는 string 변수</param>
        /// 
        internal static void LoadMaze(string filename)
        {
            //파일의 모든 문자열을 읽어오기
            string mazeFile = File.ReadAllText(filename);

            //미로의 행, 열 정보 알아내기
            foreach (char a in mazeFile)
            {
                if (a == '\n') break;
                else MAZE_COL_SIZE++;
            }
            mazeFile.Trim();
            MAZE_ROW_SIZE = mazeFile.Length / MAZE_COL_SIZE;
            Console.WriteLine(MAZE_ROW_SIZE + " , " + MAZE_COL_SIZE);

            //이차원 배열 선언 및 미로 저장하기
            char[][] maze = new char[MAZE_ROW_SIZE][];
            for (int x = 0; x < MAZE_ROW_SIZE; x++)
            {
                maze[x] = new char[MAZE_COL_SIZE];
            }
            int i = 0, j = 0;
            foreach (char a in mazeFile)
            {
                if (j == MAZE_COL_SIZE) { i++; j = 0; }
                else
                {
                    maze[i][j] = a;
                    j++;
                }
                MAZE = maze;
            }
        }

        /// <summary>
        /// 화면에 미로를 예쁘게 출력하는 함수
        /// </summary>
        internal static void PrintMaze()
        {
            for (int i = 0; i < MAZE_ROW_SIZE; i++)
            {
                for (int j = 0; j < MAZE_COL_SIZE; j++)
                {
                    if (MAZE[i][j] == '0')
                    {
                        Console.Write("  ");
                    }
                    else if (MAZE[i][j] == '1')
                    {
                        Console.Write("■");
                    }
                    else if (MAZE[i][j] == '.')
                    {
                        Console.Write("□");
                    }
                    else if (MAZE[i][j] == 'e')
                    {
                        Console.Write("○");
                    }
                    else if (MAZE[i][j] == 'x')
                    {
                        Console.Write("◎");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 미로의 입구와 출구를 탐색하는 함수
        /// </summary>
        /// <returns></returns>
        internal static Node FindEntry()
        {
            Node entry = new Node();
            Node exit = new Node();
            for (int i = 0; i < Maze.MAZE_ROW_SIZE; i++)
            {
                for (int j = 0; j < Maze.MAZE_COL_SIZE; j++)
                {
                    if (MAZE[0][j] == '0') { entry.row = 0; entry.col = j; }
                    else if (MAZE[i][0] == '0') { entry.row = i; entry.col = 0; }
                    if (MAZE[Maze.MAZE_ROW_SIZE - 1][j] == '0') { exit.row = Maze.MAZE_ROW_SIZE; exit.col = j; }
                    else if (MAZE[i][Maze.MAZE_COL_SIZE - 2] == '0') { exit.row = i; exit.col = Maze.MAZE_COL_SIZE - 2; }
                }
            }

            MAZE[entry.row][entry.col] = 'e'; //미로의 입구를 e로 표시(출력시 가독성을 위해)
            MAZE[exit.row][exit.col] = 'x'; //미로의 출구를 x로 표시
            return entry;
        }

        /// <summary>
        /// 전달된 현재좌표로부터 상하좌우가 이동할 때 유효한지 확인한 뒤 bool값으로 반환하는 함수
        /// </summary>
        /// <param name="r">현재 좌표의 행 정보</param>
        /// <param name="c">현재 좌표의 열 정보</param>
        /// <returns></returns>
        internal static bool isValidLoc(int r, int c)
        {
            //유효하지 않은 미로 좌표일경우 false반환
            if (r < 0 || c < 0 || r >= MAZE_ROW_SIZE || c >= MAZE_COL_SIZE)
                return false;
            // 탐색할 수 있는 미로인지 검사 후 반환
            else
                return MAZE[r][c] == '0' || MAZE[r][c] == 'x';

        }

        /// <summary>
        /// 전달된 현재좌표로부터 상하좌우 중 갈 수 있는 길이 1가지인지 확인하는 함수
        /// </summary>
        /// <param name="r">현재 좌표의 행 정보</param>
        /// <param name="c">현재 좌표의 열 정보</param>
        /// <returns></returns>
        internal static bool isValidLocOnly(int r, int c)
        {
            int a = 0;
            if (r < 0 || c < 0 || r >= MAZE_ROW_SIZE || c >= MAZE_COL_SIZE)
                return false; // 유효하지 않은 길이면 false 반환
            else
            { //다른 분기점을 만나면 false, 현재 분기점이 이어지고있다면 true 반환
                if (isValidLoc(r - 1, c)) a++;
                if (isValidLoc(r + 1, c)) a++;
                if (isValidLoc(r, c - 1)) a++;
                if (isValidLoc(r, c + 1)) a++;
                return a == 1;
            }
        }

        #endregion

        #region EscapeMaze

        /// <summary>
        /// 미로를 탐색하여 탈출하는 함수
        /// </summary>
        internal static void EscapeMaze()
        {
            // field 선언부
            LinkedStack list = new LinkedStack(); // 다음 분기점의 정보를 저장하는 연결리스트 (스택)
            ArrayStack stack = new ArrayStack(); //현재 분기점 정보를 담는 스택 
            LinkedList path = new LinkedList(); // 전체 이동 경로를 저장하고 출력하는 연결리스트 (큐)

            list.push(FindEntry()); //미로의 입구를 찾기위한 함수 호출
            PrintMaze(); //미로 탐색 시작 전 출력

            while (list.isEmpty() == false) //탐색할 다음분기가 없을 때까지 반복하는 반복문
            {
                if (path.isEmpty() == false) //분기 탐색 종료마다 현재경로 출력을 위한 조건문
                {
                    Console.WriteLine("---현재까지 이동한 경로--- ");
                    path.display();
                    PrintMaze();
                }

                Node next = list.pop();
                stack.push(next);
                while (!stack.isEmpty())
                {
                    Node here = stack.pop(); //좌표  이동

                    int row = here.row;
                    int col = here.col;

                    // 미로의 출구를 탐색했다면 함수 종료
                    if (MAZE[row][col] == 'x')
                    {
                        path.enqueue(here);
                        Console.WriteLine("---전체경로--- ");
                        path.display();
                        Console.WriteLine("미로 탐색 성공!!");
                        PrintMaze();
                        return;
                    }

                    //현재위치가 출구가 아니라면 다음좌표 탐색
                    else
                    {
                        path.enqueue(here);
                        if (MAZE[row][col] != 'e') MAZE[row][col] = '.'; //지나온 자리를 표시한다.

                        //탐색할 수 있는 여러갈래가 없고 한 분기만 있다면 스택에 추가하여 탐색
                        if (isValidLocOnly(row, col))
                        {
                            if (isValidLoc(row - 1, col)) stack.push(new Node(row - 1, col));//상
                            else if (isValidLoc(row + 1, col)) stack.push(new Node(row + 1, col));//하
                            else if (isValidLoc(row, col - 1)) stack.push(new Node(row, col - 1));//좌
                            else stack.push(new Node(row, col + 1));//우
                        }

                        //여러개의 분기를 만나면 연결리스트에 추가하여 탐색
                        else
                        {
                            if (isValidLoc(row, col - 1)) list.push(new Node(row, col - 1));//좌
                            if (isValidLoc(row, col + 1)) list.push(new Node(row, col + 1));//우
                            if (isValidLoc(row - 1, col)) list.push(new Node(row - 1, col));//상
                            if (isValidLoc(row + 1, col)) list.push(new Node(row + 1, col));//하
                        }
                    }
                }
            }
            Console.WriteLine("미로 탐색에 실패하였습니다.");
        }
        #endregion
    }
}
