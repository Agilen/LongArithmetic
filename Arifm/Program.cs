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
            string testDivPath = "Data/test_div.arith";
            List<string> A = new List<string>();
            List<string> B = new List<string>();
            List<string> C = new List<string>();

            //Read(testAddPath);
            //Console.WriteLine("---Long Add---");
            //for (int i = 0; i < A.Count; i++)
            //{
            //    BigInteger LA = new BigInteger(A[i], B[i], C[i]);

            //}

            //Read(testSubPath);
            //Console.WriteLine("---Long Sub---");
            //for (int i = 0; i < A.Count; i++)
            //{
            //    BigInteger LA = new BigInteger(A[i], B[i], C[i]);
            //}

            //Read(testMulPath);
            //Console.WriteLine("---Long Mul---");
            //for (int i = 0; i < A.Count; i++)
            //{
            //    LongArithmetic LA = new LongArithmetic(A[i], B[i], C[i],"*");
            //}


            //Read(testMulPath);
            //Console.WriteLine("---Long Mul---");
            //for (int i = 0; i < A.Count; i++)
            //{
            //    BigInteger LA = new BigInteger(A[i], B[i], C[i]);
            //}


            Read(testDivPath);
            Console.WriteLine("---Long Div---");

            for (int i = 0; i < A.Count; i++)
            {
                BigInteger LA = new BigInteger(A[i], B[i], C[i]);
            }


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
        

    }
}
