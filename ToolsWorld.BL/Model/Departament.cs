using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Linq;

namespace ToolsWorld.BL.Model
{
    /// <summary>
    /// Отдел.
    /// </summary>
    public class Departament : INotifyPropertyChanged
    {
        static readonly MyDbContext myDbContext = new MyDbContext();

        string departamentName;

        /// <summary>
        /// Идентификатор отдела.
        /// </summary>
        public int DepartamentId { get; set; }

        /// <summary>
        /// Наименование отдела.
        /// </summary>
        public string DepartamentName
        {
            get { return departamentName; }
            set
            {
                departamentName = value;
                OnPropertyChanged("DepartamentName");
            }
        }

        /// <summary>
        /// Коллекция сотрудников.
        /// </summary>
        public virtual ICollection<Worker> Workers { get; set; }

        /// <summary>
        /// Коллекция товаров.
        /// </summary>
        public virtual ICollection<Product> Products { get; set; }

        /// <summary>
        /// Получить коллекцию отделов.
        /// </summary>
        /// <returns>Список отделов.</returns>
        public static List<Departament> GetDepartaments()
        {
            List<Departament> departaments = new List<Departament>(myDbContext.Departaments);
            return departaments;
        }

        /// <summary>
        /// Получить представление коллекции.
        /// </summary>
        /// <returns>Представление коллекции.</returns>
        private static ICollectionView GetCollectionView()
        {
            var departaments = GetDepartaments(); // вся коллекция, без фильтра
            ICollectionView workersCollectionView;
            workersCollectionView = CollectionViewSource.GetDefaultView(departaments);   // представляем всю коллекцию
            return workersCollectionView;
        }

        /// <summary>
        /// Получить идентификатор отдела.
        /// </summary>
        /// <param name="departamentName">Наименование.</param>
        /// <returns>Идентификатор отдела.</returns>
        public static int GetDepartamentId(string departamentName)
        {
            int departamentId = 0;

            // определяем id отдела по его наименованию
            var query = from d in myDbContext.Departaments
                        where d.DepartamentName == departamentName
                        select d;

            foreach (Departament departament in query)
                departamentId = departament.DepartamentId;

            return departamentId;
        }

        /// <summary>
        /// Получить наименования отделов.
        /// </summary>
        /// <returns>Список наименований отделов.</returns>
        public static List<string> GetDepartamentNames()
        {
            List<string> DepartamentNames = new List<string>();

            var query = from d in myDbContext.Departaments
                        select d;

            foreach (Departament departament in query)
                DepartamentNames.Add(departament.DepartamentName);

            return DepartamentNames;
        }

        #region Добавление

        /// <summary>
        /// Добавить.
        /// </summary>
        /// <param name="departamentName">Наименование.</param>
        public static void Insert(string departamentName)
        {
            Departament departament = new Departament
            {
                DepartamentName = departamentName,
            };

            myDbContext.Departaments.Add(departament);
            myDbContext.SaveChanges();
        }

        #endregion

        #region Удаление.

        /// <summary>
        /// Удалить.
        /// </summary>
        /// <param name="departamentId">Идентификатор отдела.</param>
        public static void Delete(int departamentId)
        {
            Departament departament = myDbContext.Departaments.Where(d => d.DepartamentId == departamentId).FirstOrDefault();
            myDbContext.Departaments.Remove(departament);
            myDbContext.SaveChanges();
        }

        #endregion

        #region Редактирование

        /// <summary>
        /// Изменить.
        /// </summary>
        /// <param name="oldDepartamentName">Предыдущее наименование.</param>
        /// <param name="newDepartamentName">Новое наименование.</param>
        public static void Update(string oldDepartamentName, string newDepartamentName)
        {
            var query = from d in myDbContext.Departaments
                        where d.DepartamentName == oldDepartamentName
                        select d;

            foreach (Departament departament in query)
            {
                departament.DepartamentName = newDepartamentName;
            }

            myDbContext.SaveChanges();
        }

        #endregion

        #region Фильтр

        /// <summary>
        /// Фильтровать по наименованию.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns></returns>
        public static ICollectionView FilterByDepartamentName(string value)
        {
            var filter = GetCollectionView();  // получаем представление всей коллекции
            filter.Filter = d => ((Departament)d).DepartamentName == value;  // фильтруем коллекцию
            return filter;
        }

        #endregion

        /// <summary>
        /// Получить отдел его по наименованию.
        /// </summary>
        /// <param name="departamentName">Наименование.</param>
        /// <returns>Отдел.</returns>
        public static Departament GetDepartamentByDepartamentName(string departamentName)
        {
            Departament departament = new Departament();

            var query = from d in myDbContext.Departaments
                        where d.DepartamentName == departamentName
                        select d;

            foreach (Departament d in query)
                departament = d;

            return departament;
        }

        /// <summary>
        /// Реализация интерфейса INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Проверка на изменение значений.
        /// </summary>
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
