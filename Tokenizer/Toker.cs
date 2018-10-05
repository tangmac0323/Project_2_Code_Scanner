/////////////////////////////////////////////////////////////////////////
// Toker.cs  -  Tokenizer                                              //
//              Reads words and punctuation symbols from a file stream //
// ver 2.9                                                             //
// Language:    C#, Visual Studio 10.0, .Net Framework 4.0             //
// Platform:    Dell Precision T7400 , Win 7, SP 1                     //
// Application: Pr#2 Help, CSE681, Fall 2011                           //
// Author:      Jim Fawcett, CST 2-187, Syracuse University            //
//              (315) 443-3948, jfawcett@twcny.rr.com                  //
/////////////////////////////////////////////////////////////////////////
/*
 * Module Operations
 * =================
 * Toker provides, via the class CToker, the facilities to tokenize ASCII
 * text files.  That is, it composes the file's stream of characters into
 * words and punctuation symbols.
 * 
 * CToker works with a private buffer of characters from an attached file.
 * When the buffer is emptied CToker silently fills it again, so tokens
 * are always available until the end of file is reached.  End of file is
 * reported by tok = getTok() returning an empty token, e.g., tok == "".  
 *
 * Note: 
 * The tokenizer does not properly handle quoted strings that start
 * with the @ character to indicate \ should be treated as a character,
 * not the beginning of an escape sequence.
 * 
 * Public Interface
 * ================
 * CToker toker = new CToker();       // constructs CToker object
 * if(toker.openFile(fileName)) ...   // attaches toker to specified file
 * if(toker.openString(str)) ...      // attaches toker to specified string
 * toker.close();                     // closes stream
 * string tok = toker.getTok();       // extracts next token from stream
 * string tok = toker.peekNextTok();  // peeks but does not extract
 * toker.pushBack(tok);               // puts token back on stream
 * 
 */
/*
 * Build Process
 * =============
 * Required Files:
 *   Toker.cs TokenContext.cs TokenSourceFile.cs
 *   TokenSourceFile.cs 
 *   AlphaState.cs  CCOmmentsState.cs
 *   CppCommentState.cs DoubleState.cs  PuncState.cs
 *   SingleQuoteState.cs    SpeicalPuncState.cs
 *   WhiteSpaceState.cs
 * 
 * Compiler Command:
 *   csc /target:exe Toker.cs
 * 
 * Maintenance History
 * ===================
 * ver 3.0 : 3 Oct 18
 * - reconstruct class and modify the function ein each class
 * - revise getTok() function
 * ver 2.9 : 12 Feb 18
 * - fixed bug in extractComment that caused failure to detect tokens
 *   after processing a line ending with a C++ style comment
 * ver 2.8 : 14 Oct 14
 * - fixed bug in extract that caused tokenizing of multiline string
 *   to loop endlessly
 * - reset lineCount in Attach function
 * ver 2.7 : 21 Sep 14
 * - made returning comments optional
 * - fixed handling of @"..." strings
 * ver 2.6 : 19 Sep 14
 * - stopped returning comments in getTok function
 * ver 2.5 : 14 Aug 14
 * - added patch to handle @"..." string format
 * ver 2.4 : 24 Sep 11
 * - added a thrown exception if extract encounters a string with the 
 *   substring "@.  This should be handled but raises two many changes
 *   to fix immediately.
 * ver 2.3 : 05 Sep 11
 * - fixed bug collecting C Comments in eatCComment()
 * - fixed bug where token contained embedded newline, now broken
 *   into seperate tokens
 * - fixed ackward display formatting
 * - replaced untyped ArrayList with generic List<string> 
 * - added lineCount property
 * ver 2.2 : 10 Jun 08
 * - added IsGrammarPunctuation to make tokenizer treat underscore
 *   as an ASCII char rather than a punctuator and used that in
 *   fillBuffer and eatASCII
 * ver 2.1 : 14 Jun 05
 * - fixed newline handling bug in buffer filling routines:
 *   readLine, getLine, fillbuffer
 * - fixed newline handling bug in extractComment
 * ver 2.0 : 30 May 05
 * - added extraction of comments and quotes as tokens
 * - added openString(...) to attach tokenizer to string
 * ver 1.1 : 21 Sep 04
 * - added toker.close() in test stub
 * - added processing for all command line args
 * ver 1.0 : 31 Aug 03
 * - first release
 * 
 * Planned Changes:
 * ----------------
 * - Handle quoted strings that use the @"\X" construct to allow omitting 
 *   double \\ when \ should be treated like a character, not the beginning
 *   of an escape sequence. 
 * - Improve performance by change lineRemainder from string to StringBuilder
 *   to avoid a lot of copies.
 */


using System.Text;
using System;

namespace Tokenizer
{
    ///////////////////////////////////////////////////////////////////////
    // class Toker - Tokenizer
    public class Toker
    {
        private TokenContext context_;       // holds single instance of all states and token source
        //private bool keepComment = false;       // switch to keep or discard comment

        //----< initialize state machine >-------------------------------

        public Toker()
        {
            context_ = new TokenContext();      // context is the glue that holds all of the state machine parts 
        }
        //----< attempt to open source of tokens >-----------------------
        /*
         * If src is successfully opened, it uses TokenState.nextState(context_)
         * to set the initial state, based on the source content.
         */
        public bool open(string path)
        {
            TokenSourceFile src = new TokenSourceFile(context_);
            context_.src = src;
            return src.open(path);
        }
        //----< close source of tokens >---------------------------------

        public void close()
        {
            context_.src.close();
        }
        //----< extract a token from source >----------------------------

        private bool isWhiteSpaceToken(StringBuilder tok)
        {
            if (tok.Length > 1)
            {
                return (Char.IsWhiteSpace(tok[0]) && !tok[1].Equals('\n'));
            }
            else if (tok.Length > 0)
            {
                return (Char.IsWhiteSpace(tok[0]));
            }
            else
            {
                return false;
            }
        }

        public StringBuilder getTok()
        {
            StringBuilder tok = null;
            while (!isDone())
            {
                tok = context_.currentState_.getTok();
                context_.currentState_ = TokenState.nextState(context_);
                //if (!isWhiteSpaceToken(tok) && tok.ToString() != "/r/n")
                if (!isWhiteSpaceToken(tok))
                {
                    break;
                }
            }
            return tok;
        }

        //----< has Toker reached end of its source? >-------------------

        public bool isDone()
        {
            if (context_.currentState_ == null)
            {
                return true;
            }
            else
            {
                return false;
            }


            //return context_.currentState_.isDone();
        }


        public int lineCount() { return context_.src.lineCount; }

        //----< use to add more special single chars into the checking list >---
        static public void setSpecialSingleChars(string ssc)
        {
            TokenState.specialSinglePunct.Add(ssc);
        }

        //----< use to add more special double chars into the checking list >---
        static public void setSpecialDoubleChars(string ssc)
        {
            TokenState.specialDoublePunct.Add(ssc);
        }

        static public void displaySingleCharPunc()
        {
            Console.Write("\nCurrent Single Character Punctuation List: " + TokenState.specialSinglePunct.ToString() + "\n");
        }

        static public void displayDoubleCharPunc()
        {
            Console.Write("\nCurrent Double Character Punctuation List: " + TokenState.specialDoublePunct.ToString() + "\n");
        }
    }
}
