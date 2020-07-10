using System;
using System.Windows;
using ToolsWorld.BL.Model;

namespace ToolsWorld.WPF
{
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        readonly Authentication authentication = new Authentication();

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool check = authentication.CheckData(logTextBox.Text, passBox.Password);
            if (check == true)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.LogTextBlock.Text = authentication.CurrentLog;
                mainWindow.FullnameTextBlock.Text = authentication.CurrentFullname;
                mainWindow.LvlTextBlock.Text = authentication.CurrentLvl.ToString();
                mainWindow.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Имя пользователя или пароль введены неверно!");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
