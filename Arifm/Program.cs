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
            BigIntegersV2 a = new BigIntegersV2("0x50CF6A20998EEA455FCA21F671EC7A88A90FB5FBDC31007E4D9A8E6DBB3E69C1");
            BigIntegersV2 b = new BigIntegersV2("0x3D9FD9AA1AA22EB3CB02C7EDD3CDDF23C240C322CD758FB3BBCBE76E17E9DB8B");
            BigIntegersV2 Add = a + b;
            BigIntegersV2 Sub = a - b;
            Console.WriteLine(a.Write(a));
        }
    }
}
