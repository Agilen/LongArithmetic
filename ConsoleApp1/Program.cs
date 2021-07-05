using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            UInt64[] Digit = new UInt64[7] { 0, 1, 023, 2, 2 ,0,0};
            ReadDec("12345678992134567");


             void ReadDec(string n)
            {
                n = n.Trim();
                UInt64[] numbers = new UInt64[n.Length];
                for (int i = 0; i < n.Length ; i++)
                {
                    numbers[n.Length - 1 - i] = UInt64.Parse(n.Substring(i, 1));
                }
                for(int i = 0; i < numbers.Length ; i++)
                {
                    Console.Write(numbers[i]);
                }
            }
        }
    }
}
