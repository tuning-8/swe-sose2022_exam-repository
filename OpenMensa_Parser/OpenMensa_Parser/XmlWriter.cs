using System;
using System.Xml;
using System.Globalization;

namespace OpenMensa_Parser
{
    public class XmlWriter
    {
        public static string fileName = "OpenmenMensaXml.xml";
        public static string parserVersion = "1.01-1";
        public static string feedInformation = "http://openmensa.org/open-mensa-v2";
        public static string schemaInstance = "http://www.w3.org/2001/XMLSchema-instance";
        public static string schemaLocation = "http://openmensa.org/open-mensa-v2.xsd";

        public static int categoryCounter = 0;
        public static int dishCounter = 0;
        public static int priceCounter = 0;

        public static string[] roleNames = new string[] {"student", "employee", "other", "pupil"};      //array contains the customers in the same order as the Mensa-Website

        public static void WriteXmlFile()
        {
            XmlTextWriter xmlWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;

            WriteOpenMensaStandardInformation(xmlWriter);
            WriteMenuInformation(xmlWriter);
        }

        public static void WriteOpenMensaStandardInformation(XmlTextWriter xmlWriter)
        {
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
                xmlWriter.WriteString(_weekday.CategoryList[categoryCounter].DishList[dishCounter].Prices[priceCounter].ToString("F", CultureInfo.InvariantCulture));
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