using System;
using System.Text;
using System.Xml;


public class Program
{
    public static void Main(string[] args)
    {
        int arrayCounter = 0;                   //counts the index of the arrays
        string fileName = "Example.xml";        /*The data is written into this .xml file
                                                                -if it doesn't exist it will be created
                                                                -if it already exists it will be overwritten*/

            
        XmlTextWriter xmlWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);   
        /*creates a new XmlTextWriter
            - second parameter = encoding settings/format*/
        xmlWriter.Formatting = Formatting.Indented;

        string[] bookTitles = new string[] {"War and Peace", "A Song of Ice and Fire", "Dune"};
        string[] authorNames = new string[] {"Leo Tolstoy", "George R. R. Martin", "Frank Herbert"};
        double[] bookPrices = new double[] {16.99, 24.99, 12.99};

        void WriteBookInformation(string title)
        {
            xmlWriter.WriteStartElement("book");           //writes out a start tag with the specified local name
            xmlWriter.WriteAttributeString("title", title);    //writes the attribute with the local name and value
            xmlWriter.WriteStartElement("author");
            xmlWriter.WriteString(authorNames[arrayCounter]);      //writes the given text content
            xmlWriter.WriteEndElement();                           //closes the previous WriteStartAttribute() call
            xmlWriter.WriteStartElement("price");
            xmlWriter.WriteString(bookPrices[arrayCounter].ToString() + " €");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
        }

        xmlWriter.WriteStartDocument();                 //writes: <?xml version="1.0" encoding="utf-8"?>
        xmlWriter.WriteStartElement("bookstore");

        foreach(string title in bookTitles)
        {
            WriteBookInformation(title);
            arrayCounter++;
        }

        arrayCounter = 0;
        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndDocument();
        xmlWriter.Flush();      /*flushes whatever is in the buffer to the underlying streams and also 
                                    flushes the underlying stream*/
        xmlWriter.Close();      //the data is only written into the file, when this method is called
    }
}