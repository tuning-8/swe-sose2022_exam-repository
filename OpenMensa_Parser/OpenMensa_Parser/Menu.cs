using System;
using HtmlAgilityPack;

namespace OpenMensa_Parser
{
    public class Menu
    {
        HtmlParser parserInstance;
        List<Weekday> weekdayList = new List<Weekday>();
        List<HtmlNode> weekdayNodes = new List<HtmlNode>();

        public void GenrateWeekdayInstances()
        {
            this.weekdayNodes = this.parserInstance.getNodesByAttribute(this.parserInstance.rootNode, "date");

            foreach(HtmlNode weekdayNode in weekdayNodes)
            {
                weekdayList.Add(new Weekday(parserInstance, weekdayNode));
            }
        }

        public Menu (HtmlParser Parser)
        {
            this.parserInstance = Parser;
        }
    }

    public class Weekday
    {
        HtmlParser parserInstance;
        public string Date { get; set; }
        public List<Category> categoryList = new List<Category>();
        public List<HtmlNode> categoryNodes = new List<HtmlNode>();

        public Weekday(HtmlParser Parser, HtmlNode weekdayNode)
        {
            this.parserInstance = Parser;
            this.Date = weekdayNode.GetAttributeValue("date", "");
        }

        public void GenerateCategoryInstances()
        {

        }      
    }

    public class Category
    {
        HtmlParser parserInstance;
        public string Name { get; set; }
        public List<Dish> dishList = new List<Dish>();
        public List<HtmlNode> dishNodes = new List<HtmlNode>();

        public Category(HtmlParser Parser, HtmlNode categoryNode)
        {
            this.parserInstance = Parser;

        }

        public void GenerateDishInstances()
        {

        }
    }

    public class Dish
    {
        HtmlParser parserInstance;
        public string DishName { get; set; }
        public string[] Prices { get; set; }

        public List<int> specialIngredients = new List<int>();

        public Dish(HtmlParser Parser, HtmlNode dishNode)
        {
            this.parserInstance = Parser;
        }
    }
}