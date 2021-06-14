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
            Console.WriteLine("f80a0dfb70665b55645b5832f119f911fbe6eaa50950d49e2e126586c5dcc8e7872852a1".Length);


           
        }

        static void T(UInt64 A,UInt64 B,List<UInt64> Aa)
        {
            string a = "";
            string b = "";
            List<UInt32> A32 = new List<UInt32>();
            List<UInt32> B32 = new List<UInt32>();
            List<UInt32> C1 = new List<UInt32>();
            List<UInt32> C2 = new List<UInt32>();
            List<UInt32> Sum = new List<UInt32>();

            UInt64 temp;
            UInt64 carry = 0;

            a = Convert.ToString((long)A,16);
            b = Convert.ToString((long)B,16);

            for(int i = 0; i < Aa.Count; i++)
            {
                a += Aa[i];
            }
            while (a.Length % 16 ==0)
            {
                a = a.Insert(0, "0");
            }
            while (b.Length < 16)
            {
                b = b.Insert(0, "0");
            }

            for(int i = 0; i < a.Length; i += 8)
            {
                A32.Add(UInt32.Parse(a.Substring(i, 8), System.Globalization.NumberStyles.HexNumber));
            }
            A32.Reverse();

            B32.Add(UInt32.Parse(b.Substring(0, 8), System.Globalization.NumberStyles.HexNumber));
            B32.Add(UInt32.Parse(b.Substring(8, 8), System.Globalization.NumberStyles.HexNumber));
            B32.Reverse();           

            for (int i = 0; i < A32.Count; i++)
            {
                temp = A32[i] * b[0] + carry;
                C1.Add(Convert.ToUInt32(temp) & 0xffffffff);
                carry = temp >> 32;
            }
            C1.Add(Convert.ToUInt32(carry));

            C2.Add(0x00000000);
            for (int i = 0; i < A32.Count; i++)
            {
                temp = A32[i] * b[1] + carry;
                C2.Add(Convert.ToUInt32(temp) & 0xffffffff);
                carry = temp >> 32;
            }
            C2.Add(Convert.ToUInt32(carry));


        }
    }
}
