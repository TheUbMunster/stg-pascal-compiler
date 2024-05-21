using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler.Parser
{
   /// <summary>
   /// This attribute decorates properties that contain some type representing parsing data.
   /// This can be an ASTNode or a Token
   /// </summary>
   [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
   public sealed class ParserDataAttribute(int propertyOrder) : Attribute 
   {
      public int PropertyOrder { get => propertyOrder; }
   }
   /// <summary>
   /// This attribute decorates structs that contain properties that are decorated with
   /// ParserDataAttribute. This might be used in a derived ASTNode type that e.g.,
   /// uses a list of record structs, where the record struct is decorated with this attribute,
   /// containing exclusively parser data properties.
   /// </summary>
   [AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
   public sealed class ParserDataContainerAttribute() : Attribute { }
}
