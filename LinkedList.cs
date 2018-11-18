using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiroProgram_cs
{
    internal class LinkedList
    {
        Node front;
        Node rear;
        internal LinkedList() { front = null; rear = null; }
        internal bool isEmpty() { return front == null; }
        internal void enqueue(Node p)
        {
            if (isEmpty()) front = rear = p;
            else
            {
                rear.Link=p;
                rear = p;
            }
        }
        internal Node dequeue()
        {
            if (isEmpty()) return null;
            Node p = front;
            front = front.Link;
            if (front == null) rear = null;
            return p;
        }
        internal Node peek() { return front; }
        internal void display()
        {
            for (Node p = front; p != null; p = p.Link)
                p.display();
            Console.WriteLine();
        }
    }
}
