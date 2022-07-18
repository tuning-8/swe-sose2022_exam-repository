using System;
using HtmlAgilityPack;

namespace OpenMensa_Parser
{
    public class HtmlParser
    {
        public string htmlURL {get; private set;}
        HtmlWeb web;
        public HtmlDocument htmlDoc {get; private set;}
        HtmlNode rootNode;

        public HtmlParser(string url, string node)
        {
            this.htmlURL = url;
            this.web = new HtmlWeb();
            this.htmlDoc = web.Load(url);
            this.rootNode = htmlDoc.DocumentNode.SelectSingleNode(node);
        }
    }
}