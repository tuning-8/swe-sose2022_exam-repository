using System;
using HtmlAgilityPack;

namespace OpenMensa_Parser
{
    public class HtmlParser
    {
        public string htmlURL {get; private set;}

        HtmlWeb web;
        public HtmlDocument htmlDoc {get; private set;}
        public HtmlNode rootNode {get; private set;}

        //function to parse a list of nodes downwards from the 'ParentNode', where the nodes attribute name equals the parameter
        public List <HtmlAgilityPack.HtmlNode?> getNodesByAttribute(HtmlNode ParentNode, string AttributeName)
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
                            Nodes.Add(htmlDoc.DocumentNode.SelectSingleNode((ChildNode.XPath).ToString()));
                        }
                    }
                }
            }
            return Nodes;
        }

        /* function to parse a list of nodes downwards from the 'ParentNode', where the nodes attribute name 
         * and the nodes attribte value equal the parameters
         */
        public List <HtmlAgilityPack.HtmlNode?> getNodesByAttribute(HtmlNode ParentNode, string AttributeName, string AttributeValue)
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
                        if (attribute.Name == AttributeName 
                            && ChildNode.GetAttributeValue(AttributeName, "") == AttributeValue)
                            {
                                Nodes.Add(htmlDoc.DocumentNode.SelectSingleNode((ChildNode.XPath).ToString()));
                            }
                    }
                }
            }
            return Nodes;
        }


        /*
         * function to parse a list of nodes downwards from the 'ParentNode', where the nodes are between two equal "bound attributes"
         *
         * e.g.: "category" -> bound attribute; "row" -> returned nodes
         *
         * <div class="category" .....>
         * <div class="row" ....> 
         * <div class="row" ....> 
         * <div class="row" ....>
         * <div class="category" .....>
         *
         */
        public List <HtmlAgilityPack.HtmlNode?> getNodesByXPathBounds(HtmlNode ParentNode, string BoundAttributeName, string AttributeName)
        {
            //TODO: implement functionality
            return new List<HtmlAgilityPack.HtmlNode?>();
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