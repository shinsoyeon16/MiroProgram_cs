using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MiroProgram_cs
{
    public class Maze
    {
        //필드를 static으로 선언
        public static int MAZE_ROW_SIZE = 0;
        public static int MAZE_COL_SIZE = 0;
        public static char[][] MAZE;

        internal static void LoadMaze(string filename) //파일로부터 미로를 읽어오는 함수
        {
            string mazeFile = File.ReadAllText(filename);
            //행, 열 정보 읽어오기
            foreach (char a in mazeFile)
            {
                if (a == '\n') break;
                MAZE_COL_SIZE++;
            }
            foreach (char a in mazeFile)
            {
                if (a == '\n') MAZE_ROW_SIZE++;
            }

            //이차원 배열 선언 및 미로 저장하기
            char[][] maze = new char[MAZE_ROW_SIZE][];
            for (int x = 0; x < MAZE_ROW_SIZE; x++)
            {
                maze[x] = new char[MAZE_COL_SIZE];
            }
            int i = 0, j = 0;
            foreach (char a in mazeFile)
            {
                if (j > MAZE_COL_SIZE) { i++; j = 0; }
                if (a != '\n')
                    maze[i][j] = a;
                j++;
            }
            MAZE = maze;
        }

        internal static void PrintMaze() //화면에 미로를 예쁘게 출력하는 함수
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

        internal static Node FindEntry() //미로의 입구를 탐색하는 함수
        {
            Node entry = new Node();
            Node exit = new Node();
            for (int i = 0; i < Maze.MAZE_ROW_SIZE; i++)
            {
                for (int j = 0; j < Maze.MAZE_COL_SIZE; j++)
                {
                    if (MAZE[0][j] == '0') { entry.row = 0; entry.col = j; break; }
                    else if (MAZE[i][0] == '0') { entry.row = i; entry.col = 0; break; }
                    else if (MAZE[0][Maze.MAZE_COL_SIZE] == '0') { exit.row = 0; exit.col = MAZE_COL_SIZE; break; }
                    else if (MAZE[MAZE_ROW_SIZE][j] == '0') { exit.row = MAZE_ROW_SIZE; exit.col = j; break; }
                }
            }

            MAZE[entry.row][entry.col] = 'e'; //미로의 입구를 e로 표시(출력시 가독성을 위해)
            MAZE[exit.row][exit.col] = 'x'; //미로의 출구를 x로 표시
            return entry;
        }

        internal static bool isValidLoc(int r, int c) //현재좌표로부터 4방향이 유효한 길인지 탐색하는 함수
        {
            //유효하지 않은 미로 좌표일경우 false반환
            if (r < 0 || c < 0 || r >= MAZE_ROW_SIZE || c >= MAZE_COL_SIZE)
                return false;
            // 탐색할 수 있는 미로인지 검사 후 반환
            else
                return MAZE[r][c] == '0' || MAZE[r][c] == 'x';

        }
        internal static bool isValidLocOnly(int r, int c) //현재 분기점에 대한 미로정보를 탐색하는 함수
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
        internal static void EscapeMaze()
        {
            LinkedStack list = new LinkedStack(); // 다음 분기점의 정보를 저장하기 위한 연결리스트 (스택)
            ArrayStack stack = new ArrayStack(); //현재 분기점 탐색을 위한 스택 
            LinkedList path = new LinkedList(); // 전체 이동 경로를 저장하기 위한 연결리스트 (큐)

            list.push(FindEntry()); //미로의 입구를 찾기위한 함수 호출
            PrintMaze(); //미로 초기상태 출력

            while (list.isEmpty() == false) //연결리스트에 데이터가 없을때까지 반복하는 반복문
            {
                if (path.isEmpty() == false) //각 분기 탐색 종료마다 현재경로와 미로상태 출력
                {
                    Console.Write("\n현재경로: ");
                    path.display(); //현재까지의 경로 출력
                    PrintMaze();
                }

                Node next = list.pop();
                stack.push(next);
                while (!stack.isEmpty())
                {
                    Node here = stack.pop(); //좌표  이동 
                    path.enqueue(here);

                    int row = here.row;
                    int col = here.col;

                    //현재위치가 미로의 출구라면 함수 종료
                    if (MAZE[row][col] == 'x')
                    {
                        Console.Write("전체경로 : ");
                        path.display(); //전체경로 출력
                        Console.WriteLine("미로 탐색 성공!!");
                        PrintMaze();
                        return;
                    }

                    //현재위치가 미로의 출구가 아니라면 미로 탐색
                    else
                    {
                        MAZE[row][col] = '.'; //지나온 자리 확인을 위해 변경

                        //현재 분기는 스택에 추가하여 탐색
                        if (isValidLocOnly(row, col))
                        {
                            if (isValidLoc(row - 1, col)) stack.push(new Node(row - 1, col));//상
                            else if (isValidLoc(row + 1, col)) stack.push(new Node(row + 1, col));//하
                            else if (isValidLoc(row, col - 1)) stack.push(new Node(row, col - 1));//좌
                            else stack.push(new Node(row, col + 1));//우
                        }

                        //다른 분기를 만나면 연결리스트에 추가
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
    }
}
