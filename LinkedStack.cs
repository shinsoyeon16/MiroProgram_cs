using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiroProgram_cs
{
    internal class LinkedStack
    {
        Node top;
        internal LinkedStack() { top = null; }
        internal bool isEmpty()
        {
            return top == null;
        }

        internal void push(Node p)
        {
            if (isEmpty()) top = p;
            else
            {
                p.Link = top;
                top = p;
            }
        }
        internal Node pop()
        {
            if (isEmpty()) return null;
            Node p = top;
            top = top.Link;
            return p;
        }
        internal Node peek()
        {
            return top;
        }
        internal void display()
        {
            for (Node p = top; p != null; p = p.Link)
                p.display();
            Console.WriteLine();
        }
    }
}
