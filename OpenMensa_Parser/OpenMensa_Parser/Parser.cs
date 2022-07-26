/**
 * @file
 * @author  tuning-8 <tuning_8@gmx.de>
 * @version 1.3
 *
 * @section LICENSE
 *
 * Licence information can be found in README.me (https://github.com/tuning-8/swe-sose2022_exam-repository/blob/main/README.md)
 *
 * @section DESCRIPTION
 *
 * File that includes the main program.
 */

using System;
using System.Xml;
using HtmlAgilityPack;

namespace OpenMensa_Parser
{
    /**
     * @brief   Class that runs the main program
     */
    public class Program
    {
        public static void Main(string[] args)
        {
            // menu root node
            string entry_node = "/html/body/div[1]/div[3]/div[2]/div[3]/div[2]/div/div[4]";
            // website of the canteen(s)
            string[] studentServiceURLs = new string[]
            {
                "https://www.studentenwerk-freiberg.de/freiberg/essen-trinken/speiseplan/de/woche/this/"//,
                //"https://www.studentenwerk-freiberg.de/freiberg/essen-trinken/speiseplan/de/woche/next/",
            };
            // for every URL specified in the array, a new HTML parser, XML writer and Menu is instanced
            foreach (string url in studentServiceURLs)
            {
                HtmlParser menuParser = new HtmlParser(url, entry_node);
                Menu menu = new Menu(menuParser);
                menu.GenrateWeekdayInstances();
                XmlWriter menuWriter = new XmlWriter("Menu.xml", "v1.1", "2.1", 
                "http://openmensa.org/open-mensa-v2", "http://www.w3.org/2001/XMLSchema-instance",
                "http://openmensa.org/open-mensa-v2.xsd", menu,
                "/mnt/network_drives/TUBAF-ZFS1-SMB/02_SS_22/Softwareentwicklung/Exam_Project/git_repo/swe-sose2022_exam-repository/OpenMensa_Parser/OpenMensa_Parser");
                menuWriter.WriteXmlFile();
            }
        }
    }
}