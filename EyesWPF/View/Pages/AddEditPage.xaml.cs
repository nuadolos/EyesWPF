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
using System.Windows.Navigation;
using System.Windows.Shapes;
using EyesWPF.Model;
using EyesWPF.Utils;

namespace EyesWPF.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Agent newAgent = new Agent();

        public AddEditPage(Agent selectedAgent)
        {
            InitializeComponent();

            if (selectedAgent != null)
                newAgent = selectedAgent;

            DataContext = newAgent;
            AgentTypeBox.ItemsSource = Transition.Context.AgentType.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (string.IsNullOrWhiteSpace(newAgent.Title))
                error.AppendLine("Введите наименование");
            if (newAgent.AgentTypeID == 0)
                error.AppendLine("Выберите тип агента");
            if (newAgent.Priority == 0)
                error.AppendLine("Введите приоритет");
            if (string.IsNullOrWhiteSpace(newAgent.Logo))
                error.AppendLine("Введите путь логотпа");
            if (string.IsNullOrWhiteSpace(newAgent.Address))
                error.AppendLine("Введите адрес");
            if (string.IsNullOrWhiteSpace(newAgent.INN))
                error.AppendLine("Введите ИНН");
            if (string.IsNullOrWhiteSpace(newAgent.KPP))
                error.AppendLine("Введите КПП");
            if (string.IsNullOrWhiteSpace(newAgent.DirectorName))
                error.AppendLine("Введите ФИО директора");
            if (string.IsNullOrWhiteSpace(newAgent.Phone))
                error.AppendLine("Введите телефон");
            if (string.IsNullOrWhiteSpace(newAgent.Email))
                error.AppendLine("Введите корпоративную почту");

            if (error.Length > 0)
            {
                MessageBox.Show("При сохранении допущены следующие ошибки:\n" + error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newAgent.ID == 0)
                Transition.Context.Agent.Add(newAgent);

            try
            {
                Transition.Context.SaveChanges();
                MessageBox.Show("Данные сохранены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception er)
            {
                MessageBox.Show("Возникла ошибка при сохранении: " + er.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
