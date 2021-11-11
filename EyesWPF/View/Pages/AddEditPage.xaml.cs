using System;
using System.Collections.Generic;
using System.IO;
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
using EyesWPF.View.Windows;
using Microsoft.Win32;

namespace EyesWPF.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        //Создание экземпляра newAgent
        private Agent newAgent = new Agent();

        public AddEditPage(Agent selectedAgent)
        {
            InitializeComponent();

            if (selectedAgent != null)
            {
                newAgent = selectedAgent;

                BtnDelAgent.Visibility = Visibility.Visible;
                BtnAdd.Visibility = Visibility.Visible;
                BtnDel.Visibility = Visibility.Visible;
                AgentSaleGrid.ItemsSource = Transition.Context.ProductSale.ToList()
                    .Where(p => p.AgentID == selectedAgent.ID);
            }

            DataContext = newAgent;
            AgentTypeBox.ItemsSource = Transition.Context.AgentType.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (string.IsNullOrWhiteSpace(newAgent.Title))
                error.AppendLine("Введите наименование");
            if (newAgent.AgentType == null)
                error.AppendLine("Выберите тип агента");

            try
            {
                newAgent.Priority = Convert.ToInt32(PriorExcep.Text);
                if (newAgent.Priority < 0)
                    error.AppendLine("Приоритет не может быть отрицательным");
            }
            catch (Exception)
            {
                error.AppendLine("Приоритет должен быть целым");
            }

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
                Transition.MainFrame.GoBack();
            }
            catch (Exception er)
            {
                MessageBox.Show("Возникла ошибка при сохранении: " + er.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddSale addSale = new AddSale(newAgent.ID);
            if (addSale.ShowDialog() == true)
                AgentSaleGrid.ItemsSource = Transition.Context.ProductSale.ToList()
                   .Where(p => p.AgentID == newAgent.ID);
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var dataRemoving = AgentSaleGrid.SelectedItems.Cast<ProductSale>().ToList();

            if (dataRemoving != null)
            {
                if (MessageBox.Show($"Вы действительно хотите удалить {dataRemoving.Count} элементов?",
                                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Transition.Context.ProductSale.RemoveRange(dataRemoving);
                        Transition.Context.SaveChanges();
                        AgentSaleGrid.ItemsSource = Transition.Context.ProductSale.ToList()
                            .Where(p => p.AgentID == newAgent.ID);
                        MessageBox.Show("Данные удалены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show("Возникла ошибка при сохранении: " + er.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
                MessageBox.Show("Выделите хотя бы одну запись для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\View\\agents\\"));

            openDialog.Filter = "Image file (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
            openDialog.InitialDirectory = path;

            if (openDialog.ShowDialog() == true)
            {
                if (!File.Exists(path + openDialog.SafeFileName))
                    File.Copy(openDialog.FileName, path + openDialog.SafeFileName);

                LogoBox.Text = $@"agents\{openDialog.SafeFileName}";
                newAgent.Logo = LogoBox.Text;
            }
        }

        private void BtnDelAgent_Click(object sender, RoutedEventArgs e)
        {
            var IsAgentRealize = Transition.Context.ProductSale.FirstOrDefault(p => p.AgentID == newAgent.ID);

            if (IsAgentRealize == null)
            {
                if (MessageBox.Show($"Вы действительно хотите удалить агента?",
                                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var delAgent = Transition.Context.Agent.FirstOrDefault(p => p.ID == newAgent.ID);
                    var delAgentPrior = Transition.Context.AgentPriorityHistory.ToList().Where(p => p.AgentID == newAgent.ID);
                    var delAgentShop = Transition.Context.Shop.ToList().Where(p => p.AgentID == newAgent.ID);

                    try
                    {
                        Transition.Context.AgentPriorityHistory.RemoveRange(delAgentPrior);
                        Transition.Context.Shop.RemoveRange(delAgentShop);
                        Transition.Context.Agent.Remove(delAgent);
                        Transition.Context.SaveChanges();

                        Transition.MainFrame.GoBack();
                        MessageBox.Show($"Все данные о агенте были удалены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show("Возникла ошибка при сохранении: " + er.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
                MessageBox.Show("Сначала необходимо удалить информацию о реализации продукта у данного агента",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
