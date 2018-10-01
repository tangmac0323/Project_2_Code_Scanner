/////////////////////////////////////////////////////////////////////////
// Toker.cs  -  Tokenizer                                              //
//              Reads words and punctuation symbols from a file stream //
// ver 2.8                                                             //
// Language:    C#, Visual Studio 15.5.2, .Net Framework 4.0           //
// Platform:    Alienware Area-51 , Win 10,                            //
// Application: Pr#2 Help, CSE681, Fall 2018                           //
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
 *   Toker.cs
 * 
 * Compiler Command:
 *   csc /target:exe /define:TEST_CTOKER CToker.cs
 * 
 * Maintenance History
 * ===================
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


using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Project_2_Code_Scanner
{
    class CToker
    {
        private TextReader tokenSouceFile = null;       // the source of to be tokenized
        private List<string> tokenHolder = null;        // list to hold the token during process
        private string lineHolder = null;               // string variable to hold the unprocessed line

        //private List<string> commentsSet = null;        // list to store the comments
        //private int lineCount = 0;                      // variable to save the line in the source file

        //----< return comments? property >----------------------------------

        public bool returnComments
        {
            get; set;
        }
        //----< line count property >----------------------------------------

        public int lineCount
        {
            get;
            private set;
        }

        // Constructor
        public CToker()
        {
            tokBuffer = new List<string>();
            lineCount = 0;
            returnComments = false;
        }

    #if(TEST_TOKER)

        [STAThread]
        static void Main(string[] args)
        {
          Console.Write("\n  Testing CToker - Tokenizer ");
          Console.Write("\n ============================\n");

          try 
          {
            CToker toker = new CToker();
            //toker.returnComments = true;

            if (args.Length == 0)
            {
              Console.Write("\n  Please enter name of file to tokenize\n\n");
              return;
            }
            foreach (string file in args)
            {
              string msg1;
              if (!toker.openFile(file))
              {
                msg1 = "Can't open file " + file;
                Console.Write("\n\n  {0}", msg1);
                Console.Write("\n  {0}", new string('-', msg1.Length));
              }
              else
              {
                msg1 = "Processing file " + file;
                Console.Write("\n\n  {0}", msg1);
                Console.Write("\n  {0}", new string('-', msg1.Length));
                string tok = "";
                while ((tok = toker.getTok()) != "")
                  if (tok != "\n")
                    Console.Write("\n{0}", tok);
                toker.close();
              }
            }
            Console.Write("\n");
            //
            string[] msgs = new string[12];
            msgs[0] = "abc";
            msgs[11] = "-- \"abc def\" --";
            msgs[1] = "string with double quotes \"first quote\""
                      + " and \"second quote\" but no more";
            msgs[2] = "string with single quotes \'1\' and \'2\'";
            msgs[3] = "string with quotes \"first quote\" and \'2\'";
            msgs[4] = "string with C comments /* first */ and /*second*/ but no more";
            msgs[10] = @"string with @ \\stuff";
            msgs[5] = "/* single C comment */";
            msgs[6] = " -- /* another single comment */ --";
            msgs[7] = "// a C++ comment\n";
            msgs[8] = "// another C++ comment\n";
            msgs[9] = "/*\n *\n *\n */";

            foreach (string msg in msgs)
            {
              if (!toker.openString(msg))
              {
                string msg2 = "Can't open string for reading";
                Console.Write("\n\n  {0}", msg2);
                Console.Write("\n  {0}", new string('-', msg2.Length));
              }
              else
              {
                string msg2 = "Processing \"" + msg + "\"";
                Console.Write("\n\n  {0}", msg2);
                Console.Write("\n  {0}", new string('-', msg2.Length));
                string tok = "";
                while ((tok = toker.getTok()) != "")
                {
                  if (tok != "\n")
                    Console.Write("\n{0}", tok);
                  else
                    Console.Write("\nnewline");
                }
                toker.close();
              }
            }
            Console.Write("\n\n");
          }
          catch (Exception ex)
          {
            Console.Write("\n\n  token \"{0}\" has embedded newline\n\n", ex.Message);
          }
        }
    #endif
    }
}
