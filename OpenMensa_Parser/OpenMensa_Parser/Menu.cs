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
                try
                {
                    categoryList.Add(new Category(parserInstance, categoryNodes[i], categoryNodes[i+1]));
                }
                //the second bound for the last category is no category node but the last child of the date node
                catch (System.ArgumentOutOfRangeException)
                {
                    categoryList.Add(new Category(parserInstance, categoryNodes[i], weekdayNode.LastChild));
                }
                //call the function parse the categories
                categoryList[i].GenerateDishInstances();
            }
        }      
    }

    public class Category
    {
        HtmlParser parserInstance;
        HtmlNode categoryNode;
        HtmlNode nextCategoryNode;
        public string Name { get; set; }
        public List<Dish> dishList = new List<Dish>();
        public List<HtmlNode> dishNodes = new List<HtmlNode>();

        public Category(HtmlParser Parser, HtmlNode categoryNode, HtmlNode nextCategoryNode)
        {
            this.parserInstance = Parser;
            this.categoryNode = categoryNode;
            this.nextCategoryNode =nextCategoryNode;
            //extract the category string from the node
            this.Name = categoryNode.GetDirectInnerText().Trim();
        }

        //function to parse the dishes -> dishNodes and create instances of Dish -> dishList
        public void GenerateDishInstances()
        {
            //get all dishes of the current category as nodes
            this.dishNodes = this.parserInstance.getNodesByNodeBounds(categoryNode.ParentNode, categoryNode, nextCategoryNode, "class", "row");
            //iterate through the dish nodes
            for (int i = 0; i<dishNodes.Count; i++)
            {
                dishList.Add(new Dish(parserInstance, dishNodes[i]));
            }
        }
    }

    public class Dish
    {
        HtmlParser parserInstance;
        HtmlNode dishNode;
        public string DishName { get; set; }
        public string[] Prices { get; set; }

        public List<string> specialIngredients = new List<string>();

        public Dish(HtmlParser Parser, HtmlNode dishNode)
        {
            this.Prices = new string[4];
            this.parserInstance = Parser;
            this.dishNode = dishNode;

            foreach (HtmlNode dishTitle in this.parserInstance.getNodesByAttribute(this.dishNode, "class", "menu_title"))
            {
                this.DishName = dishTitle.GetDirectInnerText().Trim();
            }
            foreach (HtmlNode dishDescription in this.parserInstance.getNodesByAttribute(this.dishNode, "class", "description"))
            {
                if (dishDescription.ChildNodes[1].GetDirectInnerText().Trim() != "")
                {
                    this.DishName += " - " + dishDescription.ChildNodes[1].GetDirectInnerText().Trim();
                }
                foreach (HtmlNode dishCharacterisation in this.parserInstance.getNodesByAttribute(dishDescription, "class", "characterisation"))
                {
                    foreach (HtmlNode dishIngredient in this.parserInstance.getNodesByAttribute(dishCharacterisation, "title"))
                    {
                        this.specialIngredients.Add(dishIngredient.GetDirectInnerText().Trim());
                    }
                }
                foreach (HtmlNode dishPrice in this.parserInstance.getNodesByAttribute(dishDescription, "class", "price"))
                {
                    foreach (HtmlNode dishPriceClass in this.parserInstance.getNodesByAttribute(dishPrice, "title"))
                    {
                        this.specialIngredients.Add(dishPriceClass.GetDirectInnerText().Trim());
                    }
                }
            }
        }
    }
}