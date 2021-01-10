using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Utilities
{
    public static class StringConverter
    {
        private static Random random = new Random();

        public static string GetContentRootPath(string contentRootPath)
        {
            if (contentRootPath.Contains("C:\\"))
            {
                return Path.Combine(contentRootPath, "..", "Configuration");
            }
            return contentRootPath;
        }


        public static string GetRandomString(int length)
        {

            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static DateTime RandomDay(int year)
        {
            DateTime start = new DateTime(year, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }

        public static string GetNumbers(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }



    }
}
