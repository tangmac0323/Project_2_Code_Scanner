/////////////////////////////////////////////////////////////////////////
// Semi.cs   -  Builds semiExpressions                                 //
// ver 2.2                                                             //
// Language:    C#, Visual Studio 10.0, .Net Framework 4.0             //
// Platform:    Dell Precision T7400 , Win 7, SP 1                     //
// Application: Pr#2 Help, CSE681, Fall 2011                           //
// Author:      Jim Fawcett, CST 2-187, Syracuse University            //
//              (315) 443-3948, jfawcett@twcny.rr.com                  //
/////////////////////////////////////////////////////////////////////////
/*
 * Module Operations
 * =================
 * Semi provides, via class SemiExp, facilities to extract semiExpressions.
 * A semiExpression is a sequence of tokens that is just the right amount
 * of information to parse for code analysis.  SemiExpressions are token
 * sequences that end in "{" or a"}" or ";"
 * 
 * SemiExp works with a private Toker object attached to a specified file.
 * It provides a get() function that extracts semiExpressions from the file
 * while filtering out comments and merging quotes into single tokens.
 * 
 * Public Interface
 * ================
 * SemiExp semi = new CSemiEx;();    // constructs SemiExp object
 * if(semi.open(fileName)) ...        // attaches semi to specified file
 * semi.close();                      // closes file stream
 * if(semi.Equals(se)) ...            // do these semiExps have same tokens?
 * int hc = semi.GetHashCode()        // returns hashcode
 * if(getSemi()) ...                  // extracts and stores next semiExp
 * int len = semi.count;              // length property
 * semi.verbose = true;               // verbose property - shows tokens
 * string tok = semi[2];              // access a semi token
 * string tok = semi[1];              // extract token
 * semi.flush();                      // removes all tokens
 * semi.initialize();                 // adds ";" to empty semi-expression
 * semi.insert(2,tok);                // inserts token as third element
 * semi.Add(tok);                     // appends token
 * semi.Add(tokArray);                // appends array of tokens
 * semi.display();                    // sends tokens to Console
 * string show = semi.gotCollection();   // returns tokens as single string
 * semi.returnNewLines = false;       // property defines newline handling
 *                                    //   default is true
 * semi.gotCollection();                // return the display string
 */
//
/*
 * Build Process
 * =============
 * Required Files:
 *   SemiExpression.cs ITokenCollection.cs
 *   Toker.cs
 * 
 * Compiler Command:
 *   csc /target:exe Semi.cs Toker.cs
 * 
 * Maintenance History
 * ===================
 * ver 3.0 : 03 OCT 18
 * - modify trival function in the class to fetch the output from Toker
 * ver 2.3 : 07 Aug 18
 * - added functions isWhiteSpace and trim
 *   That eliminates whitespace tokens in SemiExp, making them easier to use.
 * ver 2.2 : 14 Aug 14
 * - added folding rule for "for(int i=0; i<count; ++i)" type statements
 * ver 2.1 : 24 Sep 11
 * - collect line starting with # and ending with \n as semiExpression.
 * ver 2.0 : 05 Sep 11
 * - Converted to new C# property syntax
 * - Converted from untyped ArrayList to generic List<string>
 * - Simplified display() and gotCollection()
 * - Added new tests in test stub
 * ver 1.9 : 27 Sep 08
 * - Changed comments on manual page to say that semi.ReturnNewLines is true by default
 * ver 1.8 : 10 Jun 08
 * - Aniruddha Gore added Contains function and set returnNewLines as the default
 * ver 1.7 : 17 Jun 06
 * - added displayNewLines property
 * ver 1.6 : 16 Jun 06
 * - added CSemi member functions copy(), remove(int i), and remove(string tok).
 * ver 1.5 : 12 Jun 05
 * - added returnNewLines property
 * - modified way get() behaves so that it will not hang on files that
 *   end with text that have no semiExp terminator.
 * ver 1.4 : 30 May 05
 * - removed CppCommentFilter, CCommentFilter, SQuoteFilter, DQuoteFilter
 *   since Toker now returns comments and quotes as tokens.
 * - added isComment(string tok) member function
 * ver 1.3 : 16 Sep 03
 * - removed insert(tokenArray), added Add(tokenArray)
 *   Since this is a change to public interface it may break some code.
 *   It simply changes the name of the function to more directly 
 *   describe what it does - append a token array.
 * - added overrides of Equals(object) and GetHashCode()
 * - completed Manual Page description of public interface
 * ver 1.2 : 14 Sep 03
 * - cosmetic changes to comments
 * - Added formatting of extracted comments (see notes in code below)
 * ver 1.1 : 13 Sep 03
 * - fixed bug in CppCommentFilter() that caused collection to terminate
 *   if a C++ comment was on same line as a semiExpression.
 * - added calls to semiExp.Add(currTok) in SQuoteFilter() and DQuoteFilter()
 *   which simplified getSemi().
 * - added some functions to create and manipulate semi-expressions.
 * ver 1.0 : 31 Aug 03
 * - first release
 * 
 * Planned Modifications:
 * ----------------------
 * - return, or don't return, comments based on discardComments property
 *   which is now present but inactive.
 */
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tokenizer;

