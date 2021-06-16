using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;

namespace Test
{
    class Program
    {
        //1100101101010
        //0000001110111011110010010000111111111011000100011110010011101001
        //0110010110100001101101111100101000110110010000101111010010110011
        //1001100010111010000001101100101000100111000001010100111011110110
        //0001110111010011001011001100001011111010111101010100000000000000
        //0000000000000000000000000000000000000000000000000000000000000000

        //196a
        //03bbc90ffb11e4e9
        //65a1b7ca3642f4b3
        //98ba06ca27054ef6
        //1dd32cc2faf54000 
        //0000000000000000
        static void Main(string[] args)
        {
            List<UInt64> A = new List<UInt64>();
            A.Add(0x65A80EEF243FEC47);
            A.Add(0x93A59686DF28D90B);
            A.Add(0xD2CE62E81B289C15);
            A.Add(0x3BD8774CB30BEBD5);
            
            LongShiftDigitsToHighDiv(A, 0);
        }

        static void LongShiftDigitsToHighDiv(List<UInt64> L, int i)
        {
            if (i <= 0)
                return L;

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

            for (int j = L.Count - 1; j >= 0; j--)
            {
                buf += LeadZeroBin((Convert.ToString((long)L[j], 2)));
            } 

            for (int j = 0; j < mod; j++)
            {
                buf = buf.Insert(buf.Length, "0");
            }
          
            L.Clear();
            
            for (int j = buf.Length - 64; j >= 0; j -=64)
            {
                L.Add(Convert.ToUInt64(buf.Substring(j, 64), 2));
            }
            L.Add(Convert.ToUInt64(buf.Substring(0, mod), 2));

            for(int j = L.Count - 1; j >= 0; j--)
            {
                Console.Write(LeadZero(Convert.ToString((long)L[j], 16)));
            }
        }

        static string LeadZero(string bits)
        {
            
                if (bits.Length < 16)
                {
                    while (bits.Length < 16)
                    {
                        bits = bits.Insert(0, "0");
                    }
                }
            
            return bits;
        }

        static string LeadZeroBin(string bits)
        {

            if (bits.Length < 64)
            {
                while (bits.Length < 64)
                {
                    bits = bits.Insert(0, "0");
                }
            }

            return bits;
        }
    }
}
