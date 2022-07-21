using System;
using System.Text;

namespace OpenMena_Parser
{
    static class IngredientsTranslator
    {
        private static Dictionary<int, string> IngredientsByIntIndicator = new Dictionary<int, string>() {
            {1,"mit Konservierungsstoffen"},
            {2, "mit Farbstoff"},
            {3, "gewachst"},
            {4, "geschwärzt"},
            {5, "mit Antioxidationsmittel"},
            {6, "mit Phosphat"},
            {7, "geschwefelt"},
            {8, "mit Süßungsmitteln"},
            {9, "mit Geschmacksverstärker"},
            {10, "phenylalaninhaltig"},
            {16, "chininhaltig"},
            {18, "Weichtiere"},
            {19, "glutenhaltig"},
            {20, "Krebstiere"},
            {21, "eihaltig"},
            {22, "Erdnüsse"},
            {23, "Soja"},
            {24, "Milch/Milchzucker"},
            {25, "Schalenfrüchte/Nüsse"},
            {26, "Sellerie"},
            {27, "Senf"},
            {28, "Sesamsamen"},
            {29, "Schwefeldioxid u. Sulfite"},
            {30, "Fisch (Allergen)"},
            {31, "Lupine"},
            {35, "mit Azofarbstoff"},
            {43, "mit kakaohaltiger Fettglasur"},
            {171, "ungeeignet für Vegetarier"}
        };

        private static Dictionary<string, string> IngredientsByStringIndicator = new Dictionary<string, string>() {

        };
    }
}