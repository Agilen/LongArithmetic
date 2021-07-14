using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Arifm
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                int k = 0;
                for (int i = 1; i < 100; i++)
                {
                    BigIntegersV2 a = new BigIntegersV2($"0b{CreatData()}");
                    BigIntegersV2 b = new BigIntegersV2(i);
                    BigIntegersV2 n = new BigIntegersV2($"0b{CreatMod()}");
                    BigIntegersV2 res = new BigIntegersV2();
                    res = res.LongModPowerMontgomery(a, b, n);

                    Console.WriteLine(a.Write("16"));
                    Console.WriteLine(b.Write("16"));
                    Console.WriteLine(n.Write("16"));
                    if (res == res.LongModPowerBarrett(a, b, n))
                    {
                        Console.WriteLine(true);
                        k++;
                    }
                    else
                        Console.WriteLine(false);
                    Console.WriteLine(res.Write("16"));
                    Console.WriteLine();

                }
          
            }

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

             
                BigIntegersV2 a = new BigIntegersV2($"0x{A[i]}");
                BigIntegersV2 b = new BigIntegersV2($"0x{B[i]}");

                BigIntegersV2 c = a - b;

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
                

                BigIntegersV2 a = new BigIntegersV2($"0x{A[i]}");
                BigIntegersV2 b = new BigIntegersV2($"0x{B[i]}");


                BigIntegersV2 c = a * b;
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
              
                BigIntegersV2 c = a / b;
                BigIntegersV2 cMod = a % b;
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

            string CreatData()
            {
                Random ran = new Random();
                string data = "0";
                for(int i = 0; i < 127; i++)
                {
                    data += ran.Next(0, 2).ToString();
                }
                return data;
            }

            string CreatMod()
            {
                Random ran = new Random();
                string data = "1";
                for(int i = 0; i < 127; i++)
                {
                    data += ran.Next(0, 2).ToString();
                }
                return data;
            }
        }




    }
}
