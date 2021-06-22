using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<UInt64> A = new List<UInt64>();
            A.Add(0x65A80EEF243FEC47);
            A.Add(0x93A59686DF28D90B);
            A.Add(0xD2CE62E81B289C15);
            A.Add(0x3BD8774CB30BEBD5);
            A.Reverse();
            List<UInt64> B = new List<UInt64>();
            B.Add(0x65A80EEF243FEC47);
            B.Add(0x93A59686DF28D90B);
            B.Add(0xD2CE62E81B289C15);
            B.Add(0x3BD8774CB30BEBD5);
            for(int i = 0; i < B.Count; i++)
            {
                A[i] = B[i];
            }

            A.Clear();
            
            int[] a = new int[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            int[] b = new int[4] { 2, 3, 4, 5 };
            a = b;
           
            
            Console.WriteLine(B[2]);
            a[2] = 10;
            Console.WriteLine(B[2]);
        }

   
    }
}
