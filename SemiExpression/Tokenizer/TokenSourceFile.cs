using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenizer
{
    class TokenSourceFile : ITokenSourceInterface
    {
        public int lineCount { get; set; } = 1;
        private StreamReader fs_;           // physical source of text
        private List<int> charQ_ = new List<int>();   // enqueing ints but using as chars
        private TokenContext context_;

        private long peekPos;               // save the current location of the peek point


        public TokenSourceFile(TokenContext context)
        {
            context_ = context;
        }
        //----< attempt to open file with a System.IO.StreamReader >-----

        public bool open(string path)
        {
            try
            {
                fs_ = new StreamReader(path, true);
                peekPos = fs_.BaseStream.Position;      // store the initial position
                context_.currentState_ = TokenState.nextState(context_);
            }
            catch (Exception ex)
            {
                Console.Write("\n  {0}\n", ex.Message);
                return false;
            }
            return true;
        }
        //----< close file >---------------------------------------------

        public void close()
        {
            fs_.Close();
        }
        //----< extract the next available integer >---------------------
        /*
         *  - checks to see if previously enqueued peeked ints are available
         *  - if not, reads from stream
         */
        public int next()
        {
            int ch;
            if (charQ_.Count == 0)  // no saved peeked ints
            {
                if (end())
                {
                    return -1;
                }
                ch = fs_.Read();
            }
            else                    // has saved peeked ints, so use the first
            {
                ch = charQ_[0];
                charQ_.Remove(ch);
            }
            if ((char)ch == '\n')   // track the number of newlines seen so far
            {
                ++lineCount;
            }

            return ch;
        }
        //----< peek n ints into source without extracting them >--------
        /*
         *  - This is an organizing prinicple that makes tokenizing easier
         *  - We enqueue because file streams only allow peeking at the first int
         *    and even that isn't always reliable if an error occurred.
         *  - When we look for two punctuator tokens, like ==, !=, etc. we want
         *    to detect their presence without removing them from the stream.
         *    Doing that is a small part of your work on this project.
         */
        public int peek(int n = 0)
        {
            if (n < charQ_.Count)  // already peeked, so return
            {
                return charQ_[n];
            }
            else                  // nth int not yet peeked
            {
                for (int i = charQ_.Count; i <= n; ++i)
                {
                    if (end())
                    {
                        return -1;
                    }
                    charQ_.Add(fs_.Read());  // read and enqueue
                }
                return charQ_[n];   // now return the last peeked
            }
        }


        /*
        public string peekLine()
        {
            string line = fs_.ReadLine();
            if (line == null)
            {
                return null;
            }

            return line;
        }
        */

        //----< reached the end of the file stream? >--------------------

        public bool end()
        {
            return fs_.EndOfStream;
        }
    }
}