namespace SemiExpression
{
    ///////////////////////////////////////////////////////////////////////
    // class SemiExp - filters token stream and collects semiExpressions

    public class SemiExp : ITokenCollection
    {
        Toker toker = null;
        List<string> semiExp = null; // this is a trailing comment
        string currTok;
        string prevTok;

        //----< line count property >----------------------------------------

        public int lineCount
        {
            get { return toker.lineCount(); }
        }
        //----< constructor >------------------------------------------------

        public SemiExp()
        {
            toker = new Toker();
            semiExp = new List<string>();
            discardComments = true;  // not implemented yet
            returnNewLines = true;
            displayNewLines = false;
            discardComments = true;
        }

        //----< test for equality >------------------------------------------

        override public bool Equals(Object semi)
        {
            SemiExp temp = (SemiExp)semi;
            if (temp.count != this.count)
                return false;
            for (int i = 0; i < temp.count && i < this.count; ++i)
                if (this[i] != temp[i])
                    return false;
            return true;
        }

        //---< pos of first str in semi-expression if found, -1 otherwise >--

        public int FindFirst(string str)
        {
            for (int i = 0; i < count - 1; ++i)
                if (this[i] == str)
                    return i;
            return -1;
        }
        //---< pos of last str in semi-expression if found, -1 otherwise >--- 

        public int FindLast(string str)
        {
            for (int i = this.count - 1; i >= 0; --i)
                if (this[i] == str)
                    return i;
            return -1;
        }
        //----< deprecated: here to avoid breakage with old code >----------- 

        public int Contains(string str)
        {
            return FindLast(str);
        }
        //----< have to override GetHashCode() >-----------------------------

        override public System.Int32 GetHashCode()
        {
            return base.GetHashCode();
        }
        //----< opens member tokenizer with specified file >-----------------

        public bool open(string fileName)
        {
            return toker.open(fileName);
        }
        //----< close file stream >------------------------------------------

        public void close()
        {
            toker.close();
        }

        //----< extract the whole line if start with using or # >------

        //----< is this the last token in the current semiExpression? >------

        bool isTerminator(string tok)
        {
            switch (tok)
            {
                case ";": return true;
                case "{": return true;
                case "}": return true;
                case "\n":
                    if (this.FindFirst("#") != -1)  // expensive - may wish to cache in get
                        return true;
                    return false;
                //case "/r/n":
                    //return true;
                default: return false;
            }
        }
        //----< get next token, saving previous token >----------------------

        string get()
        {
            prevTok = currTok;

            // check if there is more token to extract
            if (toker.isDone())
            {
                currTok = "";
                return currTok;
            }
            currTok = toker.getTok().ToString();


            if (verbose)
            {
                Console.Write("{0} ", currTok);
            }
            return currTok;
        }
        //----< is this character a punctuator> >----------------------------

        bool IsPunc(char ch)
        {
            return (Char.IsPunctuation(ch) || Char.IsSymbol(ch));
        }
        //
        //----< are these characters an operator? >--------------------------
        //
        // Performance issue - C# would not let me make opers static, so
        // it is being constructed on every call.  This is not desireable,
        // but neither is using a static data member that is initialized
        // remotely.  I will think more about this later.

        bool IsOperatorPair(char first, char second)
        {
            string[] opers = new string[]
            {
        "!=", "==", ">=", "<=", "&&", "||", "--", "++",
        "+=", "-=", "*=", "/=", "%=", "&=", "^=", "|=", "<<", ">>",
        "\\n", "\\t", "\\r", "\\f"
            };

            StringBuilder test = new StringBuilder();
            test.Append(first).Append(second);
            foreach (string oper in opers)
                if (oper.Equals(test.ToString()))
                    return true;
            return false;
        }
        //----< collect semiExpression from filtered token stream >----------

        public bool isWhiteSpace(string tok)
        {
            Char ch = tok[0];
            /*
            if (tok.Length > 1)
            {
                return (Char.IsWhiteSpace(tok[0]) || !tok[1].Equals('\n'));
            }
            else
            {
                return (Char.IsWhiteSpace(tok[0]));
            }
            */
            return (Char.IsWhiteSpace(tok[0]));
        }

