//Copyright TheUbMunster, 2024, all rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLRParser;
using PascalCompiler.Scanner;

namespace PascalCompiler.NewParser
{
   //online reading has impressed upon me that handwriting the table for a table driven parser is suislide,
   //so using a parser generator is a good idea.
   //maybe antlr even though I don't want to :(

   //https://github.com/tunnelvisionlabs/antlr4cs
   //https://www.antlr.org/
   public static class Parser
   {
      public static void Parse(Source source)
      {
         //i'm 80% sure that since "Program" is the start symbol, and the program ?must? be terminated by the last "end ." in the file, that
         //any file parsed will result in one mega ASTNode, not a list?
         //int ind = 0;
         //bool firstIter = true;
         //while (ind < source.LexerTokens.Count/* && source.LexerTokens[ind].Type != TokenType.END_OF_FILE*/)
         //{
         //   int indBefore = ind;
         //   ASTNode? node = ASTNode.Parse(source, ref ind);
         //   if (node == null/* || node.NodeType == ASTNodeType.UNDEFINED*/)
         //   {
         //      source.AppendMessage(new Source.Message(Source.CompilerPhase.Parse, Source.Severity.Error, "No valid candidates for parsing token", source.LexerTokens[ind].FileLocation, true));
         //      throw new Exception(); //todo: put a message in this??
         //   }
         //   else if (indBefore == ind)
         //   {
         //      source.AppendMessage(new Source.Message(Source.CompilerPhase.Parse, Source.Severity.Error, "The root parse function parsed a non-null node, and yet it consumed no tokens! No progress in the token stream was made, so this will go on forever; breaking free.", node.FileLocation, true));
         //      throw new Exception(); //todo: put a message in this??
         //   }
         //   if (firstIter)
         //   {
         //      firstIter = false;
         //      if (node.NodeType != ASTNodeType.Program)
         //      {
         //         source.AppendMessage(new Source.Message(Source.CompilerPhase.Parse, Source.Severity.Error, "The first AST Node parsed was not the 'Program' node!", node.FileLocation, true));
         //         throw new Exception(); //todo: put a message in this??
         //      }
         //   }

         //   source.AddParserNode(node);
         //}
         //source.ParsingComplete();
         GLRParser.GLRParser parser = new GLRParser.GLRParser(PascalGrammar.grammarSource);
      }
   }
}
