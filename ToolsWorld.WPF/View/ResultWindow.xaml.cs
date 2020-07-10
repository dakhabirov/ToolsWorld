using System;
using System.Collections.Generic;
using System.Windows;
using ToolsWorld.BL;
using ToolsWorld.BL.Model;
using ToolsWorld.BL.ViewModel;

namespace ToolsWorld.WPF.View
{
    public partial class ResultWindow : Window
    {
        public ResultWindow()
        {
            InitializeComponent();

            DataContext = new WorkerViewModel();
        }

        readonly DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("d"));

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Получить итоги продаж магазина за текущие сутки.
        /// </summary>
        public void GetCurrentDayResultToTextBlock()
        {
            double sum = Sale.GetDayResult(currentDate);
            TextBlock.Text = $"Сумма проданых товаров за сегодня {sum} руб:";
        }

        /// <summary>
        /// Получить итоги продаж сотрудников за текущие сутки.
        /// </summary>
        public void GetCurrentDayResultToListView()
        {
            string row;
            string depName;
            Dictionary<string, double> departamentsDictionary = new Dictionary<string, double>();
            List<Worker> workersList = Sale.GetWorkersWhoRealizedSaleOnDay(currentDate);

            // заполняем словарь
            try
            {
                foreach (Worker worker in workersList)
                {
                    depName = worker.Departament.DepartamentName;
                    double sum = Sale.GetWorkerDayResult(worker);
                    departamentsDictionary.Add(depName, sum);
                }

                // заполняем список
                foreach (KeyValuePair<string, double> keyValuePair in departamentsDictionary)
                {
                    row = $"{keyValuePair.Key}: {keyValuePair.Value} руб";
                    ListBox.Items.Add(row);
                }
            }
            catch
            {
                MessageBox.Show("Невозможно определить отдел сотрудника, проверьте данные сотрудника.", "Ошибка");
            }
        }

        /// <summary>
        /// Получить итоги продаж по месяцам.
        /// </summary>
        public void GetYearResultByDaysOfWeekToListView()
        {
            string record;
            Dictionary<string, double> dictionary = Sale.GetYearResultByDaysOfWeek(currentDate.Year);

            // заполняем список
            foreach (KeyValuePair<string, double> keyValuePair in dictionary)
            {
                record = $"{keyValuePair.Key}: {keyValuePair.Value} руб";
                ListBox.Items.Add(record);
            }
        }

        /// <summary>
        /// Получить итоги продаж магазина за текущий год.
        /// </summary>
        public void GetYearResultToTextBlock()
        {
            double sum = 0;
            for (int i = 0; i <= 12; i++)
            {
                sum += Sale.GetYearResult(currentDate.Year, i);
            }
            TextBlock.Text = $"Сумма проданых товаров за текущий год {sum} руб:";
        }

        /// <summary>
        /// Получить итоги продаж по месяцам.
        /// </summary>
        public void GetYearResultByMonthsToListView()
        {
            string record;
            Dictionary<string, double> dictionary = Sale.GetYearResultByMonths(currentDate.Year);

            // заполняем список
            foreach (KeyValuePair<string, double> keyValuePair in dictionary)
            {
                record = $"{keyValuePair.Key}: {keyValuePair.Value} руб";
                ListBox.Items.Add(record);
            }
        }

        /// <summary>
        /// Получить итоги продаж магазина за прошлый месяц.
        /// </summary>
        public void GetLastMonthResultToTextBlock()
        {
            double sum = Sale.GetLastMonthResult();
            TextBlock.Text = $"Сумма проданых товаров за прошлый месяц {sum} руб:";
        }

        /// <summary>
        /// Получить итоги продаж сотрудников за прошлый месяц.
        /// </summary>
        public void GetLastMonthResultToListView()
        {
            List<Worker> workersList = Sale.GetWorkersWhoRealizedSaleOnLastMonth();
            string fullname;
            double sum = 0;

            Dictionary<string, double> dictionary = new Dictionary<string, double>();

            string record;

            // заполняем словарь
            foreach (Worker worker in workersList)
            {
                fullname = worker.Fullname;
                double s = Sale.GetWorkerLastMonthResult(worker);

                try
                {
                    dictionary.Add(fullname, s);
                }
                catch
                {
                    sum += s;
                    dictionary[fullname] = sum;
                }
            }

            // заполняем список
            foreach (KeyValuePair<string, double> keyValuePair in dictionary)
            {
                record = $"{keyValuePair.Key}: {keyValuePair.Value} руб";
                ListBox.Items.Add(record);
            }
        }
    }
}