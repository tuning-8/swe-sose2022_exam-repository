/**
 * @file
 * @author  tuning-8 <tuning_8@gmx.de>
 * @version 1.15
 *
 * @section LICENSE
 *
 * Licence information can be found in README.me (https://github.com/tuning-8/swe-sose2022_exam-repository/blob/main/README.md)
 *
 * @section DESCRIPTION
 *
 * File that includes the 'HtmlParser' class with all methods needed to parse HTML nodes.
 */

using System;
using HtmlAgilityPack;

namespace OpenMensa_Parser
{
    /**
     * @brief Class that contains methods to parse HTML nodes from a website.
     *
     *
     */
    public class HtmlParser
    {
        public string htmlURL {get; private set;}

        HtmlWeb web;
        public HtmlDocument htmlDoc {get; private set;}
        public HtmlNode rootNode {get; private set;}

        /**
         * @brief   Method to parse a list of nodes
         *
         * @details A method that parses every HTML Node that has one attribute where the attributes name is equal to the parameter.
         *          The hirarchical level (starting point) is called parent. All attributes that are exactly one level below the parent
                    (so called childs) will be checked for the conditions.
         *
         * @param[in]   parentNode      HTML node (and corresponding level) where the parser starts
         * @param[in]   attributeName   Name of the attribute where all nodes are checked against (e.g. "class", "title")
         * @return      List of HTML nodes that are matching the condition
         *
         */
        public List <HtmlAgilityPack.HtmlNode?> GetNodesByAttribute(HtmlNode parentNode, string attributeName)
        {
            List <HtmlAgilityPack.HtmlNode?> Nodes = new List <HtmlAgilityPack.HtmlNode?>();
            //iteration through all direct child nodes (one hiarchal level below parent)
            foreach (var ChildNode in parentNode.ChildNodes)
            {
                if (ChildNode.NodeType == HtmlNodeType.Element)
                {
                    //collect all attributes of the current child
                    var Attributes = ChildNode.GetAttributes();
                    foreach (var attribute in Attributes)
                    {
                        //append the nodes to the list if the attribute name matches the parameter
                        if (attribute.Name == attributeName)
                        {
                            Nodes.Add(htmlDoc.DocumentNode.SelectSingleNode((ChildNode.XPath).ToString()));
                        }
                    }
                }
            }
            return Nodes;
        }

        /**
         * @brief   Method to parse a list of nodes
         *
         * @details A method that parses every HTML Node that has one attribute where the attributes name and
         *          its attribute value are equal to the parameters. The hirarchical level (starting point) is called parent.
         *          All attributes that are exactly one level below the parent (so called childs) will be checked for the conditions.
         *
         * @param[in]   parentNode      HTML node (and corresponding level) where the parser starts
         * @param[in]   attributeName   Name of the attribute where all nodes are checked against (e.g. "class", "title")
         * @param[in]   attributeValue  Value that the attribute must contain (e.g. "21.07.22", "category", "row")
         * @return      List of HTML nodes that are matching the conditions
         *
         */
        public List <HtmlAgilityPack.HtmlNode?> GetNodesByAttribute(HtmlNode parentNode, string attributeName, string attributeValue)
        {
            List <HtmlAgilityPack.HtmlNode?> Nodes = new List <HtmlAgilityPack.HtmlNode?>();
            //iteration through all direct child nodes (one hiarchal level) of the 'ParentNode'
            foreach (var ChildNode in parentNode.ChildNodes)
            {
                if (ChildNode.NodeType == HtmlNodeType.Element)
                {
                    //collect all attributes of the current child
                    var Attributes = ChildNode.GetAttributes();
                    foreach (var attribute in Attributes)
                    {
                        if (attribute.Name == attributeName 
                            && ChildNode.GetAttributeValue(attributeName, "") == attributeValue)
                            {
                                Nodes.Add(htmlDoc.DocumentNode.SelectSingleNode((ChildNode.XPath).ToString()));
                            }
                    }
                }
            }
            return Nodes;
        }

        /**
         * @brief   Method to parse a list of nodes
         *
         * @details A method that parses every HTML node that has one attribute where the attributes name and
         *          its attribute value are equal to the parameters and is between two equal "bound" attributes
         *          that are on the same level as the wanted node. The hirarchical level (starting point) is called parent.
         *          All attributes that are exactly one level below the parent (so called childs) will be checked for the conditions.
         *
         * @note    Example:
         *          <div class="category" .....>    -> first bound node (axclusive)
         *          <div class="row" ....>          -> returned node
         *          <div class="row" ....>          -> returned node
         *          <div class="row" ....>          -> returned node
         *          <div class="category" .....>    -> second bound node (exclusive)
         *
         * @param[in]   parentNode      HTML node (and corresponding level) where the parser starts
         * @param[in]   firstBoundNode  HTML node on the same level that marks the start of surrounging
         * @param[in]   secondBoundNode HTML node on the same level that marks the end of surrouding
         * @param[in]   attributeName   Name of the attribute where all nodes are checked against (e.g. "class", "title")
         * @param[in]   attributeValue  Value that the attribute must contain (e.g. "21.07.22", "category", "row")
         * @return      List of HTML nodes that are matching the conditions
         *
         */
        public List <HtmlAgilityPack.HtmlNode?> GetNodesByNodeBounds(HtmlNode parentNode, HtmlNode firstBoundNode,
            HtmlNode secondBoundNode, string attributeName, string attributeValue)
        {
            List <HtmlAgilityPack.HtmlNode?> Nodes = new List <HtmlAgilityPack.HtmlNode?>();
            //get the indexes of the bound nodes
            int indexFirtBound = parentNode.ChildNodes.IndexOf(firstBoundNode);
            int indexSecondBound = parentNode.ChildNodes.IndexOf(secondBoundNode);
            //iterate through the nodes that are between the bounds
            for (int i = indexFirtBound + 1; i<indexSecondBound; i++)
            {
                //add all nodes that are HtmlNodes (not HtmlTextNodes)
                if (parentNode.ChildNodes[i].NodeType == HtmlNodeType.Element)
                {
                    Nodes.Add(parentNode.ChildNodes[i]);
                }
            }
            return Nodes;
        }

        /**
         * @brief   Constructor that instances the 'Html Agility Pack' and loads the website
         *
         * @params[in]  url     URL to the canteens menu
         * @params[in]  node    Root node (highest hirarchical level) from where the menu is listed on the website
         */
        public HtmlParser(string url, string node)
        {
            this.htmlURL = url;
            this.web = new HtmlWeb();
            this.htmlDoc = web.Load(url);
            this.rootNode = htmlDoc.DocumentNode.SelectSingleNode(node);
        }
    }
}