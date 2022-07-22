using System;
using System.Text;
using System.Xml;
using System.Globalization;

namespace XmlWriter
{
    static class Menu
    {
        public static List<Weekday> WeekdayList { get; set; }
    }

    public class Weekday
    {
        public string Date { get; set; }

        public List<Category> CategoryList { get; set; }
    }

    public class Category
    {
        public string? Name { get; set; }
        public List<Dish>? DishList { get; set; }
    }

    public class Dish
    {
        public string? DishName { get; set; }
        public string[]? Prices { get; set; }
        public List<int>? SpecialIngredients { get; set; }
    }

    static class IngredientsTranslator
    {
        private static Dictionary<int, string> IngredientsByIntIndicator = new Dictionary<int, string>(){
            {1,"mit Konservierungsstoffen"},
            {2, "mit Farbstoff"},
            {3, "gewachst"},
            {4, "geschwärzt"},
            {5, "mit Antioxidationsmittel"},
            {6, "mit Phosphat"},
            {7, "geschwefelt"},
            {8, "mit Süßungsmitteln"},
            {9, "mit Geschmacksverstärker"},
            {10, "phenylalaninhaltig"},
            {16, "chininhaltig"},
            {18, "Weichtiere"},
            {19, "glutenhaltig"},
            {20, "Krebstiere"},
            {21, "eihaltig"},
            {22, "Erdnüsse"},
            {23, "Soja"},
            {24, "Milch/Milchzucker"},
            {25, "Schalenfrüchte/Nüsse"},
            {26, "Sellerie"},
            {27, "Senf"},
            {28, "Sesamsamen"},
            {29, "Schwefeldioxid u. Sulfite"},
            {30, "Fisch (Allergen)"},
            {31, "Lupine"},
            {35, "mit Azofarbstoff"},
            {43, "mit kakaohaltiger Fettglasur"},
            {171, "ungeeignet für Vegetarier"}
        };
        private static Dictionary<string, string> IngredientsByStringIndicator = new Dictionary<string, string>(){
            {"WEI", "Weizen"},
            {"ROG", "Roggen"},
            {"GER", "Gerste"},
            {"HAF", "Hafer"},
            {"DIN", "Dinkel"},
            {"KAM", "Kamut"},
            {"MAN", "Mandeln"},
            {"HAS", "Haselnüsse"},
            {"WAL", "Walnüsse"},
            {"CAS", "Cashewnüsse"},
            {"PEK", "Pekanüsse"},
            {"PAR", "Paranüsse"},
            {"PIS", "Pistazien"},
            {"MAC", "Macadamia"}
        };

        public static string TranslateIngredientIndicator(int indicatorInt)
        {
            return IngredientsByIntIndicator[indicatorInt];
        }   

        public static string TranslateIngredientIndicator(string indicatorString)
        {
            return  IngredientsByStringIndicator[indicatorString];
        }
    }

    static class XmlWriter
    {
        public static string fileName = "OpenmenMensaXml.xml";
        public static string parserVersion = "1.01-1";
        public static string feedInformation = "http://openmensa.org/open-mensa-v2";
        public static string schemaInstance = "http://www.w3.org/2001/XMLSchema-instance";
        public static string schemaLocation = "http://openmensa.org/open-mensa-v2.xsd";

        public static int categoryCounter = 0;
        public static int dishCounter = 0;
        public static int priceCounter = 0;

        public static string[] roleNames = new string[] {"student", "employee", "other", "pupil"};
        public static char[] removedCharacters = new char[] {'€', ' '};

        public static void WriteXmlFile()
        {
            XmlTextWriter xmlWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;

            WriteOpenMensaStandardInformation(xmlWriter);
            WriteMenuInformation(xmlWriter);
        }

        public static void WriteOpenMensaStandardInformation(XmlTextWriter xmlWriter)
        {            
            //Writing process:
            xmlWriter.WriteStartDocument();                 //writes: <?xml version="1.0" encoding="utf-8"?>
            xmlWriter.WriteStartElement("openmensa");
            xmlWriter.WriteAttributeString("version", "2.1");
            xmlWriter.WriteAttributeString("xmlns", feedInformation);
            xmlWriter.WriteAttributeString("xmlns", "xsi", null, schemaInstance);
            xmlWriter.WriteAttributeString("xsi", "schemaLocation", null, feedInformation + " " + schemaLocation);
            xmlWriter.WriteStartElement("version");
            xmlWriter.WriteString(parserVersion);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("canteen");
        }

        public static void WritePriceInformation(Weekday _weekday, XmlTextWriter xmlWriter)
        {
            foreach(string roleName in roleNames)
            {
                xmlWriter.WriteStartElement("price");
                xmlWriter.WriteAttributeString("role", roleName);
                xmlWriter.WriteString(_weekday.CategoryList[categoryCounter].DishList[dishCounter].Prices[priceCounter].Replace(",", ".").TrimEnd(removedCharacters));
                xmlWriter.WriteEndElement();
                priceCounter++;
            }
            priceCounter = 0;
        }

