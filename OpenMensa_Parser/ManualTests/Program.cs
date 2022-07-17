using System;
using System.Xml;
using HtmlAgilityPack;

public class Program
{
    public static void Main (string[] args)
    {
        string html = "https://www.studentenwerk-freiberg.de/freiberg/essen-trinken/speiseplan/de/";
        //handler init
        HtmlWeb web = new HtmlWeb();
        //(down)load of website via specified url
        HtmlDocument htmlDoc = web.Load(html);

        /* current date */
        // select the root node / starting point of html document
        HtmlNode menuNode = htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[1]/div[3]/div[2]/div[3]/div[2]/div/div[4]");
        // collect date from the attribute of html nodes
        HtmlAgilityPack.HtmlNode? menuActiveDayNode = null;
        foreach (HtmlNode subMenuNode in menuNode.ChildNodes)
        {
            if (subMenuNode.NodeType == HtmlNodeType.Element)
            {
                if (subMenuNode.GetAttributeValue("class", "") == "menu_day active")
                {
                    Console.WriteLine(subMenuNode.GetAttributeValue("date", ""));
                    menuActiveDayNode = htmlDoc.DocumentNode.SelectSingleNode((subMenuNode.XPath).ToString());
                }
            }
        }

        /* child nodes of 'menu' */
        foreach (HtmlNode subMenuNode in menuActiveDayNode.ChildNodes)
        {
            if (subMenuNode.NodeType == HtmlNodeType.Element)
            {
                // collect categories from the attribute of html nodes
                if (subMenuNode.GetAttributeValue("class", "") == "category ")
                {
                    Console.WriteLine();
                    Console.WriteLine((subMenuNode.GetDirectInnerText()).Trim());
                }
                else if (subMenuNode.GetAttributeValue("class", "") == "row")
                {
                    string categoryXPath = subMenuNode.XPath;
                    HtmlNode categoryNode = htmlDoc.DocumentNode.SelectSingleNode(categoryXPath);
                    foreach (HtmlNode subCatNode in subMenuNode.ChildNodes)
                    {
                        if (subCatNode.NodeType == HtmlNodeType.Element)
                        {
                            if (subCatNode.GetAttributeValue("class", "") == "menu_title")
                            {
                                Console.WriteLine((subCatNode.GetDirectInnerText()).Trim());
                            }
                        }
                    }
                }
            }
        }

    }
}