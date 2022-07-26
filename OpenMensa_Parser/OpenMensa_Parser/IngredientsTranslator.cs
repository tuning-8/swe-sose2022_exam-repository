/**
 * @file
 * @author  tuning-8 <tuning_8@gmx.de>, nicoschurig
 * @version 1.6
 *
 * @section LICENSE
 *
 * Licence information can be found in README.me (https://github.com/tuning-8/swe-sose2022_exam-repository/blob/main/README.md)
 *
 * @section DESCRIPTION
 *
 * File that includes the dictionary and the method for translating ingredient numbers to human readable short descriptions.
 * (e.g. '2' -> 'mit Farbstoff', 'WEI' -> 'Weizen')
 */

using System;
using System.Text;

namespace OpenMensa_Parser
{
    /**
     * @brief   Class includes the translation dictionary and method
     */
    static class IngredientsTranslator
    {
        /**
         * @brief   Dictionary of indicators and their matching string
         *
         * @details This dictionary contains all the indicator strings and their translations, that are provided in the following
         *          PDF: (https://www.studentenwerk-freiberg.de/fileadmin/essen-trinken/2019-10-22_Kennteichnungshinweise.pdf).
         *
         * @note    The HtmlParser returns all indicators in datatype string, so we don't have to differentiate between int and
         *          string and are able to store all indicators in one list<string> specialIngredients in Dish class.
         * 
         */
        private static Dictionary<string, string> IngredientsByStringIndicator = new Dictionary<string, string>() {
            {"1","mit Konservierungsstoffen"},
            {"2", "mit Farbstoff"},
            {"3", "gewachst"},
            {"4", "geschwärzt"},
            {"5", "mit Antioxidationsmittel"},
            {"6", "mit Phosphat"},
            {"7", "geschwefelt"},
            {"8", "mit Süßungsmitteln"},
            {"9", "mit Geschmacksverstärker"},
            {"10", "phenylalaninhaltig"},
            {"16", "chininhaltig"},
            {"18", "Weichtiere"},
            {"19", "glutenhaltig"},
            {"20", "Krebstiere"},
            {"21", "eihaltig"},
            {"22", "Erdnüsse"},
            {"23", "Soja"},
            {"24", "Milch/Milchzucker"},
            {"25", "Schalenfrüchte/Nüsse"},
            {"26", "Sellerie"},
            {"27", "Senf"},
            {"28", "Sesamsamen"},
            {"29", "Schwefeldioxid u. Sulfite"},
            {"30", "Fisch (Allergen)"},
            {"31", "Lupine"},
            {"35", "mit Azofarbstoff"},
            {"43", "mit kakaohaltiger Fettglasur"},
            {"171", "ungeeignet für Vegetarier"},
            {"WEI", "Weizen"},
            {"ROG", "Roggen"},
            {"GER", "Gerste"},
            {"HAF", "Hafer"},
            {"DIN", "Dinkel"},
            {"KAM", "Kamut"},
            {"MAN", "Mandeln"},
            {"HAS", "Haselnüsse"},
            {"WAL", "Walnüsse"},
            {"CAS", "Cashewnüsse"},
            {"PEK", "Pekanüsse"},
            {"PAR", "Paranüsse"},
            {"PIS", "Pistazien"},
            {"MAC", "Macadamia"}
        };

        /**
         * @brief   Method to translate ingredient number to description
         *
         * @detail  A method that returns a short description of a specific ingredient
                    (value of the dictionary) with the ingredient number/short string as
                    input (key of the dictionary)
         *
         * @param[in]   indicatorString     Ingredient abbraviation (key)
         * @return      Full description of the ingredient (value)
         */
        public static string TranslateIngredientIndicator(string indicatorString)
        {
            return  IngredientsByStringIndicator[indicatorString];
        }
    }
}