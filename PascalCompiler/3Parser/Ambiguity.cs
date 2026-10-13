//Copyright TheUbMunster, 2024, all rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PascalCompiler.Parser
{
   /// <summary>
   /// A placeholder in the parse tree for a node whose readings are spelled identically, so the parser cannot pick one. The spec
   /// resolves these through what an identifier was declared as (6.2.2.11), e.g. a bare identifier used as a factor is a
   /// variable-access, a constant-identifier, a parameterless function-designator or a bound-identifier, and "x^" is an
   /// identified-variable or a buffer-variable depending on the type of x.
   ///
   /// The parser builds every reading that fits over exactly the same tokens (see ASTNode.ParseLongest) and puts a placeholder in
   /// the slot instead: the nested class "Ambiguous" of the spec class in question (Factor.Ambiguous, VariableAccess.Ambiguous, ...),
   /// which derives from that class so that it fits the slot, holds the readings in Candidates, and has its inherited members unset.
   /// The type checker resolves each placeholder when its traversal reaches it (by then every identifier it depends on has been
   /// declared, 6.2.2.9) and assigns Resolved. Ambiguity.Collapse afterwards replaces every placeholder by its Resolved wherever it
   /// sits, so that nothing after type checking has to know ambiguity existed. Readings share child nodes, so until then the tree
   /// is a DAG at these points.
   /// </summary>
   public interface IAmbiguous
   {
      /// <summary>Every reading of the same tokens, in grammar order. Always at least two.</summary>
      IReadOnlyList<ASTNode> Candidates { get; }
      /// <summary>The reading the type checker chose; null until it has.</summary>
      ASTNode? Resolved { get; }
   }

   /// <summary>
   /// Tree-wide operations on placeholders (see IAmbiguous). They use reflection so that the generated node classes need no per-class
   /// code: a property is a child slot if it is writable and its type is an ASTNode, a list of ASTNodes, or a list of value tuples
   /// containing ASTNodes, which are the shapes GrammarClasses.cs uses.
   /// </summary>
   public static class Ambiguity
   {
      /// <summary>
      /// Replaces every placeholder under <paramref name="root"/> by the reading the type checker chose for it, recursing into the chosen
      /// readings only (the rejected ones, and anything inside them, are dropped). Returns the node that now stands where root was, since
      /// root itself may be a placeholder. Run once after type checking succeeded; an unresolved placeholder throws, because it is either
      /// a compiler bug or the consequence of a type error that should have stopped compilation before this point.
      /// </summary>
      public static T Collapse<T>(T root) where T : ASTNode
      {
         ASTNode collapsed = Resolution(root);
         CollapseChildren(collapsed, new HashSet<ASTNode>(ReferenceEqualityComparer.Instance));
         return (T)collapsed;
      }

      /// <summary>The reading a placeholder resolved to (following a chain of placeholders if need be), or the node itself when it isn't one.</summary>
      public static ASTNode Resolution(ASTNode node)
      {
         while (node is IAmbiguous placeholder)
            node = placeholder.Resolved ?? throw new InvalidOperationException($"An ambiguous {node.NodeType} at file position {node.FileLocation} was never resolved by the type checker (compiler bug, or a type error that should have stopped compilation).");
         return node;
      }

      /// <summary>
      /// Every placeholder under <paramref name="root"/>. Before type checking this is what needs resolving (readings are searched too,
      /// since placeholders nest inside readings); after Collapse it should yield nothing.
      /// </summary>
      public static IEnumerable<IAmbiguous> Placeholders(ASTNode root)
      {
         HashSet<ASTNode> visited = new HashSet<ASTNode>(ReferenceEqualityComparer.Instance);
         Stack<ASTNode> work = new Stack<ASTNode>();
         work.Push(root);
         while (work.Count > 0)
         {
            ASTNode node = work.Pop();
            if (!visited.Add(node))
               continue;
            if (node is IAmbiguous placeholder)
            {
               yield return placeholder;
               foreach (ASTNode candidate in placeholder.Candidates)
                  work.Push(candidate);
            }
            else
            {
               foreach (ASTNode child in Children(node))
                  work.Push(child);
            }
         }
      }

      #region walking the child slots (ASTNode.ChildSlots is the one definition of what a node's children are)
      private static IEnumerable<ASTNode> Children(ASTNode node)
      {
         foreach ((PropertyInfo slot, _) in ASTNode.ChildSlots(node.GetType()))
         {
            object? value = slot.GetValue(node); //a TokenNode value is neither a node nor a list and is skipped below
            if (value is ASTNode child)
               yield return child;
            else if (value is IEnumerable list)
               foreach (object? element in list)
                  foreach (ASTNode nested in NodesIn(element))
                     yield return nested;
         }
      }

      private static IEnumerable<ASTNode> NodesIn(object? element)
      {
         if (element is ASTNode node)
            yield return node;
         else if (element is ITuple tuple) //(TokenNode separator, Statement statement) and the like
            for (int i = 0; i < tuple.Length; i++)
               foreach (ASTNode inner in NodesIn(tuple[i]))
                  yield return inner;
      }

      private static void CollapseChildren(ASTNode node, HashSet<ASTNode> visited)
      {
         if (!visited.Add(node))
            return;
         foreach ((PropertyInfo slot, _) in ASTNode.ChildSlots(node.GetType()))
         {
            object? value = slot.GetValue(node);
            if (value is ASTNode child)
            {
               ASTNode replacement = Resolution(child);
               if (!ReferenceEquals(replacement, child))
                  slot.SetValue(node, replacement); //reflection may invoke the private init accessor; init is only a compile-time rule
               CollapseChildren(replacement, visited);
            }
            else if (value is IList list)
            {
               for (int i = 0; i < list.Count; i++)
               {
                  (object? element, bool changed) = CollapseElement(list[i], visited);
                  if (changed)
                     list[i] = element;
               }
            }
         }
      }

      private static (object? element, bool changed) CollapseElement(object? element, HashSet<ASTNode> visited)
      {
         switch (element)
         {
            case ASTNode child:
            {
               ASTNode replacement = Resolution(child);
               CollapseChildren(replacement, visited);
               return (replacement, !ReferenceEquals(replacement, child));
            }
            case ITuple:
            { //a boxed value tuple: its Item fields are rewritten on the box, which is then written back into the list
               object boxed = element;
               bool changed = false;
               foreach (FieldInfo item in boxed.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
               {
                  (object? updated, bool itemChanged) = CollapseElement(item.GetValue(boxed), visited);
                  if (itemChanged)
                  {
                     item.SetValue(boxed, updated);
                     changed = true;
                  }
               }
               return (boxed, changed);
            }
            default:
               return (element, false); //TokenNode and other non-node payload
         }
      }
      #endregion
   }
}
