/**
 * @file
 * @author  nicoschurig <nico-schurig@gmx.de>
 * @version 1.08
 *
 * @section LICENSE
 *
 * Licence information can be found in README.me (https://github.com/tuning-8/swe-sose2022_exam-repository/blob/main/README.md)
 *
 * @section DESCRIPTION
 *
 * File that includes the 'XmlWriter' class with all methods needed to write information into a .xml file.
 */

using System;
using System.Text;
using System.Xml;
using System.Globalization;

namespace OpenMensa_Parser
{
    /**
     * @brief Class that contains methods to write provided information into a .xml file.
     *
     *
     */
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

        /**
         * @brief   Array of customer roles
         *
         * @details This Array contains the roles of potential customers of the canteen. Currently the TU Freiberg canteen differentiates
         *          between students, employee, other and pupil. Each role gets a different price allocated.
         * 
         */
        private string[] roleNames = new string[] {"student", "employee", "other", "pupil"};

        /**
         * @brief   Array of characters removed from the price
         *
         * @details This Array contains the characters that should be removed from the price string. Open Mensa requires the following
         *          price format: 0.00. The data provided by the TU Freiberg canteen website has the format: 0,00 €. The .TrimEnd() Method
         *          is called (after the .Replace() method) with the removedCharacters parameter. That results in the required price format.
         * 
         */
        private char[] removedCharacters = new char[] {'€', ' '};

        public Menu MenuInstance { get; private set; }

        /**
         * @brief   Constructor that allocates a value to each field.
         *
         * @params[in]  fileName            Name of the generated .xml file
         * @params[in]  parserVersion       Current version of our OpenMensa parser
         * @params[in]  openMensaVersion    Current version of the OpenMensa project
         * @params[in]  feedInformation     Provides an url to the current OpenMensa Feed
         * @params[in]  schemaInstance      Provides information about the xml schema instance
         * @params[in]  schemaLocation      Download of the OpenMensa xml schema
         * @params[in]  menu                Instance of the Menu class, loaded with menu infomation
          *@params[in]  xmlFilePath         File path where the .xml file is saved at 
         */
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
        
        /**
         * @brief   Method that writes the whole .xml file.
         *
         * @details A method, that generates an Instance of XmlTextWriter. It then calls the WriteOpenMensaStandardInformation method
         *          and the WriteMenuInformation method. WriteXmlFile() method is public , so it can be accessed for each potential instance of the
         *          XmlWriter class. After calling it, the whole .xml file will be (over-)written.
         *
         */
        public void WriteXmlFile()
        {
            XmlTextWriter xmlWriter = new XmlTextWriter(_xmlFilePath + _fileName, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;

            WriteOpenMensaStandardInformation(xmlWriter);
            WriteMenuInformation(xmlWriter);
        }

        /**
         * @brief   Method that writes the declaration into the .xml file.
         *
         * @details A method that is called by the WriteXmlFile() method. It writes the deklaration into the .xml file, which is the initiation
         *          of the .xml file.
         *
         * @param[in]   xmlWriter      Instance of XmlTextWriter class
         *
         */
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

        /**
         * @brief   Method that writes the prices into the .xml file.
         *
         * @details A method that writes the prices of the given dish for each guest. There are multiple guest-roles
         *          (student, employee, pupil, others) and a matching amount of prices provided.
         *
         * @param[in]   dish            Instance of Dish class containing the needed price information.
         * @param[in]   xmlWriter       Instance of XmlTextWriter class   
         *
         */
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

        /**
         * @brief   Method that writes all the menu information into the .xml file.
         *
         * @details A method that writes the information for each dish in each category on each day. It is the writer of the
         *          actual menu, that can be seen on the OpenMensa website.
         *
         * @param[in]   xmlWriter       Instance of XmlTextWriter class 
         *
         */
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
                                xmlWriter.WriteString("-" + OpenMena_Parser.IngredientsTranslator.TranslateIngredientIndicator(specialIngretient) + " ");   
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