        public void trim()
        {
            SemiExp temp = new SemiExp();
            foreach (string tok in semiExp)
            {
                if (isWhiteSpace(tok))
                    continue;
                temp.Add(tok);
            }
            semiExp = temp.semiExp;
        }

        public bool getSemi()
        {
            semiExp.RemoveRange(0, semiExp.Count);  // empty container
            do
            {
                get();
                if (currTok == "") { break; }   // end of linea
                // check if comment
                if (!discardComments && isComment(currTok))
                {
                    semiExp.Add(currTok);
                }
                else if (discardComments && isComment(currTok))
                {
                    return true;
                }
                else if ((currTok == "\r\n") && semiExp.Count >0)
                {
                    if (semiExp[0].StartsWith("#")) { semiExp.Add(currTok); break; }
                }
                else if (returnNewLines || currTok != "\n")
                {
                    semiExp.Add(currTok);
                }
                else { break; }
            } while (!isTerminator(currTok) || count == 0 );
            trim();
            if (semiExp.Contains("for"))
            {
                SemiExp se = clone();       // make a copy of the semiExp
                getSemi();                  // note recursive call
                se.Add(semiExp.ToArray());
                getSemi();
                se.Add(semiExp.ToArray());
                semiExp.Clear();
                for (int i = 0; i < se.count; ++i) { semiExp.Add(se[i]); }
            } 
            return (semiExp.Count > 0);
        }
        //----< get length property >----------------------------------------

        public int count
        {
            get { return semiExp.Count; }
        }
        //----< indexer for semiExpression >---------------------------------

        public string this[int i]
        {
            get { return semiExp[i]; }
            set { semiExp[i] = value; }
        }
        //----< insert token - fails if out of range and returns false>------

        public bool insert(int loc, string tok)
        {
            if (0 <= loc && loc < semiExp.Count)
            {
                semiExp.Insert(loc, tok);
                return true;
            }
            return false;
        }
        //----< append token to end of semiExp >-----------------------------

        public SemiExp Add(string token)
        {
            semiExp.Add(token);
            return this;
        }
        //----< load semiExp from array of strings >-------------------------

        public void Add(string[] source)
        {
            foreach (string tok in source)
                semiExp.Add(tok);
        }
        //--< initialize semiExp with single ";" token - used for testing >--

        public bool initialize()
        {
            if (semiExp.Count > 0)
                return false;
            semiExp.Add(";");
            return true;
        }
        //----< remove all contents of semiExp >-----------------------------

        public void flush()
        {
            semiExp.RemoveRange(0, semiExp.Count);
        }
        //----< is this token a comment? >-----------------------------------

        public bool isComment(string tok)
        {
            if (tok.Length > 1)
                if (tok[0] == '/')
                    if (tok[1] == '/' || tok[1] == '*')
                        return true;
            return false;
        }
        //----< display semiExpression on Console >--------------------------

        public void display()
        {
            Console.Write("\n -- ");
            Console.Write(gotCollection());
        }

        //----< output semiExpression as txt file >--------------------------
        /*
        public void writeToFile(string filePath)
        {
            StreamWriter file = new StreamWriter("../../" + testCaseResult + testIndex + ".txt");
            System.IO.File.WriteAllText("../../" + filePath, gotCollection());
        }
        */

        //----< return display string >--------------------------------------
        // implemented
        public string gotCollection()
        {
            StringBuilder disp = new StringBuilder("");
            foreach (string tok in semiExp)
            {
                disp.Append(tok);
                if (tok.IndexOf("\n") != tok.Length - 1)
                    disp.Append(" ");
            }
            return disp.ToString();
        }
        //----< announce tokens when verbose is true >-----------------------

        public bool verbose
        {
            get;
            set;
        }
        //----< determines whether new lines are returned with semi >--------

        public bool returnNewLines
        {
            get;
            set;
        }
        //----< determines whether new lines are displayed >-----------------

        public bool displayNewLines
        {
            get;
            set;
        }
        //----< determines whether comments are discarded >------------------

        public bool discardComments
        {
            get;
            set;
        }
        //
        //----< make a copy of semiEpression >-------------------------------

        public SemiExp clone()
        {
            SemiExp copy = new SemiExp();
            for (int i = 0; i < count; ++i)
            {
                copy.Add(this[i]);
            }
            return copy;
        }
        //----< remove a token from semiExpression >-------------------------

        public bool remove(int i)
        {
            if (0 <= i && i < semiExp.Count)
            {
                semiExp.RemoveAt(i);
                return true;
            }
            return false;
        }
        //----< remove a token from semiExpression >-------------------------

        public bool remove(string token)
        {
            if (semiExp.Contains(token))
            {
                semiExp.Remove(token);
                return true;
            }
            return false;
        }

    }
}
    //
