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
    /// Логика взаимодействия для ChangePriority.xaml
    /// </summary>
    public partial class ChangePriority : Window
    {
        private List<Agent> currentAgents;

        public ChangePriority(List<Agent> transmittedAgents)
        {
            InitializeComponent();

            currentAgents = transmittedAgents;
        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in currentAgents)
            {
                try
                {
                    item.Priority = Convert.ToInt32(TextPriority.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Введите число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            try
            {
                Transition.Context.SaveChanges();
                Transition.Context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                MessageBox.Show("Данные сохранены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                this.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show($"Возникла ошибка - {er.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
