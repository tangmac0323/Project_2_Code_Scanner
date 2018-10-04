using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tokenizer
{
    abstract class TokenState : ITokenStateInterface
    {
        internal TokenContext context_ { get; set; }  // derived classes store context ref here

        static internal List<string> specialDoublePunct = new List<string>(
            new string[]{
                        "/*", "*/", "//", "!=", "==", ">=", "<=", "&&", "||", "--", "++",
                        "+=", "-=", "*=", "/=", "%=", "&=", "^=", "|=", "<<", ">>",
                        "\\n", "\\t", "\\r", "\\f"
                        }
            );

        static internal List<string> specialSinglePunct = new List<string>(new string[]
        {
        "<", ">", "[", "]", "(", ")", "{", "}", ":", "=", "+", "-", "*"
        });

        //----< delegate source opening to context's src >---------------

        public bool open(string path)
        {
            return context_.src.open(path);
        }
        //----< pass interface's requirement onto derived states >-------

        public abstract StringBuilder getTok();

        //----< derived states don't have to know about other states >---

        static public TokenState nextState(TokenContext context)
        {
            // peek on the next tok
            int nextItem = context.src.peek();
            if (nextItem < 0)
            {
                return null;
            }
            char ch = (char)nextItem;

            // check whitespace

            if (Char.IsWhiteSpace(ch))
            {
                return context.whiteSpaceState_;
            }
           
            // check alpha
            if (Char.IsLetterOrDigit(ch))
            {
                return context.alphaState_;
            }


            // check if single quoted
            if (ch.Equals('\''))
            {
                return context.sQuoteState_;
            }

            // check if double quoted
            if (ch.Equals('\"'))
            {
                return context.dQuoteState_;
            }

            // check if punctuation
            if (isPunctuation(ch))
            {
                // check the next char in the context
                int nextItem2 = context.src.peek(1);
                if (nextItem2 < 0)
                {
                    return context.puncState_;
                }
                char ch2 = (char)nextItem2;

                // check if special punctuation
                char[] tempCharBuffer = { ch, ch2 };
                string tempStr = new string(tempCharBuffer);
                if (isSpecialDoublePunctuation(tempStr))
                {
                    // check if the comment start
                    if (tempStr.Equals("//"))
                    {
                        return context.cCommentState_;
                    }
                    else if (tempStr.Equals("/*"))
                    {
                        return context.cppCommentState_;
                    }
                    else
                    {
                        return context.specialPuncState_;
                    }
                }
                else
                {
                    return context.puncState_;
                }

            }

            return null;



            //return context.puncState_;
        }
        //----< has tokenizer reached the end of its source? >-----------

        public bool isDone()
        {
            if (context_.src == null)
                return true;
            return context_.src.end();
        }

        static private bool isPunctuation(char ch)
        {
            return (!Char.IsWhiteSpace(ch) && !Char.IsLetterOrDigit(ch) || isSpecialSinglePunctuation(ch));
        }

        static private bool isSpecialDoublePunctuation(string ch_comb)
        {
            return specialDoublePunct.Contains(ch_comb);
        }

        static private bool isSpecialSinglePunctuation(string ch_comb)
        {
            return specialSinglePunct.Contains(ch_comb);
        }

        static public void setSpecialSingleChars(string ssc)
        {
            specialSinglePunct.Add(ssc);
        }

        static public void setSpecialDoubleChars(string ssc)
        {
            specialDoublePunct.Add(ssc);
        }
    }
}
