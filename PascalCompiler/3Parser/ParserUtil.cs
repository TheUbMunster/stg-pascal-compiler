using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler.Parser
{
   public class ParserException : Exception
   {
      public ParserException(string? message) : base(message) { }
      public ParserException(string? message, Exception? innerException) : base(message, innerException) { }
   }
   public enum ASTNodeType : int //this list is comprised only of terminal symbols??? (is the T suffix necessary?)
   { //actually, I think this list is comprised only of "is a thing" things. I.e., non-ast-categories
      UNDEFINED = int.MinValue,
      LabelDeclarationPart = 0, //although technechally not a terminal according to 6.2.1 spec syntax, it can be parsed as one.
      ConstantDefinitionPart,
      TypeDefinitionPart,
      VariableDeclarationPart,
      ProcedureAndFunctionDeclarationPart, //this might not be a terminal
      LabelValue,
      ConstantDefinition,
      Identifier,
      Constant
   }
}
