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

            List<UInt32> a = new List<UInt32>();
            a.Add(0xE69A2D7B);
            a.Add(0x2C7351BB);
            List<UInt32> b = new List<UInt32>();
            b.Add(0x2D299561);
            b.Add(0xB63D50C1);


            T(a, b);


            

        }

        static void T(List<UInt32> a, List<UInt32> b)
        {
            UInt64 temp;
            UInt64 carry = 0;
            string str= "";
            List<UInt32> c = new List<UInt32>();
            List<UInt32> c2 = new List<UInt32>();
            List<UInt64> C = new List<UInt64>();
            List<UInt64> C2 = new List<UInt64>();
            List<UInt64> Res = new List<UInt64>();

            for (int i = 0; i < a.Count; i++)
            {
                temp = Convert.ToUInt64(a[i]) * Convert.ToUInt64(b[0])+carry;
                c.Add(Convert.ToUInt32(temp & 0xffffffff));
                carry = temp >> 32;
            }
            c.Add(Convert.ToUInt32(carry));
            
            if (c.Count % 2 != 0)
                c.Add(0);

            c2.Add(0);
            carry = 0;
            temp = 0;
            for(int i = 0; i < a.Count; i++)
            {
                temp = Convert.ToUInt64(a[i]) * Convert.ToUInt64(b[1]) + carry;
                c2.Add(Convert.ToUInt32(temp & 0xffffffff));
                carry = temp >> 32;
            }
            c2.Add(Convert.ToUInt32(carry));

            c.Reverse();
            c2.Reverse();

            for (int i = 0; i < c.Count; i++)
            {
                str +=P(Convert.ToString(c[i], 16));
            }
            for (int i = 0; i < str.Length; i += 16)
            {
                C.Add(UInt64.Parse(str.Substring(i, 16), System.Globalization.NumberStyles.HexNumber));
            }
           
            str = "";
            for (int i = 0; i < c.Count; i++)
            {
                str += P(Convert.ToString(c2[i], 16));
            }
            for (int i = 0; i < str.Length; i += 16)
            {
                C2.Add(UInt64.Parse(str.Substring(i, 16), System.Globalization.NumberStyles.HexNumber));

            }
            C.Reverse();
            C2.Reverse();
            Res = LongAdd(C, C2);

            List<UInt64> test = new List<UInt64>();
            
            for(int i = Res.Count - 1; i >= 0; i--)
            {
                Console.Write(P16(Convert.ToString((long)Res[i], 16)));
            }
            Console.WriteLine();
            test = Res;
            test=LongShiftDigitsToHighMul(test, 4);
            for(int i = 0; i < test.Count; i++)
            {
                Console.Write(P16(Convert.ToString((long)test[i], 16)));
            }
        }

        static  List<UInt64> LongAdd(List<UInt64> A, List<UInt64> B)
        {
            UInt64 temp;
            UInt64 carry = 0;
            List<UInt64> C = new List<UInt64>();
            int lenght = B.Count;

            for (int i = 0; i < lenght; i++)
            {
                temp = A[i] + B[i] + carry;
                C.Add(temp & 0xffffffffffffffff);
                if (IsCarryExist(A[i], B[i], carry) == true)
                    carry = 1;
                else
                    carry = 0;
            }
            return C;
        }

        static bool IsCarryExist(UInt64 A, UInt64 B, UInt64 carry)
        {
            string a = "";
            string b = "";
            string c = "";
            int lenght = 0;
            Convert.ToString(15, 16);
            A.ToString();
            a = FillBits(Convert.ToString((long)A, 2));
            b = FillBits(Convert.ToString((long)B, 2));
            long Ct = (long)(A + B);
            c = FillBits(Convert.ToString((long)Ct, 2));
            lenght = a.Length;


            if (a[0] == '1' && b[0] == '1')
            {
                return true;
            }
            else if (a[0] == '0' && b[0] == '0')
            {
                return false;
            }
            else
            {
                for (int i = 1; i < a.Length; i++)
                {
                    if (a[i] == '1' && b[i] == '1')
                    {
                        return true;
                    }
                    else if (a[i] == '0' && b[i] == '0')
                    {
                        return false;
                    }
                }
                return false;
            }
        }

        static string FillBits(string bits)
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

        static string P(string s)
        {
            if (s.Length < 8)
            {
                while (s.Length < 8)
                {
                    s = s.Insert(0, "0");
                }
            }
            return s;
        }

        static string P16(string s)
        {
            if (s.Length < 16)
            {
                while (s.Length < 16)
                {
                    s = s.Insert(0, "0");
                }
            }
            return s;
        }

        static List<UInt64> LongShiftDigitsToHighMul(List<UInt64> L, int i)
        {


            for (int k = 0; k < i; k++)
            {
                L.Add(0);

                for (int j = L.Count - 1; j >= 1; j--)
                {
                    L[j] = L[j - 1];
                    L[j - 1] = 0;
                }
            }

            return L;
        }
    }
}
