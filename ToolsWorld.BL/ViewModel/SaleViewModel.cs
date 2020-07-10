using System.Collections.Generic;
using System.ComponentModel;
using ToolsWorld.BL.Model;

namespace ToolsWorld.BL.ViewModel
{
    public class SaleViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Выбранная продажа.
        /// </summary>
        Sale selectedSale;

        /// <summary>
        /// Коллекция продаж.
        /// </summary>
        public ICollection<Sale> Sales { get; set; }

        public SaleViewModel()
        {
            Sales = Sale.GetSales();
        }

        /// <summary>
        /// Выбранная продажа.
        /// </summary>
        public Sale SelectedSale
        {
            get
            {
                return selectedSale;
            }
            set
            {
                selectedSale = value;
                OnPropertyChanged("SelectedSale");
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
