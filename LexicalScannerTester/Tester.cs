/////////////////////////////////////////////////////////////////////////
// Tester.cs  -  LexicalScannerTester                                  //
//               AutoRun Test Case to Check the Functionality of       //
//               Tokenizer and SemiExpression                          //
// ver 1.0                                                             //
// Language:    C#, Visual Studio 10.0, .Net Framework 4.0             //
// Platform:    Dell Precision T7400 , Win 7, SP 1                     //
// Application: Pr#2 Help, CSE681, Fall 2011                           //
// Author:      Mengtao Tang                                           //
/////////////////////////////////////////////////////////////////////////
/*
 * Module Operations
 * =================
 *  Tester provides several test function to proceed with SemiExpression 
 *  and Tokenizer. Each test function with its corresponding test case
 *  file will compare the output from the test and the standard one, so
 *  that we are able to know if the Semi and Tokenizer are fucntional and
 *  we can also check which part would be wrong according to the specific
 *  failed test case.
 *  The output will be generated as txt file in the same folder of the 
 *  testcase file
 * 
 */


namespace LexicalScannerTester
{
    class Tester
    {
        static void Main(string[] args)
        {
            Test_Tokenizer tokenizerTest = new Test_Tokenizer();
            Test_SemiExp semiExpTest = new Test_SemiExp();

            // perform test
            tokenizerTest.TokenizerTester();
            semiExpTest.SemiExpTester();

        }
    }
}
