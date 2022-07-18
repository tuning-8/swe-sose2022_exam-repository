using System;
using System.Xml;

namespace OpenMensa_Parser
{
    public class XmlWriter
    {
        public string XmlFilePath {get; private set;}

        public XmlWriter(string filePath)
        {
            this.XmlFilePath = filePath;
        }
    }
}