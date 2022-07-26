/**
 * @file
 * @author  tuning-8 <tuning_8@gmx.de>, nicoschurig
 * @version 1.8
 *
 * @section LICENSE
 *
 * Licence information can be found in README.me (https://github.com/tuning-8/swe-sose2022_exam-repository/blob/main/README.md)
 *
 * @section DESCRIPTION
 *
 * File that includes classes and methods to store the infomation provided by the HTML parser and provide a data basis for the XML writer.
 */
 
using System;
using HtmlAgilityPack;

namespace OpenMensa_Parser
{
    /**
     * @brief   Class stores a List of 'Weekday' class instances
     */
    public class Menu
    {
        private HtmlParser _parserInstance;
        public List<Weekday> WeekdayList { get; private set; } = new List<Weekday>();
        private List<HtmlNode> _weekdayNodes = new List<HtmlNode>();

        /**
         * @brief   Constructor that takes over a instance of a HTML parser
         *
         * @param[in]   parser  instance of a HtmlParser class
         */
        public Menu (HtmlParser parser)
        {
            this._parserInstance = parser;
        }

        /**
         * @brief   Method to parse the dates and create instances of Weekday class
         *
         * @details A method that parses the nodes for each date that has a menu available. For each node
         *          a new instance of the Weekday class is stored in the WeekdayList.
         */
        public void GenrateWeekdayInstances()
        {
            // get all dates (class = 'menu_day', date=dd.mm.yyyy) of the week as nodes
            this._weekdayNodes = this._parserInstance.GetNodesByAttribute(this._parserInstance.rootNode, "date");

            // iterate through the date nodes
            for (int i = 0; i<_weekdayNodes.Count; i++)
            {
                // create a new instance of a weekday class foreach date in the list
                // pass the _parserInstance to the Weekday class
                WeekdayList.Add(new Weekday(_parserInstance, _weekdayNodes[i]));
                // call the function parse the categories
                WeekdayList[i].GenerateCategoryInstances();
            }
        }
    }

    /**
     * @brief   Class stores a List of 'Category' class instances
     */
    public class Weekday
    {
        private HtmlParser _parserInstance;
        private HtmlNode _weekdayNode;
        public string Date { get; private set; }
        public List<Category> CategoryList { get; private set; } = new List<Category>();
        private List<HtmlNode> _categoryNodes = new List<HtmlNode>();

        /**
         * @brief   Constructor that takes over a HTML node that functions as new root node and extracts date information
         *
         * @param[in]   parser      instance of a HtmlParser class
         * @param[in]   weekdayNode HTML node that contains the menu for a weekday (structured in categories)
         */
        public Weekday(HtmlParser parser, HtmlNode weekdayNode)
        {
            this._parserInstance = parser;
            this._weekdayNode = weekdayNode;
            // extract the date string from the node
            this.Date = weekdayNode.GetAttributeValue("date", "");
        }

        /**
         * @brief   Method to parse the categories and create instances of Category
         *
         * @details A method that parses the nodes for each category that one day contains. For each node
         *          a new instance of the Category class is stored in the CategoryList.
         */
        public void GenerateCategoryInstances()
        {
            // get all categories of the current weekday as nodes
            this._categoryNodes = this._parserInstance.GetNodesByAttribute(_weekdayNode, "class", "category ");

            // iterate through the category nodes
            for (int i = 0; i<_categoryNodes.Count; i++)
            {
                try
                {
                    CategoryList.Add(new Category(_parserInstance, _categoryNodes[i], _categoryNodes[i+1]));
                }
                // the second bound for the last category is no category node but the last child of the date node
                catch (System.ArgumentOutOfRangeException)
                {
                    CategoryList.Add(new Category(_parserInstance, _categoryNodes[i], _weekdayNode.LastChild));
                }
                // call the function parse the categories
                CategoryList[i].GenerateDishInstances();
            }
        }      
    }

    /**
     * @brief   Class stores a List of 'Dish' class instances
     */
    public class Category
    {
        private HtmlParser _parserInstance;
        private HtmlNode _categoryNode;
        private HtmlNode _nextCategoryNode;
        public string Name { get; private set; }
        public List<Dish> DishList { get; private set; } = new List<Dish>();
        private List<HtmlNode> _dishNodes = new List<HtmlNode>();

