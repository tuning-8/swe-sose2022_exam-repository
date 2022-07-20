using System;

namespace OpenMensa_Parser
{
    public class TxtReader
    {
        public string TxtFilePath {get; private set;}

        public string GetSpecialIngredientsString(int ingredientNumber)
        {
            return "";
        }

        public string GetSpecialIngredientsString(string ingredientLetter)
        {
            return "";
        }

        public TxtReader(string filePath)
        {
            this.TxtFilePath = filePath;
        }
    }
}