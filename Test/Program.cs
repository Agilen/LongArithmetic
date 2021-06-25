using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string bin = "0b1101010110011110101000011000011011000111010100000011000000000000000";//64
            string dec = "123143345413124360000";//22
            string hex = "0x4735D165054F53497233FBFB653EDCACA2AE5E5BC91AF12DA1FECA373573EE70";//16
            UInt64[] D;
            Console.WriteLine("18446744073709551615".Length);
                                                     
            D = ReadNumber(bin);
            for(int i = D.Length - 1; i >= 0; i--)
            {
                Console.Write(D[i]);
            }
            Console.WriteLine();
            D = ReadNumber(dec);
            for (int i = D.Length - 1; i >= 0; i--)
            {
                Console.Write(D[i]);
            }
            Console.WriteLine();
            D = ReadNumber(hex);
            for (int i = D.Length - 1; i >= 0; i--)
            {
                Console.Write(D[i]);
            }


        }
        
       
        static bool IsBinary(char ch)
        {
            if (ch == '1' || ch == '0')
                return true;
            else
            {
                return false;
            }
                        
        }
        static UInt64[] ReadNumber(string n)
        {
            n = n.Trim().ToLowerInvariant();
            
            int sys = 0;
            int m = 0;
            int lenght;
            long res;
            if (n.Length > 2)
            {
                if(n.Substring(0,2) == "0x" && n.Substring(2).All(ch => Uri.IsHexDigit(ch)))
                {
                    sys = 16;
                    m = 16;
                }
                else if(n.Substring(0, 2) == "0b" && n.Substring(2).All(ch => IsBinary(ch)))
                {
                    sys = 64;
                    m = 2;
                }
                else
                {
                    Console.WriteLine("Not bin or hex");
                }
            }
            n=n.Substring(2);
            n = LeadZero(n, sys);
            lenght = n.Length / sys;
            UInt64[] Digit = new UInt64[lenght];
            int k = Digit.Length - 1;
            for(int i = 0; i <= n.Length - sys; i += sys)
            {
                Digit[k] = Convert.ToUInt64(n.Substring(i, sys),m);
            }


            return Digit;
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
