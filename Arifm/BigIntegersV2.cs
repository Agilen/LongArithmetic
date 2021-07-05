using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arifm
{
    public class BigIntegersV2
    {
        private UInt64[] Digit;

        public BigIntegersV2(int n)
        {
            Digit = new UInt64[1] { Convert.ToUInt64(n) };
        }
        public BigIntegersV2(UInt64 n)
        {
            Digit = new UInt64[1] { n };
        }
        public BigIntegersV2()
        {
            Digit = new UInt64[1] { 0 };
        }
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
        public static bool operator <(BigIntegersV2 a, BigIntegersV2 b) => LongCmp(a, b) < 0;
        public static bool operator >(BigIntegersV2 a, BigIntegersV2 b) => LongCmp(a, b) > 0;
        public static bool operator <=(BigIntegersV2 a, BigIntegersV2 b) => LongCmp(a, b) <= 0;
        public static bool operator >=(BigIntegersV2 a, BigIntegersV2 b) => LongCmp(a, b) >= 0;
        public static bool operator ==(BigIntegersV2 a, BigIntegersV2 b) => LongCmp(a, b) == 0;
        public static bool operator !=(BigIntegersV2 a, BigIntegersV2 b) => LongCmp(a, b) != 0;
        public override bool Equals(object obj) => !(obj is BigIntegersV2) ? false : this == (BigIntegersV2)obj;

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
            UInt32[] B32 = new UInt32[b.Size*2];

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



            for (int i = 0; i < B32.Length; i += 2)
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
            int k = BitLengthV2(b);
            int t = 0;
            BigIntegersV2 R = a;
            BigIntegersV2 C ;
            while (R>=b)
            {
                t = BitLengthV2(R);
                C = LongShiftDigitsToHighDiv(b, t - k);
                while (R < C)
                {
                    if (R < C)
                    {
                        t--;
                        C = LongShiftDigitsToHighDiv(b, t - k);
                    }
                }
                R = R - C;
            }

            return R;
        }
        private static BigIntegersV2 LongDiv(BigIntegersV2 a, BigIntegersV2 b)
        {
            
            int maxLength = Math.Max(a.Size, b.Size);
            int k = BitLengthV2(b);
            int t = 0;
            BigIntegersV2 R = a;
            BigIntegersV2 Q = new BigIntegersV2(new UInt64[b.Size+1]);
            BigIntegersV2 C ;
            while (R >= b)
            {
                t = BitLengthV2(R);
                C = LongShiftDigitsToHighDiv(b, t - k);

                while (R < C)
                {
                    if (R < C)
                    {
                        t--;
                        C = LongShiftDigitsToHighDiv(b, t - k);
                    }
                }

                R = R - C;
                Q = Q + LongShiftDigitToHigh(2, t - k - 1);
            

            }
           
            return Q;
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
        public BigIntegersV2 Pow(int n,int m)
        {
            BigIntegersV2 a = new BigIntegersV2(n);
            BigIntegersV2 res = new BigIntegersV2(new UInt64[2] {Convert.ToUInt64(n),0 });
            for(int i = 0; i < m-1; i++)
            {
                res = res * a;
                res = DelNull(res);

            }

            return res;
        }

        private UInt64[] ReadDec(string n)
        {
            n = n.Trim();
            UInt64[] numbers = new UInt64[n.Length];
            for(int i = 0; i < n.Length; i++)
            {
                numbers[n.Length - 1 - i] = UInt64.Parse(n.Substring(i, 1));
            }
            BigIntegersV2 res=new BigIntegersV2();
            for(int i = 0; i < numbers.Length; i++)
            {
                res = res + Pow(10, i) * new BigIntegersV2(numbers[i]);
            }
            res = DelNull(res);
            numbers = new UInt64[res.Size];
            for(int i = 0; i < res.Size; i++)
            {
                numbers[i] = res.GetValue(i);
            }
            return numbers;
        }

        private BigIntegersV2 DelNull(BigIntegersV2 a)
        {
            int count = a.Size;
            if (a.Size != 0)
            {
                for (int i = a.Size- 1; i >= 0; i--)
                {
                    if (a.GetValue(i) == 0)
                        count--;
                    else
                        break;

                }
                count++;

                UInt64[] d = new UInt64[count];
                for (int i = 0; i < d.Length; i++)
                {
                    d[i] = a.GetValue(i);
                }
                return new BigIntegersV2(d);
            }
            return a;
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
        private static BigIntegersV2 LongShiftDigitsToHighDiv(BigIntegersV2 r, int i)
        {
            List<UInt64> L = new List<UInt64>();

            for (int j = 0; j < r.Size; j++)
            {
                L.Add(0);
                L[j] = r.GetValue(j);
            }

            if (i <= 0)
            {
                return new BigIntegersV2(L.ToArray());
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

            //while (L.Count < 2 * r.Size)
            //{
            //    L.Add(0);
            //}

            return new BigIntegersV2(L.ToArray());
        }
        private static BigIntegersV2 LongShiftDigitToHigh(int n, int k)
        {
            if (k <= -1)
            {
                return new BigIntegersV2(new UInt64[1] { 1 });
            }
            if (k == 0)
            {
                return new BigIntegersV2(new UInt64[1] { Convert.ToUInt64(n) });
            }
            string number = "";
            number += Convert.ToString(n, 2);
            for (int i = 0; i < k; i++)
            {
                number = number.Insert(number.Length, "0");
            }
            number = LeadZero(number, 64);

            UInt64[] D = new UInt64[number.Length/64];

            int l = D.Length - 1;
            for (int i = 0; i <= number.Length - 64; i += 64)
            {

                D[l] = Convert.ToUInt64(number.Substring(i, 64), 2);
                l--;
            }

            return new BigIntegersV2(D);

        }
        private static int BitLengthV2(BigIntegersV2 Bl)
        {
            int k = 0;
            for (int i = 0; i < Bl.Size; i++)
            {
                k += Convert.ToString((long)Bl.GetValue(i), 2).Length;
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
                else if (n.All(ch => IsDec(ch)))
                {
                    return ReadDec(n);
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
      
        private bool IsDec(char ch)
        {
            if (ch == '0' || ch == '1' || ch == '2' || ch == '3' || ch == '4' || ch == '5' || ch == '6' || ch == '7' || ch == '8' || ch == '9')
                return true;
            else
                return false;
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
        private string DecOutput()
        {
            BigIntegersV2 LeftPart=new BigIntegersV2(Digit);
            BigIntegersV2 Mod;
            string Output = "";
            while (LeftPart > new BigIntegersV2(10))
            {
                Mod = LeftPart % new BigIntegersV2(10);
                Output = Output.Insert(0, Convert.ToString(Mod.GetValue(0)));
                LeftPart = LeftPart / new BigIntegersV2(10);
            }
            Output= Output.Insert(0, Convert.ToString(LeftPart.GetValue(0)));
            return Output;
        }
        public string Write(string n)
        {
            string output = "";
            if (n.Trim() == "16")
            {
                for (int i = Digit.Length - 1; i >= 0; i--)
                {

                    output += LeadZero(Digit[i].ToString("X"), 16);

                }
                
            }

            if (n.Trim() == "2")
            {
                for (int i = Digit.Length - 1; i >= 0; i--)
                {

                    output += LeadZero(Convert.ToString((long)Digit[i],2),64);

                }
            }
            if (n.Trim() == "10")
            {
                output = DecOutput();
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
