using System;

namespace OpenMensa_Parser
{
    public class TxtReader
    {
        public string TxtFilePath {get; private set;}

        public string GetSpecialIngredientsString(int ingredientNumber)
        {

        }

        public string GetSpecialIngredientsString(string ingredientLetter)
        {

        }

        public TxtReader(string filePath)
        {
            this.TxtFilePath = filePath;
        }
    }
}