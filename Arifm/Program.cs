using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Arifm
{
    class Program
    {

        public class Test
        {
            //[Index(0)]
            public string A { get; set; }
            //[Index(1)]
            public string B { get; set; }

            public string C { get; set; }

        }

        
        static void Main(string[] args)
        {
            string testAddPath = "Data/test_add.arith";
            string testSubPath = "Data/test_sub.arith";
            string testMulPath = "Data/test_mul.arith";
            List<string> A = new List<string>();
            List<string> B = new List<string>();
            List<string> C = new List<string>();

            Read(testAddPath);
            Console.WriteLine("---Long Add---");
            for (int i = 0; i < A.Count; i++)
            {
                LongArithmetic LA = new LongArithmetic(A[i], B[i],C[i],"+");
                
            }

             Read(testSubPath);
            Console.WriteLine("---Long Sub---");
            for(int i = 0; i < A.Count; i++)
            {
                LongArithmetic LA = new LongArithmetic(A[i], B[i], C[i],"-");
            }

            Console.ReadLine();

            
            //Read(testMulPath);
            //Console.WriteLine("---Long Mul---");
            //for(int i = 0; i < A.Count; i++)
            //{
            //    LongArithmetic LA = new LongArithmetic(A[i], B[i], C[i], "*");
            //}
            

            void Read(string path)
            {
                A.Clear();
                B.Clear();
                C.Clear();
                

                var lines = File.ReadAllLines(path);
               

                for(int i = 0; i < lines.Length; i+=4)
                {
                    A.Add(lines[i]);
                    B.Add(lines[i + 1]);
                    C.Add(lines[i + 2]);
                }
            }

        }
        
        static UInt32[] LongAdd( UInt32[] A, UInt32[] B )
        {
            UInt64 temp;
            UInt64 carry = 0;
            int lenght;

            if (A.Length > B.Length)
                lenght = A.Length;
            else
                lenght = B.Length;
            
            UInt32[] C = new UInt32[lenght];

            for (int i = 0; i < A.Length; i++)
            {
                temp = Convert.ToUInt64(A[i]) + Convert.ToUInt64(B[i]) + carry;

                //Console.WriteLine($"A: {A[i]} B: {B[i]} Carry:{carry}");
                //Console.WriteLine($"Temp:{temp}");

                C[i] = Convert.ToUInt32(temp & 0xffffffff );
                Console.WriteLine(Convert.ToString(C[i],16));


                //Console.WriteLine($"C[{i}]:{C[i]}");

                carry = temp >> 32;

                //Console.WriteLine($"Carry:{carry}");
            }
            return C;
        }

      

        static UInt32[] LongSub( UInt32[] A, UInt32[] B)
        {
            long temp;
            UInt32 borrow = 0;
            UInt32[] C=new UInt32 [4];

            for (int i = 0; i < C.Length; i++)
            {
                temp = (A[i] + (B[i] * -1) - borrow);
                if (temp >= 0)
                {
                    C[i] = (uint)temp;
                    borrow = 0;
                }
                else
                {
                    C[i] = Convert.ToUInt32(0xffffffff + temp);
                    borrow = 1;
                }
            }

            return C;
        }

        static int LongCmp( UInt32[] A, UInt32[] B )
        {
            int i = A.Length-1;
            
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
                    return -1;
            } while (A[i] == B[i]);

            return 2;
        }

        static UInt32[] LongMulOneDigit(UInt32[] A,UInt32[] C, UInt32 b)
        {
            UInt64 carry = 0;
            UInt64 temp;
            
            for(int i=0; i< A.Length-1; i++)
            {
                temp = A[i] * Convert.ToUInt64(b) + carry;
                C[i] = Convert.ToUInt32(temp & 0xffffffff);
                carry = temp >> 32;
                
            }
            C[A.Length-1] = Convert.ToUInt32(carry);
            
            return C;

        }

        static UInt32[] LongMul(UInt32[] A,UInt32[] B,UInt32[] C)
        {
            UInt32[] temp = new UInt32[A.Length + B.Length];

            for(int i = 0; i < B.Length; i++)
            {
                CleanTemp(temp);
                temp = LongMulOneDigit(A, temp, B[i]);
                temp = LongShiftDigitsToHigh(temp,i);
                C = LongAdd(C,temp);
            }
            return C;
        }

        static (UInt32[],UInt32[]) LongDivMod(UInt32[] A,UInt32[] B)
        {
            UInt32[] Q=new UInt32[4];
            UInt32[] R=new UInt32[4];
            UInt32[] C = new UInt32[4];
            int k = BitLenght(B);
            int t = 0;
            R = A;
            do
            {
                t = BitLenght(R);
                C = LongShiftDigitsToHigh(B, t - k);
                if(LongCmp(R,C) == -1)
                {
                    t--;
                    C = LongShiftDigitsToHigh(B, t - k);
                }
                R = LongSub(R, C);
                Q[t - k] = Q[t + k] + Convert.ToUInt32(Math.Pow(2, t - k));
            } while (LongCmp(R, B) == 0 || LongCmp(R, B) == 1);
            return (Q, R);
        }
        static UInt32[] LongShiftDigitsToHigh(UInt32[] temp,int i)
        {
            if (i != 0)
            {
                for (int j = temp.Length / 2 - 1; j >= 0; j--)
                {
                    temp[j + i] = temp[j];
                    temp[j] = 0;
                }
            }
            return temp;
        }

        static void CleanTemp(UInt32[] temp)
        {
            for(int i = 0; i < temp.Length; i++)
            {
                temp[i] = 0;
            }
        }

        static void ConsoleWrite(UInt32[] Output)
        {
            for(int i = Output.Length - 1; i >= 0; i--)
            {
                Console.Write(Convert.ToString(Output[i], 16));
            }
        }

        static int BitLenght(UInt32[] M)
        {
            int lenght = 0;
            for(int i = 0; i < M.Length; i++)
            {
                lenght+=Convert.ToString(M[i]).Length;
            }
            return lenght;
        }
    }
}
