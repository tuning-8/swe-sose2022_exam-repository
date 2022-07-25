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
        private readonly string _xmlFilePath;

        private int _priceCounter = 0;

        private string[] roleNames = new string[] {"student", "employee", "other", "pupil"};
        private char[] removedCharacters = new char[] {'€', ' '};

        public Menu MenuInstance { get; private set; }

        public XmlWriter(string fileName, string parserVersion,string openMensaVersion, string feedInformation, string schemaInstance, string schemaLocation, Menu menu, string xmlFilePath)
        {
            _fileName = fileName;
            _parserVersion = parserVersion;
            _openMensaVersion = openMensaVersion;
            _feedInformation = feedInformation;
            _schemaInstance = schemaInstance;
            _schemaLocation = schemaLocation;
            this.MenuInstance = menu;
        }
        
        public void WriteXmlFile()
        {
            XmlTextWriter xmlWriter = new XmlTextWriter(_xmlFilePath + _fileName, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;

            WriteOpenMensaStandardInformation(xmlWriter);
            WriteMenuInformation(xmlWriter);
        }

        private void WriteOpenMensaStandardInformation(XmlTextWriter xmlWriter)
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

        private void WritePriceInformation(Dish dish, XmlTextWriter xmlWriter)
        {
            for(int i = 0; i < dish.Prices.Length; i++)
            {
                xmlWriter.WriteStartElement("price");
                xmlWriter.WriteAttributeString("role", roleNames[i]);
                xmlWriter.WriteString(dish.Prices[i].Replace(',', '.').TrimEnd(removedCharacters));
                xmlWriter.WriteEndElement();
            }
        }

        private void WriteMenuInformation(XmlTextWriter xmlWriter)
        {
            foreach(Weekday day in MenuInstance.WeekdayList)
            {
                DateTime dateTime = DateTime.Parse(day.Date);

                xmlWriter.WriteStartElement("day");
                xmlWriter.WriteAttributeString("date", dateTime.ToString("yyyy'-'MM'-'dd"));

                foreach(Category category in day.CategoryList)
                {
                    xmlWriter.WriteStartElement("category");
                    xmlWriter.WriteAttributeString("name", category.Name);
                    foreach(Dish dish in category.DishList)
                    {
                        xmlWriter.WriteStartElement("meal");
                        xmlWriter.WriteStartElement("name");
                        xmlWriter.WriteString(dish.DishName);
                        xmlWriter.WriteEndElement();
                    
                        if(dish.SpecialIngredients.Count != 0)
                        {
                            xmlWriter.WriteStartElement("note");

                            foreach(string specialIngretient in dish.SpecialIngredients)
                            { 
                                xmlWriter.WriteString("-" + IngredientsTranslator.TranslateIngredientIndicator(specialIngretient) + " ");   
                            }

                            xmlWriter.WriteEndElement();
                        }
                        WritePriceInformation(dish, xmlWriter);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            xmlWriter.Close();  
        }      
    }
}