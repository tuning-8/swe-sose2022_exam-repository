using System;
using System.Xml;
using HtmlAgilityPack;

namespace OpenMensa_Parser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //global variables
            string entry_node = "/html/body/div[1]/div[3]/div[2]/div[3]/div[2]/div/div[4]";
            string[] studentServiceURLs = new string[]
            {
                "https://www.studentenwerk-freiberg.de/freiberg/essen-trinken/speiseplan/de/woche/this/",
                "https://www.studentenwerk-freiberg.de/freiberg/essen-trinken/speiseplan/de/woche/next/",
            };
            foreach (string url in studentServiceURLs)
            {
                HtmlParser menuParser = new HtmlParser(url, entry_node);
                Menu menu = new Menu(menuParser);
                menu.GenrateWeekdayInstances();
            }
        }
    }
}