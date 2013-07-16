using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Performance.Aspect;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var mc = new MyClass();
            mc.MyMethod();
            Console.ReadKey();
        }
    }

    public class MyClass 
    {
        public void MyMethod() 
        {
            var yourClass = new YourClass();
            yourClass.YourMethod();

            Thread.Sleep(11);
            string x = "100";
        }
    }

    public class YourClass
    {
        public void YourMethod()
        {
            Thread.Sleep(110);
            string x = "100";
        }
    }
}
