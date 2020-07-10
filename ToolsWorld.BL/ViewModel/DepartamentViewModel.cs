using System.Collections.Generic;
using System.ComponentModel;
using ToolsWorld.BL.Model;

namespace ToolsWorld.BL.ViewModel
{
    public class DepartamentViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Выбранный отдел.
        /// </summary>
        Departament selectedDepartament;

        /// <summary>
        /// Коллекция отделов.
        /// </summary>
        public ICollection<Departament> Departaments { get; set; }

        public DepartamentViewModel()
        {
            Departaments = Departament.GetDepartaments();
        }

        /// <summary>
        /// Выбранный отдел.
        /// </summary>
        public Departament SelectedDepartament
        {
            get { return selectedDepartament; }
            set
            {
                selectedDepartament = value;
                OnPropertyChanged("SelectedDepartament");
            }
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