        public static void WriteMenuInformation(XmlTextWriter xmlWriter)
        {
            foreach(Weekday _weekday in Menu.WeekdayList)
            {
                DateTime dateTime = DateTime.Parse(_weekday.Date);

                xmlWriter.WriteStartElement("day");
                xmlWriter.WriteAttributeString("date", dateTime.ToString("yyyy'-'MM'-'dd"));

                foreach(Category category in _weekday.CategoryList)
                {
                    xmlWriter.WriteStartElement("category");
                    xmlWriter.WriteAttributeString("name", category.Name);
                    foreach(Dish dish in _weekday.CategoryList[categoryCounter].DishList)
                    {
                        xmlWriter.WriteStartElement("meal");
                        xmlWriter.WriteStartElement("name");
                        xmlWriter.WriteString(dish.DishName);
                        xmlWriter.WriteEndElement();
                    
                        if(_weekday.CategoryList[categoryCounter].DishList[dishCounter].SpecialIngredients.Count != 0)
                        {
                            xmlWriter.WriteStartElement("note");

                            foreach(int specialIngretient in _weekday.CategoryList[categoryCounter].DishList[dishCounter].SpecialIngredients)
                            { 
                                xmlWriter.WriteString("-" + IngredientsTranslator.TranslateIngredientIndicator(specialIngretient) + " ");   
                            }

                            xmlWriter.WriteEndElement();
                        }
                        WritePriceInformation(_weekday, xmlWriter);
                        dishCounter++;
                        xmlWriter.WriteEndElement();
                    }
                    categoryCounter++;
                    dishCounter = 0;
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                categoryCounter = 0;
            }
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            xmlWriter.Close();  
        }      
    }

    static class ExampleMenu
    {
        public static void CreateExampleMenu()
        {
            List<Weekday> generalWeekdayList = new List<Weekday>();
            List<Category> generalCategoryListFirst = new List<Category>();
            List<Category> generalCategoryListSecond = new List<Category>();
            List<Dish> mensaInternationalDishes = new List<Dish>();
            List<Dish> VegetarianDishes = new List<Dish>();
            List<Dish> WokAndGrillDishes = new List<Dish>();
            List<Dish> CampustellerDishes = new List<Dish>();
            List<int> IngredientsFirstDish = new List<int>();
            List<int> IngredientsSecondDish = new List<int>();
            List<int> IngredientsThirdDish = new List<int>();
            List<int> IngredientsFourthDish = new List<int>();
            List<int> IngredientsFifthDish = new List<int>();

            generalWeekdayList.Add(new Weekday() {Date = "22.03.2022"});
            generalWeekdayList.Add(new Weekday() {Date = "23.03.2022"});

            generalCategoryListFirst.Add(new Category() {Name = "mensaInternational"});
            generalCategoryListFirst.Add(new Category() {Name = "Vegetarisch"});
            generalCategoryListFirst.Add(new Category() {Name = "Campusteller"});
            generalCategoryListSecond.Add(new Category() {Name = "Wok Und Grill"});

            mensaInternationalDishes.Add(new Dish() {DishName = "Grünes Gemüsecurry mit Quinoa"});
            VegetarianDishes.Add(new Dish() {DishName = "Wallnuss-Cheddar-Burger"});
            VegetarianDishes.Add(new Dish() {DishName = "Vegetarische Wings"});
            CampustellerDishes.Add(new Dish() {DishName = "Schweinegeschnetzeltes"});
            WokAndGrillDishes.Add(new Dish() {DishName = "Chicken Wings"});

            string[] prizesFirstDish = new string[] {"2,75 €", "4,05 €", "5,30 €", "3,10 €"};
            string[] prizesSecondDish = new string[] {"3.00 €", "4.45 €", "5.70 €", "3.50 €"};
            string[] prizesThirdDish = new string[] {"3.20 €", "4.50 €", "5.50 €", "3.75 €"};
            string[] prizesFourthDish = new string[] {"2.90 €", "4.40 €", "5.10 €", "3.35 €"};
            string[] prizesFifthDish = new string[] {"2.80 €", "4.30 €", "4.90 €", "3.00 €"};

            IngredientsThirdDish.Add(22);
            IngredientsFourthDish.Add(19);
            IngredientsFourthDish.Add(23);
            IngredientsFifthDish.Add(8);
            IngredientsFifthDish.Add(24);


            //Fillling WeekdayList in Menu class 
            Menu.WeekdayList = generalWeekdayList;

            //Filling CategoryList for Weekday one and two
            Menu.WeekdayList[0].CategoryList = generalCategoryListFirst;
            Menu.WeekdayList[1].CategoryList = generalCategoryListSecond;

            //Filling DishList for Weekday one and two
            Menu.WeekdayList[0].CategoryList[0].DishList = mensaInternationalDishes;
            Menu.WeekdayList[0].CategoryList[1].DishList = VegetarianDishes;
            Menu.WeekdayList[0].CategoryList[2].DishList = CampustellerDishes;
            Menu.WeekdayList[1].CategoryList[0].DishList = WokAndGrillDishes;

            //Filling the Prices-Array of each Dish of Weekday one and two
            Menu.WeekdayList[0].CategoryList[0].DishList[0].Prices = prizesFirstDish;
            Menu.WeekdayList[0].CategoryList[1].DishList[0].Prices = prizesSecondDish;
            Menu.WeekdayList[0].CategoryList[1].DishList[1].Prices = prizesThirdDish;
            Menu.WeekdayList[0].CategoryList[2].DishList[0].Prices = prizesFourthDish;
            Menu.WeekdayList[1].CategoryList[0].DishList[0].Prices = prizesFifthDish;

            //Filling the IngredientsList of each Dish of Weekday one and two
            Menu.WeekdayList[0].CategoryList[0].DishList[0].SpecialIngredients = IngredientsFirstDish;
            Menu.WeekdayList[0].CategoryList[1].DishList[0].SpecialIngredients = IngredientsSecondDish;
            Menu.WeekdayList[0].CategoryList[1].DishList[1].SpecialIngredients = IngredientsThirdDish;
            Menu.WeekdayList[0].CategoryList[2].DishList[0].SpecialIngredients = IngredientsFourthDish;
            Menu.WeekdayList[1].CategoryList[0].DishList[0].SpecialIngredients = IngredientsFifthDish;
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            ExampleMenu.CreateExampleMenu();
            XmlWriter.WriteXmlFile();
        }
    }
}