        /**
         * @brief   Constructor that takes over a HTML node that functions as new root node and extracts category name
         *
         * @param[in]   parser              instance of a HtmlParser class
         * @param[in]   categoryNode        HTML node that is the starting point of the dedicated dish nodes
         * @param[in]   nextCategoryNode    HTML node that is the end point of the dedicated dish nodes
         *
         */
        public Category(HtmlParser parser, HtmlNode categoryNode, HtmlNode nextCategoryNode)
        {
            this._parserInstance = parser;
            this._categoryNode = categoryNode;
            this._nextCategoryNode =nextCategoryNode;
            // extract the category string from the node
            this.Name = categoryNode.GetDirectInnerText().Trim();
        }

        /**
         * @brief   Method to parse the dishes and create instances of Dish
         *
         * @details A method that parses the nodes for each dish that one category contains. For each node
         *          a new instance of the Dish class is stored in the DishList.
         */
        public void GenerateDishInstances()
        {
            // get all dishes of the current category as nodes
            this._dishNodes = this._parserInstance.GetNodesByNodeBounds(_categoryNode.ParentNode, _categoryNode, _nextCategoryNode, "class", "row");
            // iterate through the dish nodes
            for (int i = 0; i<_dishNodes.Count; i++)
            {
                DishList.Add(new Dish(_parserInstance, _dishNodes[i]));
            }
        }
    }

    /**
     * @brief   Class stores information about a dish
     */
    public class Dish
    {
        private HtmlParser _parserInstance;
        private HtmlNode _dishNode;
        public string DishName { get; private set; }
        public string[] Prices { get; private set; }

        public List<string> SpecialIngredients = new List<string>();

        /**
         * @brief   Constructor that takes over a HTML node that functions as new root node and extracts all information of a dish
         *
         * @param[in]   parser      instance of a HtmlParser class
         * @param[in]   dishNode    HTML node that contains all information of one dish
         *
         */
        public Dish(HtmlParser parser, HtmlNode dishNode)
        {
            this.Prices = new string[4];
            this._parserInstance = parser;
            this._dishNode = dishNode;

            // GetNodesByAttribute returns allways a list -> iteration through one element -> TODO: improvements needed
            foreach (HtmlNode dishTitle in this._parserInstance.GetNodesByAttribute(this._dishNode, "class", "menu_title"))
            {
                this.DishName = dishTitle.GetDirectInnerText().Trim();
            }
            foreach (HtmlNode dishDescription in this._parserInstance.GetNodesByAttribute(this._dishNode, "class", "description"))
            {
                // add the dish description to the title of the dish
                if (dishDescription.ChildNodes[1].GetDirectInnerText().Trim() != "")
                {
                    this.DishName += " - " + dishDescription.ChildNodes[1].GetDirectInnerText().Trim();
                }
                // parent node for the ingredients (one element list as well)
                foreach (HtmlNode dishCharacterisation in this._parserInstance.GetNodesByAttribute(dishDescription, "class", "characterisation"))
                {
                    // write all special ingredients to the list
                    foreach (HtmlNode dishIngredient in this._parserInstance.GetNodesByAttribute(dishCharacterisation, "title"))
                    {
                        this.SpecialIngredients.Add(dishIngredient.GetDirectInnerText().Trim());
                    }
                }
                // parent node for the prices (one element list as well)
                foreach (HtmlNode dishPrice in this._parserInstance.GetNodesByAttribute(dishDescription, "class", "price"))
                {
                    // indexer for the price array -> additionally needed because of foreach loop (but in this case better than for)
                    int priceIndexer=0;
                    foreach (HtmlNode dishPriceClass in this._parserInstance.GetNodesByAttribute(dishPrice, "title"))
                    {
                        // write all prices to the array
                        this.Prices[priceIndexer] = dishPriceClass.GetDirectInnerText().Trim();
                        priceIndexer++;
                    }
                }
            }
        }
    }
}