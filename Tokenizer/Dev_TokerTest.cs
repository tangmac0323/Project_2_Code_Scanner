using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenizer
{

    class Dev_TokerTest
    {
        static bool testToker(string path)
        {
            Toker toker = new Toker();

            string fqf = System.IO.Path.GetFullPath(path);
            if (!toker.open(fqf))
            {
                Console.Write("\n can't open {0}\n", fqf);
                return false;
            }
            else
            {
                Console.Write("\n  processing file: {0}", fqf);
            }
            while (!toker.isDone())
            {
                StringBuilder tok = toker.getTok();
                Console.Write("\n -- line#{0, 4} : {1}", toker.lineCount(), tok);
            }
            toker.close();
            return true;
        }
        
        
        static void Main(string[] args)
        {
            Console.Write("\n  Demonstrate Toker class");
            Console.Write("\n =========================");

            StringBuilder msg = new StringBuilder();
            msg.Append("\n  Some things this demo does not do for CSE681 Project #2:");
            msg.Append("\n  - collect comments as tokens");
            msg.Append("\n  - collect double quoted strings as tokens");
            msg.Append("\n  - collect single quoted strings as tokens");
            msg.Append("\n  - collect specified single characters as tokens");
            msg.Append("\n  - collect specified character pairs as tokens");
            msg.Append("\n  - integrate with a SemiExpression collector");
            msg.Append("\n  - provide the required package structure");
            msg.Append("\n");

            Console.Write(msg);

            testToker("../../Test.txt");
            //testToker("../../Toker.cs");

            Console.Write("\n\n");
        }
        
    }
}
