using System;
using System.Collections.Generic;
using System.Text;

namespace Arifm
{
    class LongArithmetic
    {

        private List<UInt64> A = new List<UInt64>();
        private List<UInt64> B = new List<UInt64>();
        private List<UInt64> C = new List<UInt64>();
        private List<UInt64> CTest = new List<UInt64>();
        private string a = "";
        private string b = "";
        private string c = "";



        public LongArithmetic(string a, string b, string c, string oper)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            FillList();
            if (oper == "+")
            {
                C = LongAdd(A, B);
            }
            else if (oper == "-")
            {
                C = LongSub(A, B);
            }
            else
            {
                C = LongMul(A, B);
            }
            if (Write(C) == this.c)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine($"Result: {Write(C)}");
            Console.WriteLine($"Expected Result: {this.c}");
            Console.WriteLine();
            Console.ResetColor();
        }


        private void FillList()
        {
            A.Clear();
            B.Clear();
            C.Clear();

            a = FillBits(a);
            b = FillBits(b);
            int lengthA = a.Length;
            int lengthB = b.Length;
            int lengthC = c.Length;
            int mod = lengthA % 16;

            for (int i = 0; i < lengthA - mod; i += 16)
            {
                A.Add(UInt64.Parse(a.Substring(i, 16), System.Globalization.NumberStyles.HexNumber));
            }
            A.Reverse();

            mod = lengthB % 16; ;

            for (int i = 0; i < lengthB - mod; i += 16)
            {
                B.Add(UInt64.Parse(b.Substring(i, 16), System.Globalization.NumberStyles.HexNumber));
            }
            B.Reverse();

        }

        private List<UInt64> LongAdd(List<UInt64> A, List<UInt64> B)
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

        private List<UInt64> LongSub(List<UInt64> A, List<UInt64> B)
        {
            List<UInt64> C = new List<UInt64>();
            UInt64 borrow = 0;
            int lenght = A.Count;
            for (int i = 0; i < lenght; i++)
            {
                C.Add(A[i] - B[i] - borrow);
                if (B[i] != 0 && (B[i] + borrow) == 0)
                {
                    borrow = 1;
                }
                else if (A[i] >= (B[i] + borrow))
                {
                    borrow = 0;
                }
                else
                {
                    borrow = 1;
                }

            }
            return C;
        }

        private List<UInt64> LongMulOneDigit(List<UInt32> A, UInt32 B1, UInt32 B2)
        {

            UInt64 temp;
            UInt64 carry = 0;
            string str = "";
            List<UInt32> c = new List<UInt32>();
            List<UInt32> c2 = new List<UInt32>();
            List<UInt64> C = new List<UInt64>();
            List<UInt64> C2 = new List<UInt64>();
            List<UInt64> Res = new List<UInt64>();

            for (int i = 0; i < A.Count; i++)
            {
                temp = Convert.ToUInt64(A[i]) * Convert.ToUInt64(B1) + carry;
                c.Add(Convert.ToUInt32(temp & 0xffffffff));
                carry = temp >> 32;
            }
            c.Add(Convert.ToUInt32(carry));

            if (c.Count % 2 != 0)
                c.Add(0);

            c2.Add(0);
            carry = 0;
            temp = 0;
            for (int i = 0; i < A.Count; i++)
            {
                temp = Convert.ToUInt64(A[i]) * Convert.ToUInt64(B2) + carry;
                c2.Add(Convert.ToUInt32(temp & 0xffffffff));
                carry = temp >> 32;
            }
            c2.Add(Convert.ToUInt32(carry));

            c.Reverse();
            c2.Reverse();

            for (int i = 0; i < c.Count; i++)
            {
                str += LeadZero8(Convert.ToString(c[i], 16));
            }
            for (int i = 0; i < str.Length; i += 16)
            {
                C.Add(UInt64.Parse(str.Substring(i, 16), System.Globalization.NumberStyles.HexNumber));
            }

            str = "";
            for (int i = 0; i < c.Count; i++)
            {
                str += LeadZero8(Convert.ToString(c2[i], 16));
            }
            for (int i = 0; i < str.Length; i += 16)
            {
                C2.Add(UInt64.Parse(str.Substring(i, 16), System.Globalization.NumberStyles.HexNumber));

            }
            C.Reverse();
            C2.Reverse();
            Res = LongAdd(C, C2);

            return Res;

        }

