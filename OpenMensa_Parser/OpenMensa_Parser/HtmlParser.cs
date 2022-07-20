using System;
using HtmlAgilityPack;

namespace OpenMensa_Parser
{
    public class HtmlParser
    {
        public string htmlURL {get; private set;}
        HtmlWeb web;
        public HtmlDocument htmlDoc {get; private set;}
        HtmlNode rootNode;

        //method to parse a single node, where the attribute name 'class' has the parameter 'className' as attribute value
        public HtmlAgilityPack.HtmlNode? getNodeByClassName(string className)
        {
            HtmlAgilityPack.HtmlNode? Node = null;
            foreach (var subMenuNode in this.rootNode.ChildNodes)
            {
                if (subMenuNode.NodeType == HtmlNodeType.Element)
                {
                    if (subMenuNode.GetAttributeValue("class", "") == className)
                    {
                        Console.WriteLine(subMenuNode.GetAttributeValue("date", ""));
                        Node = htmlDoc.DocumentNode.SelectSingleNode((subMenuNode.XPath).ToString());
                    }
                }
            }
            return Node;
        }

        //function to print the inner text of a node with Null exception handling (only for test purpose)
        public void printInnerText(HtmlNode node, string def)
        {
            try
            {
                Console.WriteLine((node.GetDirectInnerText()).Trim());
            } catch (System.NullReferenceException)
            {
                Console.WriteLine(def);
            }
        }

        public HtmlParser(string url, string node)
        {
            this.htmlURL = url;
            this.web = new HtmlWeb();
            this.htmlDoc = web.Load(url);
            this.rootNode = htmlDoc.DocumentNode.SelectSingleNode(node);
        }
    }
}