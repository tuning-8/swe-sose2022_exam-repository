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
            List<HtmlNode> Dates = new List<HtmlNode>();
            List<HtmlNode> Categories = new List<HtmlNode>();
            List<HtmlNode> Dishes = new List<HtmlNode>();
            List<HtmlNode> DishTitles = new List<HtmlNode>();
            List<HtmlNode> DishDescriptions = new List<HtmlNode>();
            List<HtmlNode> DishIngredients = new List<HtmlNode>();
            List<HtmlNode> DishPrices = new List<HtmlNode>();

            //print all dates of this week
            Dates = testParser.getNodesByAttribute(testParser.rootNode, "date");
            foreach (HtmlNode date in Dates)
            {
                Console.WriteLine(date.GetAttributeValue("date", "").Trim());
            }

            //print all categories of the first date of the week
            Categories = testParser.getNodesByAttribute(Dates[0], "class", "category ");
            foreach (HtmlNode category in Categories)
            {
                Console.WriteLine(category.GetDirectInnerText().Trim());
            }

            //print all dishes (with ingredients and prices) of first category of the first date of the week
            Dishes = testParser.getNodesByNodeBounds(Dates[0], Categories[0], Categories[1], "class", "row");
            foreach (HtmlNode dish in Dishes)
            {
                DishTitles = testParser.getNodesByAttribute(dish, "class", "menu_title");
                foreach (HtmlNode dishTitle in DishTitles)
                {
                    Console.WriteLine(dishTitle.GetDirectInnerText().Trim());
                }
                DishDescriptions = testParser.getNodesByAttribute(dish, "class", "description");
                foreach (HtmlNode dishDescription in DishDescriptions)
                {
                    Console.WriteLine(dishDescription.ChildNodes[1].GetDirectInnerText().Trim());
                    DishIngredients = testParser.getNodesByAttribute(dishDescription, "class", "characterisation");
                    DishIngredients = testParser.getNodesByAttribute(DishIngredients[0], "title");
                    foreach (HtmlNode dishIngredient in DishIngredients)
                    {
                        Console.WriteLine(dishIngredient.GetDirectInnerText().Trim());
                    }

                    DishPrices = testParser.getNodesByAttribute(dishDescription, "class", "price");
                    DishPrices = testParser.getNodesByAttribute(DishPrices[0], "title");
                    foreach (HtmlNode dishPriceClass in DishPrices)
                    {
                        Console.WriteLine(dishPriceClass.GetDirectInnerText().Trim());
                    }
                }
            }
        }
    }
}