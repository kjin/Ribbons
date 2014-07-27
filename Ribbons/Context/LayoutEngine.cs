﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ribbons.Content;

namespace Ribbons.Context
{
    public class LayoutEngine
    {
        LayoutTree layoutTree;

        public LayoutEngine(Text source)
        {
            int length = source.Length;
            StringBuilder processed = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                String preprocessed = source[i];
                bool quote = false;
                bool comment = false;
                // Filter out all non-quote whitespace, turn
                // semicolons into empty brackets, and clear comments.
                for (int j = 0; j < preprocessed.Length; j++)
                {
                    switch (preprocessed[j])
                    {
                        case '\"':
                            quote = !quote;
                            break;
                        case ' ':
                            if (quote)
                                processed.Append(' ');
                            break;
                        case ';':
                            if (quote)
                                processed.Append(';');
                            else
                                processed.Append("{}");
                            break;
                        case '#':
                            comment = true;
                            break;
                        default:
                            processed.Append(preprocessed[j]);
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
            layoutTree = new LayoutTree();
            layoutTree.Root = dummy.Children.First.Value;
#if DEBUG
            if (dummy.Children.Count > 0)
                Console.WriteLine("LayoutEngine WARNING: {0} top-level nodes found... the last {1} will be ignored.", dummy.Children.Count, dummy.Children.Count - 1);
            
#endif
        }

        static private void Parse(String sourceString, LayoutTreeNode node)
        {
            List<String> cSourceStrings = new List<string>();
            // Make the child source strings
            int bracketLevel = 0;
            int startIndex = 0;
            int length = 0;
            for (int i = 0; i < sourceString.Length; i++)
            {
                length++;
                if (sourceString[i] == '{')
                    bracketLevel++;
                else if (sourceString[i] == '}')
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
                Console.WriteLine("LayoutEngine WARNING: At least one mismatched bracket found.");
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
                int leftBracket = cSourceString.IndexOf('{');
                int rightBracket = cSourceString.LastIndexOf('}');
                if (leftBracket == -1 || rightBracket == -1)
                {
#if DEBUG
                    Console.WriteLine("LayoutEngine WARNING: Key {0} is missing a bracket.", key);
#endif
                    return;
                }
                string value = cSourceString.Substring(equals, leftBracket - equals);
                LayoutTreeNode currentNode = new LayoutTreeNode(key, value);
                node.Children.AddLast(currentNode);
                Parse(sourceString.Substring(leftBracket, rightBracket - leftBracket), currentNode);
            }
        }
    }

    class LayoutTree
    {
        public LayoutTreeNode Root;
        public String RepresentativeString { get { if (!stringMade) MakeString(); return representativeString; } }
        String representativeString;
        bool stringMade = false;

        public void MakeString()
        {
            StringBuilder sb = new StringBuilder();
            Root.ToString(sb, "");
            representativeString = sb.ToString();
            stringMade = true;
        }
    }

    class LayoutTreeNode
    {
        public string Key;
        public string Value;
        public LinkedList<LayoutTreeNode> Children;

        public LayoutTreeNode(string key, string value)
        {
            Key = key;
            Value = value;
            Children = new LinkedList<LayoutTreeNode>();
        }

        public void ToString(StringBuilder sb, string prependedSpaces)
        {
            sb.AppendFormat("{0}{1} | {2}\r\n", prependedSpaces, Key, Value);
            string childPrependedSpaces = prependedSpaces + "    ";
            for (LinkedListNode<LayoutTreeNode> childNode = Children.First;
                 childNode.Next != null;
                 childNode = childNode.Next)
            {
                childNode.Value.ToString(sb, childPrependedSpaces);
            }
        }
    }
}