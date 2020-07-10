using System;
using System.Windows;
using ToolsWorld.BL.Model;

namespace ToolsWorld.WPF.View
{
    /// <summary>
    /// Логика взаимодействия для DepartamentWindow.xaml
    /// </summary>
    public partial class DepartamentWindow : Window
    {
        public DepartamentWindow()
        {
            InitializeComponent();
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
                MessageBox.Show("Проверь правильность введеных данных!", "Ошибка.");
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
            string departamentName = DepartamentNameTextBox.Text;

            // проверяем на уникальность
            Departament departament = Departament.GetDepartamentByDepartamentName(departamentName);
            if (departament.DepartamentName == departamentName)
            {
                MessageBox.Show($"{departamentName} уже зарегистрирован!", "Ошибка");
            }
            else
            {
                Departament.Insert(departamentName);
                MessageBox.Show($"{departamentName} успешно добавлен в базу данных.", "Сообщение");
                Close();
            }
        }

        /// <summary>
        /// Обновить.
        /// </summary>
        private void Update()
        {
            string oldDepartamentName = DepartamentNameTextBlock.Text;
            string newDepartamentName = DepartamentNameTextBox.Text;

            // проверяем на уникальность
            Departament departament = Departament.GetDepartamentByDepartamentName(oldDepartamentName);
            if (departament.DepartamentName == newDepartamentName)
            {
                MessageBox.Show($"{newDepartamentName} уже зарегистрирован!", "Ошибка");
            }
            else
            {
                Departament.Update(oldDepartamentName, newDepartamentName);
                MessageBox.Show($"Данные отдела успешно обновлены.", "Сообщение");
                Close();
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
