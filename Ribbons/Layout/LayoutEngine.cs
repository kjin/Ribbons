using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ribbons.Content;

namespace Ribbons.Layout
{
    public static class LayoutEngine
    {
        public static Text SaveLayout(LayoutTree layout)
        {
            List<string> strings = new List<string>();
            SaveNode(layout.Root, strings, "");
            return new Text(strings.ToArray());
        }

        static void SaveNode(LayoutTreeNode node, List<string> strings, string indent)
        {
            strings.Add(String.Format("{0}{1}{2} = {3}{4}",
                                indent,
                                node.Key,
                                node.KeyExtension != null ? String.Format("[{0}]", node.KeyExtension) : "",
                                node.Value,
                                node.Children.Count == 0 ? ";" : ""));
            if (node.Children.Count > 0)
            {
                strings.Add(String.Format("{0}(", indent));
                string newIndent = indent + "    ";
                foreach (LayoutTreeNode n in node.Children)
                    SaveNode(n, strings, newIndent);
                strings.Add(String.Format("{0})", indent));
            }
        }

        public static LayoutTree BuildLayout(Text source)
        {
            int length = source.Length;
            StringBuilder processed = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                String preprocessed = source[i];
                // State flags
                bool quote = false;
                bool array = false;
                bool script = false;
                bool comment = false;
                bool followsRightBracket = false;
                // Filter out all non-quote whitespace, turn
                // semicolons into empty brackets, and clear comments.
                for (int j = 0; j < preprocessed.Length; j++)
                {
                    if (quote && preprocessed[j] != '\"')
                    {
                        processed.Append(preprocessed[j]);
                        continue;
                    }
                    switch (preprocessed[j])
                    {
                        case '\"':
                            quote = !quote;
                            break;
                        case '[':
                            array = true;
                            processed.Append('[');
                            break;
                        case ']':
                            array = false;
                            processed.Append(']');
                            break;
                        case ' ':
                            break;
                        case ',':
                            if (array)
                                processed.Append(',');
                            else if (!followsRightBracket)
                                processed.Append("()");
                            break;
                        case ')':
                            processed.Append("())");
                            followsRightBracket = true;
                            break;
                        case '#':
                            comment = true;
                            break;
                        default:
                            processed.Append(preprocessed[j]);
                            followsRightBracket = false;
                            break;
                    }
                    if (comment)
                        break;
                }
#if DEBUG
                if (quote)
                    Console.WriteLine("LayoutEngine WARNING: No matching endquote on line {0}.", i);
#endif
            }
            // Dummy node because the root has not yet been born
            LayoutTreeNode dummy = new LayoutTreeNode("dummy", "dummy");
            Parse(processed.ToString(), dummy);
            LayoutTree layoutTree = new LayoutTree();
            layoutTree.Root = dummy.Children.First.Value;
            Console.WriteLine(layoutTree.RepresentativeString);
#if DEBUG
            if (dummy.Children.Count > 1)
                Console.WriteLine("LayoutEngine WARNING: {0} top-level nodes found... the last {1} will be ignored.", dummy.Children.Count, dummy.Children.Count - 1);
#endif
            return layoutTree;
        }

        static void Parse(String sourceString, LayoutTreeNode node)
        {
            List<String> cSourceStrings = new List<string>();
            // Make the child source strings
            int bracketLevel = 0;
            int startIndex = 0;
            int length = 0;
            for (int i = 0; i < sourceString.Length; i++)
            {
                length++;
                if (sourceString[i] == '(')
                    bracketLevel++;
                else if (sourceString[i] == ')')
                {
                    // We're about to close the first bracket, so do stuff
                    if (bracketLevel == 1)
                    {
                        cSourceStrings.Add(sourceString.Substring(startIndex, length));
                        startIndex += length;
                        length = 0;
                    }
                    bracketLevel--;
                }
            }
            if (bracketLevel != 0)
            {
#if DEBUG
                Console.WriteLine("LayoutEngine WARNING: At least one mismatched bracket found in parent key {0}.", node.Key);
#endif
                return;
            }
            // Parse each child source string
            for (int i = 0; i < cSourceStrings.Count; i++)
            {
                string cSourceString = cSourceStrings[i];
                int equals = cSourceString.IndexOf('=');
                // Base case: No equals - no key/value
                if (equals == -1)
                    return;
                string key = cSourceString.Substring(0, equals);
                int leftBracket = cSourceString.IndexOf('(');
                int rightBracket = cSourceString.LastIndexOf(')');
                if (leftBracket == -1 || rightBracket == -1)
                {
#if DEBUG
                    Console.WriteLine("LayoutEngine WARNING: Key {0} is missing a bracket.", key);
#endif
                    return;
                }
                string value = cSourceString.Substring(equals + 1, leftBracket - equals - 1);
                LayoutTreeNode currentNode = new LayoutTreeNode(key, value);
                node.Children.AddLast(currentNode);
                Parse(cSourceString.Substring(leftBracket + 1, rightBracket - leftBracket - 1), currentNode);
            }
        }
    }
}