        private List<UInt64> LongMul(List<UInt64> A, List<UInt64> B)
        {
            string a = "";
            string b = "";
            int lenght = A.Count * 2;
            List<UInt64> temp = new List<UInt64>();
            List<UInt64> C = new List<UInt64>();
            List<UInt32> A32 = new List<UInt32>();
            List<UInt32> B32 = new List<UInt32>();

            A.Reverse();
            B.Reverse();
            for (int i = 0; i < A.Count; i++)
            {
                a += LeadZero(Convert.ToString((long)A[i], 16), 1);
            }

            for (int i = 0; i < B.Count; i++)
            {
                b += LeadZero(Convert.ToString((long)B[i], 16), 1);
            }

            for (int i = 0; i < a.Length; i += 8)
            {
                A32.Add(UInt32.Parse(a.Substring(i, 8), System.Globalization.NumberStyles.HexNumber));
            }

            for (int i = 0; i < b.Length; i += 8)
            {
                B32.Add(UInt32.Parse(b.Substring(i, 8), System.Globalization.NumberStyles.HexNumber));
            }

            A32.Reverse();
            B32.Reverse();

            for (int i = 0; i < lenght; i += 2)
            {
                temp = LongMulOneDigit(A32, B32[i], B32[i + 1]);

                temp = LongShiftDigitsToHighMul(temp, i / 2);

                C = PrepList(C, temp.Count);
                C = LongAdd(C, temp);

            }

            return C;

        }

        private (List<UInt64>, List<UInt64>) LongDivMod(List<UInt64> A, List<UInt64> B)
        {
            int k, t;
            List<UInt64> R = new List<UInt64>();
            List<UInt64> Q = new List<UInt64>();
            List<UInt64> C = new List<UInt64>();
            // PrepList(Q, A.Count);
            R = A;
            k = BitLenght(B);

            while (LongCmp(R, B) == 0 || LongCmp(R, B) == 1) ;
            {
                t = BitLenght(R);
                C = LongShiftDigitsToHighDiv(B, t - k);
                if (LongCmp(R, C) == -1)
                {
                    t--;
                    C = LongShiftDigitsToHighDiv(B, t - k);
                }
                R = LongSub(R, C);

            } 

            return (Q, R);
        }

        private int LongCmp(List<UInt64> A, List<UInt64> B)
        {
          
            for(int i= A.Count - 1; i >= 0; i--)
            {
                if (A[i] > B[i])
                {
                    return 1;
                }
                else if(A[i] < B[i])
                {
                    return -1;
                }
            } 

            return 0;
        }

        private List<UInt64> LongShiftDigitsToHighMul(List<UInt64> L, int i)
        {

            if (i == 0)
                return L;

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

        private List<UInt64> LongShiftDigitsToHighDiv(List<UInt64> L, int i)
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

            for (int j = buf.Length - 64; j >= 0; j -= 64)
            {
                L.Add(Convert.ToUInt64(buf.Substring(j, 64), 2));
            }
            L.Add(Convert.ToUInt64(buf.Substring(0, mod), 2));

            return L;
        }

        private List<UInt64> PrepList(List<UInt64> L, int n)
        {
            for (int i = L.Count; i < n; i++)
            {
                L.Add(0);
            }
            return L;
        }

        private int BitLenght(List<UInt64> L)
        {
            int l = 0;
            for (int i = 0; i < L.Count; i++)
            {
                l += Convert.ToString((long)L[i], 2).Length;
            }
            return l;
        }

        private string Write(List<UInt64> W)
        {
            string output = "";
            for (int i = W.Count - 1; i >= 0; i--)
            {
                output += LeadZero(W[i].ToString("X"), i);
            }

            return output;
        }

        private bool IsCarryExist(UInt64 A, UInt64 B, UInt64 carry)
        {
            string a = "";
            string b = "";
            a = FillBits(Convert.ToString((long)A, 2));
            b = FillBits(Convert.ToString((long)B, 2));

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

        private string FillBits(string bits)
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

        private string LeadZero8(string bits)
        {
            if (bits.Length < 8)
            {
                while (bits.Length < 8)
                {
                    bits = bits.Insert(0, "0");
                }
            }
            return bits;
        }
        private string LeadZero(string bits, int i)
        {
            if (i != C.Count - 1)
            {
                if (bits.Length < 16)
                {
                    while (bits.Length < 16)
                    {
                        bits = bits.Insert(0, "0");
                    }
                }
            }
            return bits;
        }

        private string LeadZeroBin(string bits)
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

