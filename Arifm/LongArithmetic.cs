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

        

        public LongArithmetic(string a,string b,string c,string oper)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            FillList();
            if (oper == "+")
            {
                C = LongAdd(A, B);
            }
            else if(oper == "-")
            {
                C = LongSub(A, B);
            }
            else
            {
                C = LongMul(A,B);
            }
            if (Write(C) == this.c)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine((Write(C)));
            Console.WriteLine(this.c);
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

            for(int i=0; i < lengthA - mod; i += 16)
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
            int lenght = A.Count;
            
            for(int i = 0; i < lenght; i++)
            {
                temp = A[i] + B[i] + carry;
                C.Add(temp & 0xffffffffffffffff);
                if (IsCarryExist(A[i], B[i], carry) == true)
                {
                    carry = 1;
                }
                else
                    carry = 0;
                
            }
            

            return C;
        }

        private List<UInt64> LongSub(List<UInt64> A,List<UInt64> B)
        {
            List<UInt64> C = new List<UInt64>();
            UInt64 borrow = 0;
            int lenght = A.Count;
            for (int i = 0; i < lenght; i++)
            {
                C.Add(A[i] - B[i] - borrow);
                if(B[i]!=0 && (B[i]+borrow)==0)
                {
                    borrow = 1;
                }
                else if (A[i] >= (B[i]+borrow))
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

        private List<UInt64> LongMulOneDigit(List<UInt32> A,UInt32 B)
        {

            List<UInt64> C64 = new List<UInt64>();
            List<UInt32> C = new List<UInt32>();

            UInt64 temp;
            UInt64 carry = 0;

            string a = "";
            string b = "";

            for(int i = 0; i < A.Count; i++)
            {
                temp = Convert.ToUInt64(A[i]) * Convert.ToUInt64(B) + carry;
                C.Add((uint)(temp & 0xffffffff));
                carry = temp >> 32;
            }
            C.Add(Convert.ToUInt32(carry));
            C.Add(0);

            for(int i = 0; i < C.Count; i++)
            {
                a +=LeadZero8(Convert.ToString(C[i], 16),i);
            }

            for (int i = 0; i < a.Length; i += 16)
            {
                C64.Add(UInt64.Parse(a.Substring(i, 16), System.Globalization.NumberStyles.HexNumber));
            }
                        
            return C64;
            
        }

        private List<UInt64> LongMul(List<UInt64> A, List<UInt64> B)
        {
            string a = "";
            string b = "";
            int lenght = A.Count*2;
            List<UInt64> temp = new List<UInt64>();
            List<UInt64> C = new List<UInt64>();
            List<UInt32> A32 = new List<UInt32>();
            List<UInt32> B32 = new List<UInt32>();

            for(int i = 0; i < A.Count; i++)
            {
                a +=LeadZero(Convert.ToString((long)A[i], 16),1);
                b +=LeadZero(Convert.ToString((long)B[i], 16),1);
            }
           

            for (int i = 0; i < a.Length; i += 8)
            {
                A32.Add(UInt32.Parse(a.Substring(i, 8), System.Globalization.NumberStyles.HexNumber));
            }
        

            for (int i = 0; i < b.Length; i += 8)
            {
                B32.Add(UInt32.Parse(b.Substring(i, 8), System.Globalization.NumberStyles.HexNumber));
            }
           

            temp = PrepList(temp, A32.Count + 1 );
            C = PrepList(C, A32.Count );

            for (int i = 0; i < lenght; i++)
            {
                CleanTemp(temp);
                temp = LongMulOneDigit(A32, B32[i]);
                temp = PrepList(temp, A32.Count + 1);
                temp = LongShiftDigitsToHighMul(temp, i);
                C = LongAdd(C, temp);
            }

            return C;

        }

        private (List<UInt32>,List<UInt32>) LongDivMod(List<UInt32> A,List<UInt32> B)
        {
            int k, t;
            List<UInt32> R = new List<UInt32>();
            List<UInt32> Q = new List<UInt32>();
            List<UInt32> C = new List<UInt32>();
           // PrepList(Q, A.Count);
            R = A;
            k = BitLenght(B);

            do
            {
                t = BitLenght(R);
                C = LongShiftDigitsToHighDiv(B, t - k);
                if (LongCmp(R, C) == -1)
                {
                    t--;
                    C = LongShiftDigitsToHighDiv(B, t - k);
                }
                //R = LongSub(R, C);

            } while (LongCmp(R,B)==0 || LongCmp(R,B)==1);

            return (Q, R);
        }

        private int LongCmp(List<UInt32> A,List<UInt32> B)
        {
            int i = A.Count - 1;
            do
            {
                i--;
                if (i == -1)
                {
                    return 0;
                }
                else if (A[i] > B[i])
                {
                    return 1;    
                }
                else
                {
                    return -1;
                }
            } while (A[i]==B[i]);
        }

        private List<UInt64> CleanTemp(List<UInt64> temp)
        {
            for(int i = 0; i < temp.Count; i++)
            {
                temp[i] = 0;
            }
            return temp;
        }
        
        private List<UInt64> LongShiftDigitsToHighMul(List<UInt64> L,int i)
        {
            
            if(i!=0 || i == L.Count/2)
            {
                for (int j = L.Count/2 - 1; j >= 0; j--)
                {
                    L[j + i] = L[j];
                    L[j] = 0;
                }
            }
            return L;
        }

        private List<UInt32> LongShiftDigitsToHighDiv(List<UInt32> L,int i)
        {

            return L;
        }

        private List<UInt64> PrepList(List<UInt64> L,int n)
        {
            for(int i = L.Count; i < n+1; i++)
            {
                L.Add(0);
            }
            return L;
        }

        private int BitLenght(List<UInt32> L)
        {
            int l = 0;
            for(int i = 0; i < L.Count; i++)
            {
                l += Convert.ToString(L[i],2).Length;
            }
            return l;
        }

        private string Write(List<UInt64> W)
        {
            string output = "";
            for(int i = W.Count - 1; i >= 0; i--)
            {
                //output+= Convert.ToString(W[i], 16);
               output+=LeadZero(W[i].ToString("X"),i);
            }
            if (output[0] == '0')
            {
                output = output.Substring(1);
            }
            return output;
        }

        private bool IsCarryExist(UInt64 A, UInt64 B, UInt64 carry)
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

        private string LeadZero8(string bits, int i)
        {
            if (i != C.Count - 1)
            {
                if (bits.Length < 8)
                {
                    while (bits.Length < 8)
                    {
                        bits = bits.Insert(0, "0");
                    }
                }
            }

            return bits;
        }
        private string LeadZero(string bits,int i)
        {
            if (i != C.Count-1)
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

       
    }
}
