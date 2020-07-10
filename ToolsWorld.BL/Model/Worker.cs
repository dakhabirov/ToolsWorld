using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Data;
using System.Linq;

namespace ToolsWorld.BL.Model
{
    /// <summary>
    /// Сотрудник.
    /// </summary>
    public class Worker : INotifyPropertyChanged
    {
        static readonly MyDbContext myDbContext = new MyDbContext();

        string log;
        string pass;
        string fullname;
        string tel;
        int? departamentId;
        int lvl;

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int WorkerId { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        [MaxLength(15)]
        public string Log
        {
            get { return log; }
            set
            {
                log = value;
                OnPropertyChanged("Log");
            }
        }

        /// <summary>
        /// Пароль.
        /// </summary>
        [MaxLength(25)]
        public string Pass
        {
            get { return pass; }
            set
            {
                pass = value;
                OnPropertyChanged("Pass");
            }
        }

        /// <summary>
        /// Полное имя.
        /// </summary>
        [MaxLength(100)]
        public string Fullname
        {
            get { return fullname; }
            set
            {
                fullname = value;
                OnPropertyChanged("Fullname");
            }
        }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        [MaxLength(16)]
        public string Tel
        {
            get { return tel; }
            set
            {
                tel = value;
                OnPropertyChanged("Tel");
            }
        }

        /// <summary>
        /// Идентификатор отдела.
        /// </summary>
        public int? DepartamentId
        {
            get { return departamentId; }
            set
            {
                departamentId = value;
                OnPropertyChanged("DepartamentId");
            }
        }

        /// <summary>
        /// Уровень доступа.
        /// </summary>
        public int Lvl
        {
            get { return lvl; }
            set
            {
                lvl = value;
                OnPropertyChanged("Lvl");
            }
        }

        /// <summary>
        /// Коллекция продаж.
        /// </summary>
        public virtual ICollection<Sale> Sales { get; set; }

        /// <summary>
        /// Коллекция поставок.
        /// </summary>
        public virtual ICollection<Order> Orders { get; set; }

        /// <summary>
        /// Отдел.
        /// </summary>
        public virtual Departament Departament { get; set; }

        /// <summary>
        /// Реализация интерфейса INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Получить коллекцию сотрудников.
        /// </summary>
        /// <returns>Коллекция сотрудников.</returns>
        public static List<Worker> GetWorkers()
        {
            List<Worker> workers = new List<Worker>(myDbContext.Workers);
            return workers;
        }

        /// <summary>
        /// Получить представление коллекции.
        /// </summary>
        /// <returns>Представление коллекции.</returns>
        private static ICollectionView GetCollectionView()
        {
            var workers = GetWorkers(); // вся коллекция, без фильтра
            ICollectionView workersCollectionView;
            workersCollectionView = CollectionViewSource.GetDefaultView(workers);   // представляем всю коллекцию
            return workersCollectionView;
        }


        #region Добавление

        /// <summary>
        /// Добавить.
        /// </summary>
        /// <param name="log">Логин.</param>
        /// <param name="pass">Пароль.</param>
        /// <param name="fullname">Полное имя.</param>
        /// <param name="tel">Номер телефона.</param>
        /// <param name="departamentId">Идентификатор отдела.</param>
        /// <param name="lvl">Уровень доступа.</param>
        public static void Insert(string log, string pass, string fullname, string tel, int departamentId, int lvl)
        {
            Worker worker = new Worker
            {
                Log = log,
                Pass = pass,
                Fullname = fullname,
                Tel = tel,
                DepartamentId = departamentId,
                Lvl = lvl
            };

            myDbContext.Workers.Add(worker);
            myDbContext.SaveChanges();
        }

        #endregion

        #region Редактирование

        /// <summary>
        /// Изменить.
        /// </summary>
        /// <param name="log">Логин.</param>
        /// <param name="pass">Пароль.</param>
        /// <param name="fullname">Полное имя.</param>
        /// <param name="tel">Номер телефона.</param>
        /// <param name="departamentId">Идентификатор отдела.</param>
        /// <param name="lvl">Уровень доступа.</param>
        public static void Update(string log, string pass, string fullname, string tel, int departamentId, int lvl)
        {
            var query = from w in myDbContext.Workers
                        where w.Log == log
                        select w;

            foreach (Worker worker in query)
            {
                worker.Log = log;
                worker.Pass = pass;
                worker.Fullname = fullname;
                worker.Tel = tel;
                worker.DepartamentId = departamentId;
                worker.Lvl = lvl;
            }

            myDbContext.SaveChanges();
        }

        #endregion

        #region Удаление

        /// <summary>
        /// Удалить.
        /// </summary>
        /// <param name="workerId">Идентификатор сотрудника.</param>
        public static void Delete(int workerId)
        {
            Worker worker = myDbContext.Workers.Where(w => w.WorkerId == workerId).FirstOrDefault();

            myDbContext.Workers.Remove(worker);
            myDbContext.SaveChanges();
        }

        #endregion

        #region Фильтр

        /// <summary>
        /// Фильтровать по логину.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Коллекция сотрудников.</returns>
        public static ICollectionView FilterByLog(string value)
        {
            var filter = GetCollectionView();  // получаем представление всей коллекции
            filter.Filter = w => ((Worker)w).Log == value;  // фильтруем коллекцию
            return filter;
        }

        /// <summary>
        /// Фильтровать по имени.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Коллекция сотрудников.</returns>
        public static ICollectionView FilterByFullname(string value)
        {
            var filter = GetCollectionView();  // получаем представление всей коллекции
            filter.Filter = w => ((Worker)w).Fullname == value;  // фильтруем коллекцию
            return filter;
        }

        /// <summary>
        /// Фильтровать по номеру телефона.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Коллекция сотрудников.</returns>
        public static ICollectionView FilterByTel(string value)
        {
            var filter = GetCollectionView();  // получаем представление всей коллекции
            filter.Filter = w => ((Worker)w).Tel == value;  // фильтруем коллекцию
            return filter;
        }

        /// <summary>
        /// Фильтровать по наименованию отдела.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Коллекция сотрудников.</returns>
        public static ICollectionView FilterByDepartamentName(string value)
        {
            var filter = GetCollectionView();  // получаем представление всей коллекции
            filter.Filter = w => ((Worker)w).Departament.DepartamentName == value;  // фильтруем коллекцию
            return filter;
        }

        /// <summary>
        /// Фильтровать по уровню доступа.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Коллекция сотрудников.</returns>
        public static ICollectionView FilterByLvl(int value)
        {
            var filter = GetCollectionView();  // получаем представление всей коллекции
            filter.Filter = w => ((Worker)w).lvl == value;  // фильтруем коллекцию
            return filter;
        }

        #endregion

        /// <summary>
        /// Получить сотрудника по логину.
        /// </summary>
        /// <param name="log">Имя пользователя.</param>
        /// <returns>Сотрудник.</returns>
        public static Worker GetWorkerByLog(string log)
        {
            Worker worker = new Worker();

            var query = from w in myDbContext.Workers
                        where w.Log == log
                        select w;

            foreach (Worker w in query)
                worker = w;

            return worker;
        }

        /// <summary>
        /// Сменить пароль.
        /// </summary>
        /// <param name="workerId">Идентификатор сотрудника.</param>
        /// <param name="newPass">Новый пароль.</param>
        public static void ChangePassword(int workerId, string newPass)
        {
            // определяем сотрудника
            var query = from w in myDbContext.Workers
                        where w.WorkerId == workerId
                        select w;

            foreach (Worker worker in query)
                worker.Pass = newPass;  // присваиваем новый пароль

            myDbContext.SaveChanges();
        }

        /// <summary>
        /// Проверка на изменение значений.
        /// </summary>
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
