using OpcLabs.BaseLib.ComInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEPZET
{
    static class Constant
    {

        public static string ComChannel = "ZKWPBL2019";
        public static string VarTable = "2019H3";
        public static string ServerIP = "localhost";
        public static string ServerClass = "Kepware.KEPServerEX.V6";
        public static int Hala = 3;


        //public const string R4 = "Simulation.Register_R4";

        public static string input1RstN = ComChannel + "." + VarTable + "." + "input1RstN"; //v OPC: boolean
        public static string input2RstN = ComChannel + "." + VarTable + "." + "input2RstN"; //v OPC: boolean
        public static string input3RstN = ComChannel + "." + VarTable + "." + "input3RstN"; //v OPC: boolean
        public static string input4RstN = ComChannel + "." + VarTable + "." + "input4RstN"; //v OPC: boolean
        public static string input5RstN = ComChannel + "." + VarTable + "." + "input5RstN"; //v OPC: boolean
        public static string input6RstN = ComChannel + "." + VarTable + "." + "input6RstN"; //v OPC: boolean
              
        public static string output1RstN = ComChannel + "." + VarTable + "." + "output1RstN"; //v OPC: boolean
        public static string output2RstN = ComChannel + "." + VarTable + "." + "output2RstN"; //v OPC: boolean
        public static string output3RstN = ComChannel + "." + VarTable + "." + "output3RstN"; //v OPC: boolean
        public static string output4RstN = ComChannel + "." + VarTable + "." + "output4RstN"; //v OPC: boolean
        public static string output5RstN = ComChannel + "." + VarTable + "." + "output5RstN"; //v OPC: boolean
        public static string output6RstN = ComChannel + "." + VarTable + "." + "output6RstN"; //v OPC: boolean

        private static void Ahoj() {
            Console.WriteLine("Ahoj");
        }


        public static string inputXRstN(string X) { return ComChannel + "." + VarTable + "." + X + "RstN";} //v OPC: boolean
        public static string outputXRstN(string X) { return ComChannel + "." + VarTable + "." + X + "RstN"; } //v OPC: boolean


        //public const string output1Rst = ComChannel + "." + VarTable + "." + "output1RstN"; //v OPC: boolean
        //public const string output2Rst = ComChannel + "." + VarTable + "." + "output2RstN"; //v OPC: boolean

        public static int ih_input1Rst;
        public static int ih_input2Rst;
        public static int ih_input3Rst;
        public static int ih_input4Rst;
        public static int ih_input5Rst;
        public static int ih_input6Rst;

        public static int ih_output1Rst;
        public static int ih_output2Rst;
        public static int ih_output3Rst;
        public static int ih_output4Rst;
        public static int ih_output5Rst;
        public static int ih_output6Rst;

        public static int ih_showInput1;
        public static int ih_showInput2;
        public static int ih_showInput3;
        public static int ih_showInput4;
        public static int ih_showInput5;
        public static int ih_showInput6;

        public static string showInput1N = ComChannel + "." + VarTable + "." + "showInput1N";
        public static string showInput2N = ComChannel + "." + VarTable + "." + "showInput2N";
        public static string showInput3N = ComChannel + "." + VarTable + "." + "showInput3N";
        public static string showInput4N = ComChannel + "." + VarTable + "." + "showInput4N";
        public static string showInput5N = ComChannel + "." + VarTable + "." + "showInput5N";
        public static string showInput6N = ComChannel + "." + VarTable + "." + "showInput6N";
               
        public static string showOutput1 = ComChannel + "." + VarTable + "." + "showOutput1N"; //v OPC:  boolean
        public static string showOutput2 = ComChannel + "." + VarTable + "." + "showOutput2N"; //v OPC: boolean
        public static string showOutput3 = ComChannel + "." + VarTable + "." + "showOutput3N"; //v OPC: boolean
        public static string showOutput4 = ComChannel + "." + VarTable + "." + "showOutput4N"; //v OPC: boolean
        public static string showOutput5 = ComChannel + "." + VarTable + "." + "showOutput5N"; //v OPC: boolean
        public static string showOutput6 = ComChannel + "." + VarTable + "." + "showOutput6N"; //v OPC: boolean
               
        public static string input1N = ComChannel + "." + VarTable + "." + "Input1N"; //v OPC Word array
        public static string input2N = ComChannel + "." + VarTable + "." + "Input2N"; //v OPC Word array
        public static string input3N = ComChannel + "." + VarTable + "." + "Input3N"; //v OPC Word array
        public static string input4N = ComChannel + "." + VarTable + "." + "Input4N"; //v OPC Word array
        public static string input5N = ComChannel + "." + VarTable + "." + "Input5N"; //v OPC Word array
        public static string input6N = ComChannel + "." + VarTable + "." + "Input6N"; //v OPC Word array
               
        public static string output1N = ComChannel + "." + VarTable + "." + "Output1N"; //v OPC Word array
        public static string output2N = ComChannel + "." + VarTable + "." + "Output2N"; //v OPC Word array
        public static string output3N = ComChannel + "." + VarTable + "." + "Output3N"; //v OPC Word array
        public static string output4N = ComChannel + "." + VarTable + "." + "Output4N"; //v OPC Word array
        public static string output5N = ComChannel + "." + VarTable + "." + "Output5N"; //v OPC Word array
        public static string output6N = ComChannel + "." + VarTable + "." + "Output6N"; //v OPC Word array
        


        public const int requestedUpdateRate = 100;

        public static DateTime ZERO = new DateTime(0);

        /* porty serveru */
        public static int Port11 = 0;
        public static int Port12 = 0;
        public static int Port21 = 0;
        public static int Port22 = 0;

        /* porty citacky */
        public static int cPort11 = 0;
        public static int cPort12 = 0;
        public static int cPort21 = 0;
        public static int cPort22 = 0;

        /* IP citacky */
        public static string cIP11 = null;
        public static string cIP12 = null;
        public static string cIP21 = null;
        public static string cIP22 = null;

        internal static void AktualizujPLCCety()
        {
            input1RstN = ComChannel + "." + VarTable + "." + "input1RstN"; //v OPC: boolean
            input2RstN = ComChannel + "." + VarTable + "." + "input2RstN"; //v OPC: boolean
            input3RstN = ComChannel + "." + VarTable + "." + "input3RstN"; //v OPC: boolean
            input4RstN = ComChannel + "." + VarTable + "." + "input4RstN"; //v OPC: boolean
            input5RstN = ComChannel + "." + VarTable + "." + "input5RstN"; //v OPC: boolean
            input6RstN = ComChannel + "." + VarTable + "." + "input6RstN"; //v OPC: boolean

            output1RstN = ComChannel + "." + VarTable + "." + "output1RstN"; //v OPC: boolean
            output2RstN = ComChannel + "." + VarTable + "." + "output2RstN"; //v OPC: boolean
            output3RstN = ComChannel + "." + VarTable + "." + "output3RstN"; //v OPC: boolean
            output4RstN = ComChannel + "." + VarTable + "." + "output4RstN"; //v OPC: boolean
            output5RstN = ComChannel + "." + VarTable + "." + "output5RstN"; //v OPC: boolean
            output6RstN = ComChannel + "." + VarTable + "." + "output6RstN"; //v OPC: boolean

            showInput1N = ComChannel + "." + VarTable + "." + "showInput1N";
            showInput2N = ComChannel + "." + VarTable + "." + "showInput2N";
            showInput3N = ComChannel + "." + VarTable + "." + "showInput3N";
            showInput4N = ComChannel + "." + VarTable + "." + "showInput4N";
            showInput5N = ComChannel + "." + VarTable + "." + "showInput5N";
            showInput6N = ComChannel + "." + VarTable + "." + "showInput6N";

            showOutput1 = ComChannel + "." + VarTable + "." + "showOutput1N"; //v OPC:  boolean
            showOutput2 = ComChannel + "." + VarTable + "." + "showOutput2N"; //v OPC: boolean
            showOutput3 = ComChannel + "." + VarTable + "." + "showOutput3N"; //v OPC: boolean
            showOutput4 = ComChannel + "." + VarTable + "." + "showOutput4N"; //v OPC: boolean
            showOutput5 = ComChannel + "." + VarTable + "." + "showOutput5N"; //v OPC: boolean
            showOutput6 = ComChannel + "." + VarTable + "." + "showOutput6N"; //v OPC: boolean

            input1N = ComChannel + "." + VarTable + "." + "Input1N"; //v OPC Word array
            input2N = ComChannel + "." + VarTable + "." + "Input2N"; //v OPC Word array
            input3N = ComChannel + "." + VarTable + "." + "Input3N"; //v OPC Word array
            input4N = ComChannel + "." + VarTable + "." + "Input4N"; //v OPC Word array
            input5N = ComChannel + "." + VarTable + "." + "Input5N"; //v OPC Word array
            input6N = ComChannel + "." + VarTable + "." + "Input6N"; //v OPC Word array

            output1N = ComChannel + "." + VarTable + "." + "Output1N"; //v OPC Word array
            output2N = ComChannel + "." + VarTable + "." + "Output2N"; //v OPC Word array
            output3N = ComChannel + "." + VarTable + "." + "Output3N"; //v OPC Word array
            output4N = ComChannel + "." + VarTable + "." + "Output4N"; //v OPC Word array
            output5N = ComChannel + "." + VarTable + "." + "Output5N"; //v OPC Word array
            output6N = ComChannel + "." + VarTable + "." + "Output6N"; //v OPC Word array
    }
    }
}
