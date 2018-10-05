///////////////////////////////////////////////////////////////////
//  TokenizerTester
// - provide test function for each test case
using System;
using System.IO;
using System.Text;
using Tokenizer;

namespace LexicalScannerTester
{
    public class Test_Tokenizer
    {
        static string testCaseFileName = "TestCaseFolder/test_";
        static string testCaseResult = "TestCaseFolder/testTokenizerResult_";


        static string testCaseResultStandard = "TestCaseFolder/testTokenizerStandard_";

        public Test_Tokenizer()
        {
            return;
        }

        // public function include each test function
        public void TokenizerTester() { 
            // test_1
            if (!requirement_1()){
                Console.Write("\n  Testing Result: Failed\n");
            }
            else
            {
                Console.Write("\n  Testing Result: Passed\n");
            }

            // test_2
            if (!requirement_2()){
                Console.Write("\n  Testing Result: Failed\n");
            }
            else
            {
                Console.Write("\n  Testing Result: Passed\n");
            }

            // test_3
            if (!requirement_3()){
                Console.Write("\n  Testing Result: Failed\n");
            }
            else
            {
                Console.Write("\n  Testing Result: Passed\n");
            }

            // test_4
            if (!requirement_4()){
                Console.Write("\n  Testing Result: Failed\n");
            }
            else
            {
                Console.Write("\n  Testing Result: Passed\n");
            }

            // test_5
            if (!requirement_5()){
                Console.Write("\n  Testing Result: Failed\n");
            }
            else
            {
                Console.Write("\n  Testing Result: Passed\n");
            }

            // test_6
            if (!requirement_6()){
                Console.Write("\n  Testing Result: Failed\n");
            }
            else
            {
                Console.Write("\n  Testing Result: Passed\n");
            }

            // test_7
            if (!requirement_7()){
                Console.Write("\n  Testing Result: Failed\n");
            }
            else
            {
                Console.Write("\n  Testing Result: Passed\n");
            }

            // test_8
            if (!requirement_8()){
                Console.Write("\n  Testing Result: Failed\n");
            }
            else
            {
                Console.Write("\n  Testing Result: Passed\n");
            }

            // test_9
            if (!requirement_9()){
                Console.Write("\n  Testing Result: Failed\n");
            }
            else
            {
                Console.Write("\n  Testing Result: Passed\n");
            } 
        }
//#endif


        //----< test for alphanumeric tokens >---
        static private bool requirement_1(int testIndex = 1)
        {
            Console.Write("\n  Testing alphanumeric tokens");
            Console.Write("\n ============================");
            return testTokenizer(testIndex);
        }

        //----< test for single line comment tokens >---
        static private bool requirement_2(int testIndex = 2)
        {
            Console.Write("\n  Testing single line comment tokens");
            Console.Write("\n ============================");
            return testTokenizer(testIndex);
        }

        //----< test for multi line comment tokens >---
        static private bool requirement_3(int testIndex = 3)
        {
            Console.Write("\n  Testing multi line comment tokens");
            Console.Write("\n ============================");
            return testTokenizer(testIndex);
        }

        //----< test for single character punctuation tokens >---
        static private bool requirement_4(int testIndex = 4)
        {
            Console.Write("\n  Testing single character punctuation tokens");
            Console.Write("\n ============================");
            return testTokenizer(testIndex);
        }

        //----< test for double character punctuation tokens >---
        static private bool requirement_5(int testIndex = 5)
        {
            Console.Write("\n  Testing double character punctuation tokens");
            Console.Write("\n ============================");
            return testTokenizer(testIndex);
        }

        //----< test for single quotated tokens >---
        static private bool requirement_6(int testIndex = 6)
        {
            Console.Write("\n  Testing single quotated tokens");
            Console.Write("\n ============================");
            return testTokenizer(testIndex);
        }

        //----< test for double quotated tokens >---
        static private bool requirement_7(int testIndex = 7)
        {
            Console.Write("\n  Testing double quotated tokens");
            Console.Write("\n ============================");
            return testTokenizer(testIndex);
        }

        //----< test for for-loop express case >---
        static private bool requirement_8(int testIndex = 8)
        {
            Console.Write("\n  Testing for-loop express tokens");
            Console.Write("\n ============================");
            return testTokenizer(testIndex);
        }

        //----< test for # start with express case >---
        static private bool requirement_9(int testIndex = 9)
        {
            Console.Write("\n  Testing # start with express tokens");
            Console.Write("\n ============================");
            return testTokenizer(testIndex);
        }


        //----< compare two txt files >---
        static private bool compareTwoFiles(string fileName_1, string fileName_2)
        {
            StreamReader li = new StreamReader(fileName_1);
            StreamReader li2 = new StreamReader(fileName_2);
            while (true)
            {
                if (li.EndOfStream || li2.EndOfStream)
                    break;
                string liTxt = li.ReadLine();
                string li2Txt = li2.ReadLine();
                if (!liTxt.Equals(li2Txt))
                {
                    li.Close();
                    li2.Close();
                    return false;

                }
            }
            li.Close();
            li2.Close();
            return true;
        }

        //----< test for each tokenizer case by index>---
        static private bool testTokenizer(int testIndex)
        {
            Toker toker = new Toker();
            StreamWriter file = new StreamWriter("../../" + testCaseResult + testIndex + ".txt");   // read from the testcase cs file

            string fqf = System.IO.Path.GetFullPath("../../" + testCaseFileName + testIndex + ".cs");   // open the write to file
            if (!toker.open(fqf))
            {
                file.Close();
                Console.Write("\n can't open {0}\n", fqf);
                return false;
            }
            else
            {
                Console.Write("\n  processing file: {0}", fqf);
            }

            // write token into the output file untill eof
            while (!toker.isDone())
            {
                StringBuilder tok = toker.getTok();
                file.Write("\n -- line#{0, 4} : {1}", toker.lineCount(), tok);
            }
            toker.close();
            file.Close();

            return compareTwoFiles("../../" + testCaseResult + testIndex + ".txt", "../../" + testCaseResultStandard + testIndex + ".txt");
        }

        static public void addSingleCharToken(string a)
        {
            Toker.setSpecialSingleChars(a);
        }

        static public void addDoubleCharToken(string a)
        {
            Toker.setSpecialDoubleChars(a);
        }
    }
}
