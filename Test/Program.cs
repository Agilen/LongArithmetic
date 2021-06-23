using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
          
            List<UInt64> B = new List<UInt64>();
            B.Add(0x65A80EEF243FEC47);
            B.Add(0x93A59686DF28D90B);
            B.Add(0xD2CE62E81B289C15);
            B.Add(0x3BD8774CB30BEBD5);
           
            int k = LongCmp(A.ToArray(), B.ToArray());

            Console.WriteLine(BitLengthV2(B.ToArray()));
            Console.WriteLine(BitLength(B.ToArray()));
          

           
        }

        static int BitLengthV2(UInt64[] Bl)
        {
            int k = 0;
            for (int i = 0; i < Bl.Length; i++)
            {
                k += Convert.ToString((long)Bl[i], 2).Length;
            }
            return k;
        }

        static int BitLength(UInt64[] Bl)
        {
            int k = 0;

            for (int i = 0; i < Bl.Length; i++)
            {
                k += Convert.ToInt32(Math.Ceiling(Math.Log2(Bl[i] + 1)));
            }

            return k;
        }
        static int LongCmp(UInt64[] A, UInt64[] B)
        {
            int minLength = new int[2] {A.Length,B.Length}.Min();
            
            if (A.Length != B.Length)
            {
                if (A.Length > B.Length)
                {
                    minLength = B.Length;
                    for (int i = A.Length - 1; i > minLength-1; i--)
                    {
                        if (A[i] != 0)
                            return 1;
                    }
                }
                else
                {
                    minLength = A.Length;
                    for (int i = B.Length - 1; i > minLength-1; i--)
                    {
                        if (B[i] != 0)
                            return -1;
                    }
                }
            }

            for (int i = minLength - 1; i >= 0; i--)
            {
                if (A[i] > B[i])
                {
                    return 1;
                }
                else if (A[i] < B[i])
                {
                    return -1;
                }
            }

            return 0;
        }
    }
}
