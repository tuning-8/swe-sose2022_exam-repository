namespace OpenMensa_Parser
{
    public class Menu
    {
        List<string> dateList = new List<string>();

        List<string> ParseDate()
        {
        }
    }

    public class Weekday
    {
        struct DishStruct
        {
            string dishName;
            double[] prices;
            List<string> specialIngerdients;
        }
        struct CategoryStruct
        {
            string name;
            List<DishStruct> dishList;
        }
        public string Date {get; private set;}

    }
}