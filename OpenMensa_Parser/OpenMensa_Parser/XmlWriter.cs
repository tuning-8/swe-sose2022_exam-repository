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

        }

        public static void WriteOpenMensaStandardInformation(XmlTextWriter xmlWriter)
        {

        }

        public static void WritePriceInformation(Weekday _weekday, XmlTextWriter xmlWriter)
        {

        }

        public static void WriteMenuInformation(XmlTextWriter xmlWriter)
        {
            
        }
    }
}