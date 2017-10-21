using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        public static int HoldTime = 0;
        static void Main(string[] args)
        { 

            AttachedTag tag = new AttachedTag("192.168.25.0", "AA");
            while (true)
            {
                Console.WriteLine("Presione enter para simular lectura.");
                var r = Console.ReadLine();
                if(r == "r")
                    tag = new AttachedTag("192.168.25.0", "AA");
                if (r.Contains("h"))
                {
                    try
                    {
                        HoldTime = Convert.ToInt32(r.Substring(2));
                        Console.WriteLine("HoldTime= " + HoldTime);
                    }
                    catch { }
                }
 
                tag.newRead();
            }
        }
        public class AttachedTag
        {
            public int ReadTimes { get; set; } = 1;
            public int WaitTime { get; set; } = 2;
            public string epc { get; set; }
            public string ip { get; set; }
            public DateTime timestamp { get; set; }

            public AttachedTag(string _ip, string _epc)
            {
                epc = _epc;
                ip = _ip;
                timestamp = DateTime.Now;
            }
            public bool newRead()
            {
                int elapsed = DateTime.Now.Subtract(timestamp).Minutes;
                Console.WriteLine("Elapsed minutes: " + elapsed);
                timestamp = DateTime.Now;
                ReadTimes++;
                Console.WriteLine("ReadTimes= " + ReadTimes);
                WaitTime = Fib(ReadTimes);
                Console.WriteLine("Fibonacci: " + WaitTime);
                WaitTime = HoldTime / 60 > WaitTime ? HoldTime / 60 : WaitTime;
                Console.WriteLine("HoldTime: " + HoldTime);
                Console.WriteLine("WaitTime: " + WaitTime);

                if (elapsed > 55)
                {
                    Console.WriteLine("Elapsed is greater than 55. WaitTime = 2, ReadTimes = 1;");
                    WaitTime = 2;
                    ReadTimes = 1;
                    return true;
                }

                Console.WriteLine("Response: (is elapsed greater than WaitTime)" + (elapsed > WaitTime));

                if (elapsed > WaitTime)
                    return true;
                else
                    return false;

            }
            static int Fib(int n)
            {
                double sqrt5 = Math.Sqrt(5);
                double p1 = (1 + sqrt5) / 2;
                double p2 = -1 * (p1 - 1);


                double n1 = Math.Pow(p1, n + 1);
                double n2 = Math.Pow(p2, n + 1);
                ulong fib = (ulong)((n1 - n2) / sqrt5);
                return fib > 50 ? 55 : (int)fib;
            }
        }
    }
}
