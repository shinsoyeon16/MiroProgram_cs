using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiroProgram_cs
{
    /// <summary>
    /// 배열로 만든 스택
    /// </summary>
    internal class ArrayStack
    {
        const int MAX_STACK_SIZE = 300;

        int top;
        Node[] node = new Node[MAX_STACK_SIZE];
        internal ArrayStack() { top = -1; }
        internal bool isEmpty()
        {
            return top == -1;
        }
        internal bool isFull()
        {
            return top == MAX_STACK_SIZE - 1;
        }

        internal void push(Node e)
        {
            if (isFull()) _error("스택 포화 에러");
            node[++top] = e;
        }
        internal Node pop()
        {
            if (isEmpty()) _error("스택 공백 에러");
            return node[top--];
        }
        internal Node peek()
        {
            if (isEmpty()) _error("스택 공백 에러");
            return node[top];
        }

        internal void _error(string msg)
        {
            Console.WriteLine("{msg}");
            return;
        }
    }
}
