using System.Windows;
using ToolsWorld.BL.Model;
namespace ToolsWorld.WPF.View
{
    public partial class SupplierWindow : Window
    {
        public SupplierWindow()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (EnterButton.Content.ToString() == "Добавить")
            {
                Insert();
            }
            else if (EnterButton.Content.ToString() == "Сохранить")
            {
                Update();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Insert()
        {
            string supplierName = SupplierNameTextBox.Text;
            string tel = TelTextBox.Text;

            // проверяем на уникальность
            Supplier supplier = Supplier.GetSupplierBySupplierName(supplierName);
            if (supplier.SupplierName == supplierName)
            {
                MessageBox.Show($"Поставщик {supplierName} уже зарегистрирован!", "Ошибка");
            }
            else
            {
                Supplier.Insert(supplierName, tel);
                MessageBox.Show($"Поставщик {supplierName} успешно добавлен в базу данных.", "Сообщение");
                Close();
            }
        }

        private void Update()
        {
            string supplierName = SupplierNameTextBox.Text;
            string tel = TelTextBox.Text;

            Supplier.Update(supplierName, tel);

            Supplier.Update(supplierName, tel);
            MessageBox.Show($"Данные {supplierName} поставщика успешно обновлены.", "Сообщение");
            Close();
        }
    }
}
