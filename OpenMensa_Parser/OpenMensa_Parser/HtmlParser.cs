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

        //function to parse a list of nodes downwards from the 'ParentNode', where the nodes attribute name equals the parameter
        public List <HtmlAgilityPack.HtmlNode?> getNodesByAttribute(HtmlNode ParentNode, string AttributeName, string? AttributeValue)
        {
            List <HtmlAgilityPack.HtmlNode?> Nodes = new List <HtmlAgilityPack.HtmlNode?>();
            //iteration through all direct child nodes (one hiarchal level) of the 'ParentNode'
            foreach (var ChildNode in ParentNode.ChildNodes)
            {
                if (ChildNode.NodeType == HtmlNodeType.Element)
                {
                    //collect all attributes of the current child
                    var Attributes = ChildNode.GetAttributes();
                    foreach (var attribute in Attributes)
                    {
                        if (attribute.Name == AttributeName)
                        {
                            //returns the node, which has the proper 'AttributeValue' to the 'AttributeName'
                            if (AttributeValue != null)
                            {
                                if (ChildNode.GetAttributeValue(AttributeName, "") == AttributeValue)
                                {
                                    Nodes.Add(htmlDoc.DocumentNode.SelectSingleNode((ChildNode.XPath).ToString()));
                                }
                            }
                            //makeshift: if no specific attribute value is provided to the function it returns the first node matching the 'AttributeName'
                            //TODO: overload function
                            else
                            {
                                Nodes.Add(htmlDoc.DocumentNode.SelectSingleNode((ChildNode.XPath).ToString()));
                            }
                        }
                    }
                }
            }
            return Nodes;
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