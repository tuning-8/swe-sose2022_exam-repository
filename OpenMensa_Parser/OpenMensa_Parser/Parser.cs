using System;
using System.Xml;
using HtmlAgilityPack;

namespace OpenMensa_Parser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //manual tests for HtmlParser class:
            string menu_url = "https://www.studentenwerk-freiberg.de/freiberg/essen-trinken/speiseplan/de/woche/this/";
            string entry_node = "/html/body/div[1]/div[3]/div[2]/div[3]/div[2]/div/div[4]";
            HtmlParser testParser = new HtmlParser(menu_url, entry_node);
            HtmlNode menuNode = testParser.getNodeByClassName("menu_day active");
            testParser.printInnerText(menuNode, "");
        }
    }
}