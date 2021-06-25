using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arifm
{
    public class BigIntegersV2
    {
        private UInt64[] Digit;

        public BigIntegersV2(UInt64[] digit)
        {
            Digit = digit;
        }
        
        public BigIntegersV2(string digit)
        {
            Digit = ReadNumber(digit);
        }

        public int Size => Digit.Length;
        public UInt64 GetValue(int i) => i < Size ? Digit[i] : (UInt64)0;
        public static BigIntegersV2 operator +(BigIntegersV2 a, BigIntegersV2 b) => LongAdd(a, b);
        public static BigIntegersV2 operator -(BigIntegersV2 a, BigIntegersV2 b) => LongSub(a, b);
        public static BigIntegersV2 operator *(BigIntegersV2 a, BigIntegersV2 b) => LongMul(a, b);
        public static BigIntegersV2 operator /(BigIntegersV2 a, BigIntegersV2 b) => LongDiv(a, b);
        public static BigIntegersV2 operator %(BigIntegersV2 a, BigIntegersV2 b) => LongDivMod(a, b);
        
        private static BigIntegersV2 LongSub(BigIntegersV2 a,BigIntegersV2 b)
        {
            int maxLength = Math.Max(a.Size, b.Size);
            UInt64[] C = new UInt64[maxLength];
            UInt64 borrow = 0;
            for (int i = 0; i < maxLength; i++)
            {
                C[i] = a.GetValue(i) - b.GetValue(i) - borrow;
                if (b.GetValue(i) != 0 && (b.GetValue(i) + borrow) == 0)
                {
                    borrow = 1;
                }
                else if (a.GetValue(i) >= (b.GetValue(i) + borrow))
                {
                    borrow = 0;
                }
                else
                {
                    borrow = 1;
                }

            }
            return new BigIntegersV2(C);
        }
        private static BigIntegersV2 LongAdd(BigIntegersV2 a,BigIntegersV2 b)
        {
            int maxSize = Math.Max(a.Size, b.Size);
            UInt64[] C = new UInt64[maxSize];

            UInt64 temp;
            UInt64 carry = 0;

            for (int i = 0; i < maxSize; i++)
            {
                temp = a.GetValue(i) + b.GetValue(i) + carry;
                C[i] = temp & 0xffffffffffffffff;
                if (IsCarryExist(a.GetValue(i), b.GetValue(i), carry) == true)
                    carry = 1;
                else
                    carry = 0;
            }

            return new BigIntegersV2(C);
        }
        private static BigIntegersV2 LongMul(BigIntegersV2 a, BigIntegersV2 b)
        {
            string aa = "";
            string bb = "";
            int maxLength = Math.Max(a.Size, b.Size);
            int lenght = maxLength * 2;

            UInt64[] temp = new UInt64[lenght];
            UInt64[] C = new UInt64[lenght];
            UInt32[] A32 = new UInt32[lenght];
            UInt32[] B32 = new UInt32[lenght];

            for (int i = a.Size - 1; i >= 0; i--)
            {
                aa += LeadZero(Convert.ToString((long)a.GetValue(i), 16), 16);
            }

            for (int i = b.Size - 1; i >= 0; i--)
            {
                bb += LeadZero(Convert.ToString((long)b.GetValue(i), 16), 16);
            }

            for (int i = 0; i < aa.Length; i += 8)
            {
                A32[A32.Length - (i / 8) - 1] = (UInt32.Parse(aa.Substring(i, 8), System.Globalization.NumberStyles.HexNumber));
            }

            for (int i = 0; i < bb.Length; i += 8)
            {
                B32[B32.Length - (i / 8) - 1] = (UInt32.Parse(bb.Substring(i, 8), System.Globalization.NumberStyles.HexNumber));
            }



            for (int i = 0; i < lenght; i += 2)
            {
                temp = LongMulOneDigit(A32, B32[i], B32[i + 1]);

                temp = LongShiftDigitsToHighMul(temp, i / 2);


                C = LongAdd(C, temp);

            }

            return new BigIntegersV2(C);

        }
        private static int LongCmp(BigIntegersV2 A, BigIntegersV2 B)
        {
            int maxLength = Math.Max(A.Size, B.Size);


            for (int i = maxLength - 1; i >= 0; i--)
            {
                if (A.GetValue(i) > B.GetValue(i))
                {
                    return 1;
                }
                else if (A.GetValue(i) < B.GetValue(i))
                {
                    return -1;
                }
            }

            return 0;
        }
        private static BigIntegersV2 LongDivMod(BigIntegersV2 a, BigIntegersV2 b)
        {
            int maxLength = Math.Max(a.Size, b.Size);
            UInt64[] A = new UInt64[maxLength];
            UInt64[] B = new UInt64[maxLength];
            for(int i = 0; i < maxLength; i++)
            {
                A[i] = a.GetValue(i);
                B[i] = b.GetValue(i);
            }
            int k = BitLengthV2(B);
            int t = 0;
            string Qq = " ";
            Qq = LeadZero(Qq, 4 * 64);
            UInt64[] R = A;
            UInt64[] Q = new UInt64[5];
            UInt64[] C = new UInt64[A.Length];
            while (LongCmp(new BigIntegersV2(R), new BigIntegersV2(B)) == 0 || LongCmp(new BigIntegersV2(R), new BigIntegersV2(B)) == 1)
            {
                t = BitLengthV2(R);
                C = LongShiftDigitsToHighDiv(B, t - k);

                while (LongCmp(new BigIntegersV2(R), new BigIntegersV2(C)) == -1)
                {
                    if (LongCmp(new BigIntegersV2(R), new BigIntegersV2(C)) == -1)
                    {
                        t--;
                        C = LongShiftDigitsToHighDiv(B, t - k);
                    }
                }

                R = LongSub(R,C);
                Q = LongAdd(Q, LongShiftDigitToHigh(2, t - k - 1));

            }

            return new BigIntegersV2(R);
        }
        private static BigIntegersV2 LongDiv(BigIntegersV2 a, BigIntegersV2 b)
        {
            int maxLength = Math.Max(a.Size, b.Size);
            UInt64[] A = new UInt64[maxLength];
            UInt64[] B = new UInt64[maxLength];
            for (int i = 0; i < maxLength; i++)
            {
                A[i] = a.GetValue(i);
                B[i] = b.GetValue(i);
            }
            int k = BitLengthV2(B);
            int t = 0;
            string Qq = " ";
            Qq = LeadZero(Qq, 4 * 64);
            UInt64[] R = A;
            UInt64[] Q = new UInt64[5];
            UInt64[] C = new UInt64[A.Length];
            while (LongCmp(new BigIntegersV2(R), new BigIntegersV2(B)) == 0 || LongCmp(new BigIntegersV2(R), new BigIntegersV2(B)) == 1)
            {
                t = BitLengthV2(R);
                C = LongShiftDigitsToHighDiv(B, t - k);

                while (LongCmp(new BigIntegersV2(R), new BigIntegersV2(C)) == -1)
                {
                    if (LongCmp(new BigIntegersV2(R), new BigIntegersV2(C)) == -1)
                    {
                        t--;
                        C = LongShiftDigitsToHighDiv(B, t - k);
                    }
                }

                R = LongSub(R, C);
                Q = LongAdd(Q, LongShiftDigitToHigh(2, t - k - 1));

            }

            return new BigIntegersV2(Q);
        }
        private static UInt64[] LongMulOneDigit(UInt32[] A, UInt32 B1, UInt32 B2)
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
        private static UInt64[] LongAdd(UInt64[] A, UInt64[] B)
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
        private static UInt64[] LongSub(UInt64[] A, UInt64[] B)
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
        private static UInt64[] LongShiftDigitsToHighMul(UInt64[] L, int i)
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
        private static UInt64[] LongShiftDigitsToHighDiv(UInt64[] r, int i)
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
        private static UInt64[] LongShiftDigitToHigh(int n, int k)
        {
            if (k == -1)
            {
                return new UInt64[5] { 1, 0, 0, 0, 0 };
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
        private static int BitLengthV2(UInt64[] Bl)
        {
            int k = 0;
            for (int i = 0; i < Bl.Length; i++)
            {
                k += Convert.ToString((long)Bl[i], 2).Length;
            }
            return k;
        }
        private static bool IsCarryExist(UInt64 A, UInt64 B, UInt64 carry)
        {
            string a = "";
            string b = "";
            a = LeadZero(Convert.ToString((long)A, 2), 64);
            b = LeadZero(Convert.ToString((long)B, 2), 64);

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
        private UInt64[] ReadNumber(string n)
        {
            n = n.Trim().ToLowerInvariant();
            int sys = 0;
            int m = 0;
            int lenght;
            
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
            n = LeadZero(n.Substring(2), sys);
            lenght = n.Length / sys;
            UInt64[] Digit = new UInt64[lenght];
            int k = 0;
            for (int i = n.Length - sys; i >=-1; i -= sys)
            {
                Digit[k] = Convert.ToUInt64(n.Substring(i, sys), m);
                k++;
            }

            return Digit;
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
        private static string LeadZero(string bit, int nBit)
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

        public string Write(BigIntegersV2 W)
        {
            string output = "";
            for (int i = W.Size - 1; i >= 0; i--)
            {

                output += LeadZero(W.GetValue(i).ToString("X"), 16);

            }

            return DelLeadZero(output);
        }

        private string DelLeadZero(string str)
        {
            while (str[0] == '0')
            {
                str = str.Substring(1);
            }
            return str;
        }
    }
}
