using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EyesWPF.Model;
using EyesWPF.Utils;

namespace EyesWPF.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddSale.xaml
    /// </summary>
    public partial class AddSale : Window
    {
        private ProductSale newProdSale = new ProductSale();

        public AddSale(int idAgent)
        {
            InitializeComponent();

            DataContext = newProdSale;
            newProdSale.AgentID = idAgent;
            TitleProdBox.ItemsSource = Transition.Context.Product.ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (newProdSale.Product == null)
                error.AppendLine("Выберите продукт");

            try
            {
                newProdSale.ProductCount = Convert.ToInt32(CountExcep.Text);
                if (newProdSale.ProductCount < 0)
                    error.AppendLine("Количество продукции не может быть отрицательным");
            }
            catch (Exception)
            {
                error.AppendLine("Количество продукции должно быть целым");
            }

            if (string.IsNullOrWhiteSpace(newProdSale.SaleDate.ToString()))
                error.AppendLine("Введите дату продажи правильно: ММ.ДД.ГГ");

            if (error.Length > 0)
            {
                MessageBox.Show("При сохранении допущены следующие ошибки:\n" + error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newProdSale.ID == 0)
                Transition.Context.ProductSale.Add(newProdSale);

            try
            {
                Transition.Context.SaveChanges();
                Transition.Context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DialogResult = true;
                MessageBox.Show("Данные сохранены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show("Возникла ошибка при сохранении: " + er.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
