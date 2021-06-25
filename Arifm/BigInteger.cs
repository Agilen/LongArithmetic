using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arifm
{
    class BigInteger
    {
        private UInt64[] A;
        private UInt64[] B;
        private UInt64[] C;
        private (UInt64[], UInt64[]) DivRes;
        public BigInteger(string a, string b, string c)
        {
            FillData(a, b);
        }

        private void FillData(string a, string b)
        {
            int length;
            a = LeadZero(a, 64);
            length = a.Length / 16;
            A = new UInt64[length];
            int k = 0;
            for (int i = a.Length - 16; i > -1; i -= 16)
            {
                A[k] = UInt64.Parse(a.Substring(i, 16), System.Globalization.NumberStyles.HexNumber);
                k++;
            }


            b = LeadZero(b, 64);
            length = b.Length / 16;
            B = new UInt64[length];
            k = 0;
            for (int i = b.Length - 16; i > -1; i -= 16)
            {
                B[k] = UInt64.Parse(b.Substring(i, 16), System.Globalization.NumberStyles.HexNumber);
                k++;
            }

        }

        private UInt64[] LongAdd(UInt64[] A, UInt64[] B)
        {
            UInt64[] C = new UInt64[A.Length];
            UInt64 temp;
            UInt64 carry = 0;

            for (int i = 0; i < A.Length; i++)
            {
                temp = A[i] + B[i] + carry;
                C[i] = temp & 0xffffffffffffffff;
                if (IsCarryExist(A[i], B[i], carry) == true)
                    carry = 1;
                else
                    carry = 0;
            }

            return C;
        }

        private UInt64[] LongSub(UInt64[] A, UInt64[] B)
        {
            UInt64[] C = new UInt64[A.Length];
            UInt64 borrow = 0;
            for (int i = 0; i < A.Length; i++)
            {
                C[i] = A[i] - B[i] - borrow;
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

        private UInt64[] LongMulOneDigit(UInt32[] A, UInt32 B1, UInt32 B2)
        {

            UInt64 temp;
            UInt64 carry = 0;
            string str = "";
            UInt32[] c = new UInt32[A.Length + 2];
            UInt32[] c2 = new UInt32[A.Length + 2];
            UInt64[] C = new UInt64[c.Length / 2 + 1];
            UInt64[] C2 = new UInt64[c2.Length / 2 + 1];
            UInt64[] buf;
            UInt64[] Res = new UInt64[A.Length];

            for (int i = 0; i < A.Length; i++)
            {
                temp = Convert.ToUInt64(A[i]) * Convert.ToUInt64(B1) + carry;
                c[i] = (Convert.ToUInt32(temp & 0xffffffff));
                carry = temp >> 32;
            }
            c[c.Length - 2] = (Convert.ToUInt32(carry));

            carry = 0;
            temp = 0;
            for (int i = 0; i < A.Length; i++)
            {
                temp = Convert.ToUInt64(A[i]) * Convert.ToUInt64(B2) + carry;
                c2[i + 1] = (Convert.ToUInt32(temp & 0xffffffff));
                carry = temp >> 32;
            }
            c2[c2.Length - 1] = (Convert.ToUInt32(carry));



            for (int i = c.Length - 1; i >= 0; i--)
            {
                str += LeadZero(Convert.ToString(c[i], 16), 8);
            }

            for (int i = str.Length - 16; i > -1; i -= 16)
            {
                C[C.Length - (i / 16) - 2] = (UInt64.Parse(str.Substring(i, 16), System.Globalization.NumberStyles.HexNumber));
            }

            str = "";
            for (int i = c2.Length - 1; i >= 0; i--)
            {
                str += LeadZero(Convert.ToString(c2[i], 16), 8);
            }
            for (int i = str.Length - 16; i > -1; i -= 16)
            {
                C2[C2.Length - (i / 16) - 2] = (UInt64.Parse(str.Substring(i, 16), System.Globalization.NumberStyles.HexNumber));
            }

            buf = LongAdd(C, C2);


            for (int i = 0; i < buf.Length; i++)
            {
                Res[i] = buf[i];
            }

            return Res;

        }

        private UInt64[] LongMul(UInt64[] A, UInt64[] B)
        {
            string a = "";
            string b = "";
            int lenght = A.Length * 2;
            UInt64[] temp = new UInt64[lenght];
            UInt64[] C = new UInt64[lenght];
            UInt32[] A32 = new UInt32[lenght];
            UInt32[] B32 = new UInt32[lenght];

            for (int i = A.Length - 1; i >= 0; i--)
            {
                a += LeadZero(Convert.ToString((long)A[i], 16), 16);
            }

            for (int i = B.Length - 1; i >= 0; i--)
            {
                b += LeadZero(Convert.ToString((long)B[i], 16), 16);
            }

            for (int i = 0; i < a.Length; i += 8)
            {
                A32[A32.Length - (i / 8) - 1] = (UInt32.Parse(a.Substring(i, 8), System.Globalization.NumberStyles.HexNumber));
            }

            for (int i = 0; i < b.Length; i += 8)
            {
                B32[B32.Length - (i / 8) - 1] = (UInt32.Parse(b.Substring(i, 8), System.Globalization.NumberStyles.HexNumber));
            }



            for (int i = 0; i < lenght; i += 2)
            {
                temp = LongMulOneDigit(A32, B32[i], B32[i + 1]);

                temp = LongShiftDigitsToHighMul(temp, i / 2);


                C = LongAdd(C, temp);

            }

            return C;

        }

        private (UInt64[],UInt64[]) LongDivMod(UInt64[] A, UInt64[] B)
        {
            int k = BitLengthV2(B);
            int t = 0;
            int nb = 0;
            int iter = 0;
            string Qq = " ";
            Qq = LeadZero(Qq, 4 * 64);
            UInt64[] R = A;
            UInt64[] Q = new UInt64[5];
            UInt64[] C = new UInt64[A.Length];
            while (LongCmp(R, B) == 0 || LongCmp(R, B) == 1)
            {
                t = BitLengthV2(R);
                C = LongShiftDigitsToHighDiv(B, t - k);

                while (LongCmp(R, C) == -1)
                {
                    if (LongCmp(R, C) == -1)
                    {
                        t--;
                        C = LongShiftDigitsToHighDiv(B, t - k);
                    }
                }
                
                R = LongSub(R, C);
                Q = LongAdd(Q, LongShiftDigitToHigh(2, t - k - 1));
                
            }
            
            return (Q,R);
        }

        private int LongCmp(UInt64[] A, UInt64[] B)
        {
            int minLength = new int[2] { A.Length, B.Length }.Min();

            if (A.Length != B.Length)
            {
                if (A.Length > B.Length)
                {
                    minLength = B.Length;
                    for (int i = A.Length - 1; i > minLength - 1; i--)
                    {
                        if (A[i] != 0)
                            return 1;
                    }
                }
                else
                {
                    minLength = A.Length;
                    for (int i = B.Length - 1; i > minLength - 1; i--)
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
        private int BitLengthV2(UInt64[] Bl)
        {
            int k = 0;
            for (int i = 0; i < Bl.Length; i++)
            {
                k += Convert.ToString((long)Bl[i], 2).Length;
            }
            return k;
        }

        private int BitLength(UInt64[] Bl)
        {
            int k = 0;

            for (int i = 0; i < Bl.Length; i++)
            {
                k += Convert.ToInt32(Math.Ceiling(Math.Log2(Bl[i] + 1)));
            }

            return k;
        }

        private UInt64[] LongShiftDigitsToHighDiv(UInt64[] r, int i)
        {
            List<UInt64> L = new List<UInt64>();

            for (int j = 0; j < r.Length; j++)
            {
                L.Add(0);
                L[j] = r[j];
            }

            if (i <= 0)
            {
                while (L.Count < 2 * r.Length)
                {
                    L.Add(0);
                }

                return L.ToArray();
            }
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
            }

            while (L.Count < 2 * r.Length)
            {
                L.Add(0);
            }

            return L.ToArray();
        }
        private UInt64[] LongShiftDigitsToHighMul(UInt64[] L, int i)
        {

            if (i == 0)
                return L;
            for (int k = 0; k < i; k++)
            {
                for (int j = L.Length - 1; j >= 1; j--)
                {
                    L[j] = L[j - 1];
                    L[j - 1] = 0;
                }
            }

            return L;
        }

        private UInt64[] LongShiftDigitToHigh(int n, int k)
        {
            if (k==-1)
            {
                return new UInt64[5] { 1, 0, 0, 0, 0};
            }
            string number = "";
            number += Convert.ToString(n, 2);
            for (int i = 0; i < k; i++)
            {
                number = number.Insert(number.Length, "0");
            }
            number = LeadZero(number, 320);

            UInt64[] Digit = new UInt64[5];
            
            int l = Digit.Length - 1;
            for (int i = 0; i <= number.Length - 64; i += 64)
            {

                Digit[l] = Convert.ToUInt64(number.Substring(i, 64), 2);
                l--;
            }

            return Digit;

        }


        private bool IsCarryExist(UInt64 A, UInt64 B, UInt64 carry)
        {
            string a = "";
            string b = "";
            a = LeadZero(Convert.ToString((long)A, 2),64);
            b = LeadZero(Convert.ToString((long)B, 2),64);

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

        private string LeadZero(string bit,int nBit)
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

        private string Write(UInt64[] W)
        {
            string output = "";
            for (int i = W.Length - 1; i >= 0; i--)
            {
             
                    output += LeadZero(W[i].ToString("X"), 16);
                
            }

            return DelLeadZero(output);
        }

        private string DelLeadZero(string str)
        {
            while (str[0]=='0')
            {
                str = str.Substring(1);
            }
            return str;
        }

        
        private bool IsBinary(char ch)
        {
            if (ch == '1' || ch == '0')
                return true;
            else
            {
                return false;
            }

        }
        public UInt64[] ReadNumber(string n)
        {
            n = n.Trim().ToLowerInvariant();

            int sys = 0;
            int m = 0;
            int lenght;
            long res;
            if (n.Length > 2)
            {
                if (n.Substring(0, 2) == "0x" && n.Substring(2).All(ch => Uri.IsHexDigit(ch)))
                {
                    sys = 16;
                    m = 16;
                }
                else if (n.Substring(0, 2) == "0b" && n.Substring(2).All(ch => IsBinary(ch)))
                {
                    sys = 64;
                    m = 2;
                }
                else
                {
                    Console.WriteLine("Not bin or hex");
                }
            }
            n = n.Substring(2);
            n = LeadZero(n, sys);
            lenght = n.Length / sys;
            UInt64[] Digit = new UInt64[lenght];
            int k = Digit.Length - 1;
            for (int i = 0; i <= n.Length - sys; i += sys)
            {
                Digit[k] = Convert.ToUInt64(n.Substring(i, sys), m);
            }


            return Digit;
        }

    }
        
    
}
