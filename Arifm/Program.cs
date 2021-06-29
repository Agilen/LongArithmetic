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
        static void Main(string[] args)
        {

            List<string> A = new List<string>();
            List<string> B = new List<string>();
            List<string> C = new List<string>();

            string DataAdd = "Data/test_add.arith";
            string DataSub = "Data/test_sub.arith";
            string DataMul = "Data/test_mul.arith";
            string DataDiv = "Data/test_div.arith";

            Console.WriteLine("---Long Add Test---");
            ReadData(DataAdd);
            for (int i = 0; i < C.Count; i++)
            {
                BigIntegersV2 a = new BigIntegersV2($"0x{A[i]}");
                BigIntegersV2 b = new BigIntegersV2($"0x{B[i]}");
                BigIntegersV2 c = a + b;
                Console.WriteLine(c.Write("16"));
                Console.WriteLine(C[i]);
            }

            Console.WriteLine("---Long Sub Test---");
            ReadData(DataSub);
            for (int i = 0; i < C.Count; i++)
            {
                BigIntegersV2 a = new BigIntegersV2($"0x{A[i]}");
                BigIntegersV2 b = new BigIntegersV2($"0x{B[i]}");
                BigIntegersV2 c = a - b;
                Console.WriteLine(c.Write("16"));
                Console.WriteLine(C[i]);
            }

            Console.WriteLine("---Long Mul Test---");
            ReadData(DataMul);
            for (int i = 0; i < C.Count; i++)
            {
                BigIntegersV2 a = new BigIntegersV2($"0x{A[i]}");
                BigIntegersV2 b = new BigIntegersV2($"0x{B[i]}");
                BigIntegersV2 c = a * b;
                Console.WriteLine(c.Write("16"));
                Console.WriteLine(C[i]);
            }

            Console.WriteLine("---Long Div Test---");
            ReadData(DataDiv);
            for (int i = 0; i < C.Count; i++)
            {
                BigIntegersV2 a = new BigIntegersV2($"0x{A[i]}");
                BigIntegersV2 b = new BigIntegersV2($"0x{B[i]}");
                BigIntegersV2 c = a / b;
                BigIntegersV2 cMod = a % b;
                Console.WriteLine(c.Write("16") + " " + cMod.Write("16"));
                Console.WriteLine(C[i]);
            }



            void ReadData(string Data)
            {
                A.Clear();
                B.Clear();
                C.Clear();

                var lines = File.ReadAllLines(Data);

                for (int i = 0; i < lines.Length; i += 4)
                {
                    A.Add(lines[i]);
                    B.Add(lines[i + 1]);
                    C.Add(lines[i + 2]);
                }
            }
        }




    }
}
