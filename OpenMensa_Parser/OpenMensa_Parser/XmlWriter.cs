using System;
using System.Text;
using System.Xml;
using System.Globalization;

namespace OpenMensa_Parser
{
    public class XmlWriter
    {
        private readonly string _fileName;
        private readonly string _parserVersion;
        private readonly string _openMensaVersion;
        private readonly string _feedInformation;
        private readonly string _schemaInstance;
        private readonly string _schemaLocation;

        private int _categoryCounter = 0;
        private int _dishCounter = 0;
        private int _priceCounter = 0;

        private string[] roleNames = new string[] {"student", "employee", "other", "pupil"};
        private char[] removedCharacters = new char[] {'€', ' '};

        public XmlWriter(string fileName, string parserVersion,string openMensaVersion, string feedInformation, string schemaInstance, string schemaLocation)
        {
            _fileName = fileName;
            _parserVersion = parserVersion;
            _openMensaVersion = openMensaVersion;
            _feedInformation = feedInformation;
            _schemaInstance = schemaInstance;
            _schemaLocation = schemaLocation;
        }
        
        public static void WriteXmlFile()
        {
            XmlTextWriter xmlWriter = new XmlTextWriter(_fileName, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;

            WriteOpenMensaStandardInformation(xmlWriter);
            WriteMenuInformation(xmlWriter);
        }

        public static void WriteOpenMensaStandardInformation(XmlTextWriter xmlWriter)
        {            
            //Writing process:
            xmlWriter.WriteStartDocument();                 //writes: <?xml version="1.0" encoding="utf-8"?>
            xmlWriter.WriteStartElement("openmensa");
            xmlWriter.WriteAttributeString("version", _openMensaVersion);
            xmlWriter.WriteAttributeString("xmlns", _feedInformation);
            xmlWriter.WriteAttributeString("xmlns", "xsi", null, _schemaInstance);
            xmlWriter.WriteAttributeString("xsi", "schemaLocation", null, _feedInformation + " " + _schemaLocation);
            xmlWriter.WriteStartElement("version");
            xmlWriter.WriteString(_parserVersion);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("canteen");
        }

        public static void WritePriceInformation(Weekday weekday, XmlTextWriter xmlWriter)
        {
            foreach(string roleName in roleNames)
            {
                xmlWriter.WriteStartElement("price");
                xmlWriter.WriteAttributeString("role", roleName);
                xmlWriter.WriteString(weekday.CategoryList[_categoryCounter].DishList[_dishCounter].Prices[_priceCounter].Replace(",", ".").TrimEnd(removedCharacters));
                xmlWriter.WriteEndElement();
                _priceCounter++;
            }
            _priceCounter = 0;
        }

        public static void WriteMenuInformation(XmlTextWriter xmlWriter)
        {
            foreach(Weekday day in Menu.WeekdayList)
            {
                DateTime dateTime = DateTime.Parse(day.Date);

                xmlWriter.WriteStartElement("day");
                xmlWriter.WriteAttributeString("date", dateTime.ToString("yyyy'-'MM'-'dd"));

                foreach(Category category in day.CategoryList)
                {
                    xmlWriter.WriteStartElement("category");
                    xmlWriter.WriteAttributeString("name", category.Name);
                    foreach(Dish dish in day.CategoryList[_categoryCounter].DishList)
                    {
                        xmlWriter.WriteStartElement("meal");
                        xmlWriter.WriteStartElement("name");
                        xmlWriter.WriteString(dish.DishName);
                        xmlWriter.WriteEndElement();
                    
                        if(day.CategoryList[_categoryCounter].DishList[_dishCounter].SpecialIngredients.Count != 0)
                        {
                            xmlWriter.WriteStartElement("note");

                            foreach(int specialIngretient in day.CategoryList[_categoryCounter].DishList[_dishCounter].SpecialIngredients)
                            { 
                                xmlWriter.WriteString("-" + IngredientsTranslator.TranslateIngredientIndicator(specialIngretient) + " ");   
                            }

                            xmlWriter.WriteEndElement();
                        }
                        WritePriceInformation(day, xmlWriter);
                        _dishCounter++;
                        xmlWriter.WriteEndElement();
                    }
                    _categoryCounter++;
                    _dishCounter = 0;
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                _categoryCounter = 0;
            }
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            xmlWriter.Close();  
        }      
    }
}