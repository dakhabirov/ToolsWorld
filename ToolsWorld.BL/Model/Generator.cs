using System;

namespace ToolsWorld.BL
{
    /// <summary>
    /// Генератор случайных значений.
    /// </summary>
    public class Generator
    {
        readonly Random rnd = new Random();

        /// <summary>
        /// Сгенерировать случайную строку.
        /// </summary>
        /// <returns>Строка.</returns>
        public string GenerateRandomString(int size)
        {
            string outStr = "";
            string[] arr =
            {
                "1","2","3","4","5","6","7","8","9",
                "A", "B","C","D","E","F","G","H","J","K","L","M","N","P","Q","R","S","T","U","V","W","X","Y","Z",
                "a","b","c","d","e","f","g","h","i","j","k","m","n","o","p","q","r","s","t","u","v","w","x","y","z"
            };

            for (int i = 0; i < size; i++)
            {
                outStr += arr[rnd.Next(0, 57)];
            }

            return (outStr);
        }

        /// <summary>
        /// Сгенерировать случайное число.
        /// </summary>
        /// <returns>Строка.</returns>
        public string GenerateRandomNumeric(int size)
        {
            string outStr = "";
            string[] arr =
            {
                "0", "1","2","3","4","5","6","7","8","9"
            };

            for (int i = 0; i < size; i++)
            {
                outStr += arr[rnd.Next(0, arr.Length)];
            }

            return (outStr);
        }

        /// <summary>
        /// Сгенерировать случайную дату.
        /// </summary>
        /// <param name="startDate">Начальная дата.</param>
        /// <param name="stopDate">Конечная дата.</param>
        /// <returns>Дата.</returns>
        public string GenerateRandomDate(string startDate, string stopDate)
        {
            DateTime start = DateTime.Parse(startDate);
            DateTime stop = DateTime.Parse(stopDate);
            return start.AddDays(rnd.Next(0, new TimeSpan(stop.Ticks - start.Ticks).Days)).ToString("d");
        }
    }
}
