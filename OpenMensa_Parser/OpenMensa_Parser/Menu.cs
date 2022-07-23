using System;
using HtmlAgilityPack;

namespace OpenMensa_Parser
{
    public class Menu
    {
        public List<Weekday> weekdayList = new List<Weekday>();
        public List<HtmlNode> weekdayNodes = new List<HtmlNode>();

        public void GenrateWeekdayInstances(HtmlParser testParser)
        {

        }
    }

    public class Weekday
    {
        public string Date { get; set; }
        public List<Category> categoryList = new List<Category>();
        public List<HtmlNode> categoryNodes = new List<HtmlNode>();

        public Weekday(HtmlNode weekdayNode)
        {

        }

        public void GenerateCategoryInstances()
        {

        }      
    }

    public class Category
    {
        public string Name { get; set; }
        public List<Dish> dishList = new List<Dish>();
        public List<HtmlNode> dishNodes = new List<HtmlNode>();

        public Category(HtmlNode categoryNode)
        {

        }

        public void GenerateDishInstances()
        {

        }
    }

    public class Dish
    {
        public string DishName { get; set; }
        public string[] Prices { get; set; }

        public List<int> specialIngredients = new List<int>();

        public Dish(HtmlNode dishNode)
        {
            
        }
    }
}