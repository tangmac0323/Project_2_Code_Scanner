///////////////////////////////////////////////////////////////////
//----< test for for-loop express case >---
namespace LexicalScannerTester.TestCaseFolder
{
    class test_8
    {
        public void testLoop()
        {
            int m = 0;

            int a = 2;
            for (int i = 0; i < 10; i++)
            {
                m *= 2;
            }

            while (true)
            {
                a += 3;
            }
        }
    }
}
