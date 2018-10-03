using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemiExpression
{
    class SemiExpTest
    {
        static void Main(string[] args)
        {
            Console.Write("\n  Testing semiExp Operations");
            Console.Write("\n ============================\n");

            SemiExp test = new SemiExp();
            test.returnNewLines = true;
            test.displayNewLines = true;

            string testFile = "../../testSemi.txt";
            if (!test.open(testFile))
                Console.Write("\n  Can't open file {0}", testFile);
            while (test.getSemi())
                test.display();

            test.initialize();
            test.insert(0, "this");
            test.insert(1, "is");
            test.insert(2, "a");
            test.insert(3, "test");
            test.display();

            Console.Write("\n  2nd token = \"{0}\"\n", test[1]);

            Console.Write("\n  removing first token:");
            test.remove(0);
            test.display();
            Console.Write("\n");

            Console.Write("\n  removing token \"test\":");
            test.remove("test");
            test.display();
            Console.Write("\n");

            Console.Write("\n  making copy of semiExpression:");
            SemiExp copy = test.clone();
            copy.display();
            Console.Write("\n");

            if (args.Length == 0)
            {
                Console.Write("\n  Please enter name of file to analyze\n\n");
                return;
            }
            SemiExp semi = new SemiExp();
            semi.returnNewLines = false;
            if (!semi.open(args[0]))
            {
                Console.Write("\n  can't open file {0}\n\n", args[0]);
                return;
            }

            Console.Write("\n  Analyzing file {0}", args[0]);
            Console.Write("\n ----------------------------------\n");

            while (semi.getSemi())
                semi.display();
            semi.close();
        }
    }
}
