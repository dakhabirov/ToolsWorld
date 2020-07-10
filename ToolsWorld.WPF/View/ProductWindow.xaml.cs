using System;
using System.Windows;
using ToolsWorld.BL.Model;

namespace ToolsWorld.WPF.View
{
    public partial class ProductWindow : Window
    {
        public ProductWindow()
        {
            InitializeComponent();

            DepartamentNamesComboBox.ItemsSource = Departament.GetDepartamentNames();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch
            {
                MessageBox.Show("Проверьте корректность введеных данных!", "Ошибка.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Добавить.
        /// </summary>
        private void Insert()
        {
            string productName = ProductNameTextBox.Text;
            int amount = Convert.ToInt32(AmountTextBox.Text);
            string unit = UnitComboBox.Text;
            double price = Convert.ToDouble(PriceTextBox.Text);
            int departamentId = Departament.GetDepartamentId(DepartamentNamesComboBox.Text);

            // проверяем на уникальность
            Product product = Product.GetProductByProductName(productName);
            if (product.ProductName == productName)
            {
                MessageBox.Show($"Товар с наименованием '{productName}' уже зарегистрирован!", "Ошибка");
            }
            else
            {
                Product.Insert(productName, amount, unit, price, departamentId);
                MessageBox.Show($"Товар '{productName}' успешно добавлен в базу данных.", "Сообщение");
                Close();
            }
        }

        /// <summary>
        /// Обновить.
        /// </summary>
        private void Update()
        {
            string productName = ProductNameTextBox.Text;
            int amount = Convert.ToInt32(AmountTextBox.Text);
            string unit = UnitComboBox.Text;
            double price = Convert.ToDouble(PriceTextBox.Text);
            int departamentId = Departament.GetDepartamentId(DepartamentNamesComboBox.Text);

            Product.Update(productName, amount, unit, price, departamentId);
            MessageBox.Show($"Данные товара успешно обновлены.", "Сообщение");
            Close();
        }
    }
}
