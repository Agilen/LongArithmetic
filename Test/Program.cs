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
            UInt64[] l = A.ToArray();
            UInt64[] res;
            //res = LongShiftDigitsToHighDiv(l, 76);
            //for (int i = res.Length - 1; i >= 0; i--)
            //{
            //    Console.Write(LeadZero(res[i].ToString("X"),16));
            //}
           
            Console.WriteLine(Convert.ToInt64(Math.Floor(Convert.ToDecimal((t - k) / 64))));
        }

        static UInt64[] LongShiftDigitsToHighDiv(UInt64[] L, int i)
        {

            if (i <= 0)
                return L;

            string buf = "";
            int mod = i % 64;
            int m = (i - mod) / 64;
            for (int k = 0; k < m; k++)
            { 
                for (int j = L.Length - 1; j >= 1; j--)
                {
                    L[j] = L[j - 1];
                    L[j - 1] = 0;
                }
            }

            for (int j = L.Length - 1; j >= 0; j--)
            {
                buf += LeadZero((Convert.ToString((long)L[j], 2)), 64);
            }

            for (int j = 0; j < mod; j++)
            {
                buf = buf.Insert(buf.Length, "0");
            }
            int n = 0;
            for (int j = buf.Length - 64; j >= 0; j -= 64)
            {
                L[n] = (Convert.ToUInt64(buf.Substring(j, 64), 2));
                n++;
            }
            L[n] = (Convert.ToUInt64(buf.Substring(0, mod), 2));

            return L;
        }
        static UInt64[] LongShiftDigitsToHighDivList(UInt64[] r, int i)
        {
            List<UInt64> L = new List<UInt64>();
            
            for(int j = 0; j < r.Length; j++)
            {
                L.Add(0);
                L[j] = r[j];
            }

            if (i <= 0)
                return L.ToArray();

            string buf = "";
            int mod = i % 64;
            int m = (i - mod) / 64;
            for (int k = 0; k < m; k++)
            {
                L.Add(0);

                for (int j = L.Count - 1; j >= 1; j--)
                {
                    L[j] = L[j - 1];
                    L[j - 1] = 0;
                }
            }
            if (mod != 0)
            {
                for (int j = L.Count - 1; j >= 0; j--)
                {
                    buf += LeadZero(Convert.ToString((long)L[j], 2), 64);
                }

                for (int j = 0; j < mod; j++)
                {
                    buf = buf.Insert(buf.Length, "0");
                }

                L.Clear();

                for (int j = buf.Length - 64; j >= 0; j -= 64)
                {
                    L.Add(Convert.ToUInt64(buf.Substring(j, 64), 2));
                }
                L.Add(Convert.ToUInt64(buf.Substring(0, mod), 2));

                while (L.Count < r.Length)
                {
                    L.Add(0);
                }
            }
            return L.ToArray();
        }

        static string LeadZero(string bit, int nBit)
        {
            if (bit.Length % nBit != 0)
            {
                while (bit.Length % nBit != 0)
                {
                    bit = bit.Insert(0, "0");
                }
            }

            return bit;
        }


    }
}
