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

            BigIntegersV2 bg = new BigIntegersV2("0x30A9896684869E8FA25194279F6624A19B1FCBB1C1AE5CFC3FF8CEC40C6306D");

            Console.WriteLine(bg.Write("10"));
            Console.WriteLine(bg.Write("16"));
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
                BigIntegersV2 aDec = new BigIntegersV2(a.Write("10"));
                BigIntegersV2 bDec = new BigIntegersV2(b.Write("10"));

                BigIntegersV2 c = aDec + bDec;
                BigIntegersV2 TestC = new BigIntegersV2($"0x{C[i]}");

                if (TestC == c)
                    Console.WriteLine(true);
                else
                {
                    Console.WriteLine(false);
                }
            }

            Console.WriteLine("---Long Sub Test---");
            ReadData(DataSub);
            for (int i = 0; i < 20; i++)
            {

                Console.WriteLine(i);
                BigIntegersV2 a = new BigIntegersV2($"0x{A[i]}");
                BigIntegersV2 b = new BigIntegersV2($"0x{B[i]}");
                BigIntegersV2 aDec = new BigIntegersV2(a.Write("10"));
                BigIntegersV2 bDec = new BigIntegersV2(b.Write("10"));
                BigIntegersV2 c = aDec - bDec;

                BigIntegersV2 TestC = new BigIntegersV2($"0x{C[i]}");
                if (TestC == c)
                    Console.WriteLine(true);
                else
                {
                    Console.WriteLine(false);
                }
            }


            Console.WriteLine("---Long Mul Test---");
            ReadData(DataMul);
            for (int i = 0; i < 20; i++)
            {
                Console.Write(i);
                Console.WriteLine();

                BigIntegersV2 a = new BigIntegersV2($"0x{A[i]}");
                BigIntegersV2 b = new BigIntegersV2($"0x{B[i]}");
                BigIntegersV2 aDec = new BigIntegersV2(a.Write("10"));
                BigIntegersV2 bDec = new BigIntegersV2(b.Write("10"));

                BigIntegersV2 c = aDec * bDec;
                BigIntegersV2 TestC = new BigIntegersV2($"0x{C[i]}");
                if (TestC == c)
                    Console.WriteLine(true);
                else
                {
                    Console.WriteLine(false);
                }
            }

            Console.WriteLine("---Long Div Test---");
            ReadData(DataDiv);
            string Ct;
            string CtMod;
            for (int i = 0; i < C.Count; i++)
            {
                BigIntegersV2 a = new BigIntegersV2($"0x{A[i]}");
                BigIntegersV2 b = new BigIntegersV2($"0x{B[i]}");
                BigIntegersV2 aDec = new BigIntegersV2(a.Write("10"));
                BigIntegersV2 bDec = new BigIntegersV2(b.Write("10"));
                BigIntegersV2 c = aDec/ bDec;
                BigIntegersV2 cMod = aDec % bDec;
                Ct = C[i].Substring(0,C[i].IndexOf(' '));
                CtMod = C[i].Substring(C[i].IndexOf(' ')+1);
                BigIntegersV2 CT = new BigIntegersV2($"0x{Ct}");
                BigIntegersV2 CTM = new BigIntegersV2($"0x{CtMod}");
                if (c == CT)
                    Console.WriteLine(true);
                else
                    Console.WriteLine(false);

                if (cMod == CTM)
                    Console.WriteLine(true);
                else
                    Console.WriteLine(false);
            }

            //void ToDec()
            //{
            //    ADec.Clear();
            //    BDec.Clear();
            //    CDec.Clear();
            //    BigIntegersV2 dec;
            //    for (int i = 0; i < A.Count; i++)
            //    {
            //        dec = new BigIntegersV2($"0x{A[i]}");
            //        ADec.Add(dec.Write("10"));
            //        dec = new BigIntegersV2($"0x{B[i]}");
            //        BDec.Add(dec.Write("10"));
            //        dec = new BigIntegersV2($"0x{C[i]}");
            //        CDec.Add(dec.Write("10"));
            //        Console.WriteLine(i + 1);
            //    }
            //}

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
