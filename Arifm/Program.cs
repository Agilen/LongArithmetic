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
                BigIntegersV2 a = new BigIntegersV2("0x40D4ED6B22B4A26625AFFF98B70342C0742C4EE21087230415DF1B9348B28C94");
                BigIntegersV2 b = new BigIntegersV2("0x1A98996C6EFBC1BC3C230BE9272861A04689D8D76C4F361DCD35972D469197B4");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                for (int i = 0; i < 1000; i++)
                {
                    BigIntegersV2 c = a + b;
                }
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine("Average Add " + ts.TotalMilliseconds/1000);
            }

            {
                BigIntegersV2 a = new BigIntegersV2("0x3AF01A7357B25888DD937053E63DF5BC8562ED86D24295AC8C491BF41E428869");
                BigIntegersV2 b = new BigIntegersV2("0xFC31AC9F4BB19608207B449B6F318CE53ECEEEA214C2981971036F45F587932");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                for (int i = 0; i < 1000; i++)
                {
                    BigIntegersV2 c = a - b;
                }
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine("Average Sub " + ts.TotalMilliseconds / 1000);
            }

            {
                BigIntegersV2 a = new BigIntegersV2("0x1FA5C57629704BEC9142567B9ECFD3FADF4029E8171C39AB6A7F4BB5551D7AC1");
                BigIntegersV2 b = new BigIntegersV2("0x75F4A39FEAE14D872A5E8374B27CB3FADE464D35AAE3D9B285478AE0563EEE6A");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                for (int i = 0; i < 1000; i++)
                {
                    BigIntegersV2 c = a - b;
                }
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine("Average Mul " + ts.TotalMilliseconds / 1000);
            }

            {
                BigIntegersV2 a = new BigIntegersV2("0x6FC747E8A92E7ADD219DA48AF56A378B7D484FF9E2CEC81C24970D982CD381EE3CEC65072296645350319B24752497AF4B06B81284F25927C3DC71EED5345CE7");
                BigIntegersV2 b = new BigIntegersV2("0x181F440F6C8BF3FBBA82755EDA369685FEF7226AD6BDC38D3646E61D86084768");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                for (int i = 0; i < 1000; i++)
                {
                    BigIntegersV2 c = a / b;
                }
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine("Average DivMod " + ts.TotalMilliseconds / 1000);
            }

            {
                BigIntegersV2 a = new BigIntegersV2("0xFC31AC9F4BB19608207B449B6F318CE53ECEEEA214C2981971036F45F587932");
                BigIntegersV2 b = new BigIntegersV2(100);
                BigIntegersV2 n = new BigIntegersV2("0x3AF01A7357B25888DD937053E63DF5BC8562ED86D24295AC8C491BF41E428869");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                for (int i = 0; i < 1000; i++)
                {
                    BigIntegersV2 c = n.LongModPowerBarrett(a,b,n);
                    Console.WriteLine(c.Write("16"));
                    
                }
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine("Average Pow " + ts.TotalMilliseconds / 1000);
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
                string data = "1";
                for(int i = 0; i < 255; i++)
                {
                    data += ran.Next(0, 2).ToString();
                }
                return data;
            }

            string CreatMod()
            {
                Random ran = new Random();
                string data = "1";
                for(int i = 0; i < 255; i++)
                {
                    data += ran.Next(0, 2).ToString();
                }
                
                return data;
            }
        }




    }
}
