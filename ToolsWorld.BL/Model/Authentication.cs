using System.Collections.Generic;

namespace ToolsWorld.BL.Model
{
    public class Authentication
    {
        /// <summary>
        /// Коллекция сотрудников.
        /// </summary>
        public ICollection<Worker> Workers { get; set; }

        public Authentication()
        {
            Workers = Worker.GetWorkers();
        }

        /// <summary>
        /// Текущий логин .
        /// </summary>
        public string CurrentLog { get; set; }

        /// <summary>
        /// Текущее имя.
        /// </summary>
        public string CurrentFullname { get; set; }

        /// <summary>
        /// Текущий уровень доступа.
        /// </summary>
        public int CurrentLvl { get; set; }

        /// <summary>
        /// Проверить введеные данные.
        /// </summary>
        /// <param name="log">Логин.</param>
        /// <param name="pass">Пароль.</param>
        public bool CheckData(string log, string pass)
        {
            bool check = false;

            foreach (Worker w in Workers)
            {
                if (w.Log == log && w.Pass == pass)
                {
                    check = true;
                    CurrentLog = w.Log;
                    CurrentFullname = w.Fullname;
                    CurrentLvl = w.Lvl;
                    break;
                }
            }

            return check;
        }
    }
}
