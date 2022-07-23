using System;
using System.Text;
using System.Xml;
using System.Globalization;

namespace OpenMensa_Parser
{
    public class XmlWriter
    {
        public string FileName { get; private set; }
        public string ParserVersion { get; private set; }
        public string OpenMensaVersion { get; private set; }
        public string FeedInformation { get; private set; }
        public string SchemaInstance { get; private set; }
        public string SchemaLocation { get; private set; }

        private int categoryCounter = 0;
        private int dishCounter = 0;
        private int priceCounter = 0;

        private string[] roleNames = new string[] {"student", "employee", "other", "pupil"};
        private char[] removedCharacters = new char[] {'€', ' '};

        public static void WriteXmlFile()
        {
            XmlTextWriter xmlWriter = new XmlTextWriter(FileName, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;

            WriteOpenMensaStandardInformation(xmlWriter);
            WriteMenuInformation(xmlWriter);
        }

        public static void WriteOpenMensaStandardInformation(XmlTextWriter xmlWriter)
        {            
            //Writing process:
            xmlWriter.WriteStartDocument();                 //writes: <?xml version="1.0" encoding="utf-8"?>
            xmlWriter.WriteStartElement("openmensa");
            xmlWriter.WriteAttributeString("version", OpenMensaVersion);
            xmlWriter.WriteAttributeString("xmlns", FeedInformation);
            xmlWriter.WriteAttributeString("xmlns", "xsi", null, SchemaInstance);
            xmlWriter.WriteAttributeString("xsi", "schemaLocation", null, FeedInformation + " " + SchemaLocation);
            xmlWriter.WriteStartElement("version");
            xmlWriter.WriteString(ParserVersion);
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
}