using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ribbons.Context
{
    public class LayoutTreeNode
    {
        public string Key;
        public string KeyExtension;
        public string Value;
        public LinkedList<LayoutTreeNode> Children;

        public LayoutTreeNode(string key, string value)
        {
            int leftBracketIndex = key.IndexOf('[');
            if (leftBracketIndex >= 0)
            {
#if DEBUG
                if (key.LastIndexOf(']') != key.Length - 1)
                    Console.WriteLine("LayoutEngine WARNING: Key {0} has '[' but doesn't end with ']'.");
#endif
                Key = key.Substring(0, leftBracketIndex);
                KeyExtension = key.Substring(leftBracketIndex + 1, key.Length - leftBracketIndex - 2);
            }
            else
            {
                Key = key;
                KeyExtension = null;
            }
            Value = value;
            Children = new LinkedList<LayoutTreeNode>();
        }

        public void ToString(StringBuilder sb, string prependedSpaces)
        {
            sb.AppendFormat("{0}{1} | {2}\r\n", prependedSpaces, Key, Value);
            string childPrependedSpaces = prependedSpaces + "    ";
            for (LinkedListNode<LayoutTreeNode> childNode = Children.First;
                 childNode != null;
                 childNode = childNode.Next)
            {
                childNode.Value.ToString(sb, childPrependedSpaces);
            }
        }
    }
}
