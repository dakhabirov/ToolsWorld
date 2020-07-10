using System.Collections.Generic;
using System.ComponentModel;
using ToolsWorld.BL.Model;

namespace ToolsWorld.BL.ViewModel
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Выбранный заказ.
        /// </summary>
        Order selectedOrder;

        /// <summary>
        /// Коллекция заказов.
        /// </summary>
        public ICollection<Order> Orders { get; set; }

        public OrderViewModel()
        {
            Orders = Order.GetOrders();
        }

        /// <summary>
        /// Выбранный заказ.
        /// </summary>
        public Order SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                selectedOrder = value;
                OnPropertyChanged("SelectedOrder");
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
