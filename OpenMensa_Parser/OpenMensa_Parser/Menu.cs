using System;
using HtmlAgilityPack;

namespace OpenMensa_Parser
{
    public class Menu
    {
        HtmlParser parserInstance;
        List<Weekday> weekdayList = new List<Weekday>();
        List<HtmlNode> weekdayNodes = new List<HtmlNode>();

        //function to parse the dates -> weekdayNodes and create instances of Weekday -> weekdayList
        public void GenrateWeekdayInstances()
        {
            //get all dates (class = 'menu_day', date=dd.mm.yyyy) of the week as nodes
            this.weekdayNodes = this.parserInstance.getNodesByAttribute(this.parserInstance.rootNode, "date");

            //iterate through the date nodes
            for (int i = 0; i<weekdayNodes.Count; i++)
            {
                //create a new instance of a weekday class foreach date in the list
                //pass the parserInstance to the Weekday class
                weekdayList.Add(new Weekday(parserInstance, weekdayNodes[i]));
                //call the function parse the categories
                weekdayList[i].GenerateCategoryInstances();
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
        HtmlNode weekdayNode;
        public string Date { get; set; }
        public List<Category> categoryList = new List<Category>();
        public List<HtmlNode> categoryNodes = new List<HtmlNode>();

        public Weekday(HtmlParser Parser, HtmlNode weekdayNode)
        {
            this.parserInstance = Parser;
            this.weekdayNode = weekdayNode;
            //extract the date string from the node
            this.Date = weekdayNode.GetAttributeValue("date", "");
        }

        //function to parse the categories -> categoryNodes and create instances of Category -> categoryList
        public void GenerateCategoryInstances()
        {
            //get all categories of the current weekday as nodes
            this.categoryNodes = this.parserInstance.getNodesByAttribute(weekdayNode, "class", "category ");

            //iterate through the category nodes
            for (int i = 0; i<categoryNodes.Count; i++)
            {
                //pass the parserInstance to the Category class
                categoryList.Add(new Category(parserInstance, categoryNodes[i]));
                //call the function parse the categories
                categoryList[i].GenerateDishInstances();
            }
        }      
    }

    public class Category
    {
        HtmlParser parserInstance;
        HtmlNode categoryNode;
        public string Name { get; set; }
        public List<Dish> dishList = new List<Dish>();
        public List<HtmlNode> dishNodes = new List<HtmlNode>();

        public Category(HtmlParser Parser, HtmlNode categoryNode)
        {
            this.parserInstance = Parser;
            this.categoryNode = categoryNode;
            this.Name = categoryNode.GetDirectInnerText().Trim();
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