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
using EyesWPF.Utils;
using EyesWPF.Model;
using EyesWPF.View.Windows;

namespace EyesWPF.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для AgentViewPage.xaml
    /// </summary>
    public partial class AgentViewPage : Page
    {
        private List<Agent> ItemsAgent { get { return Transition.Context.Agent.ToList(); } }

        private NavigateList navigate = new NavigateList();

        public AgentViewPage()
        {
            InitializeComponent();
            
            var allTypes = Transition.Context.AgentType.ToList();
            allTypes.Insert(0, new AgentType
            {
                Title = "Все типы"
            });
            ComboFilt.ItemsSource = allTypes;
            ComboFilt.SelectedIndex = 0;
            ComboSort.SelectedIndex = 0;

            navigate.EndIndex = ItemsAgent.Count;
            AgentView.ItemsSource = ItemsAgent.GetRange(navigate.StartIndex, navigate.CountOutAgents);
        }

        public void UpdateData()
        {
            var currentData = ItemsAgent;

            if (ComboFilt.SelectedIndex > 0)
                currentData = currentData.Where(p => p.AgentType.Title == (ComboFilt.SelectedValue as AgentType).Title).ToList();

            if (TextFilt.Text != "Введите для поиска")
                currentData = currentData.Where(p => p.Title.ToLower().Contains(TextFilt.Text.ToLower()) 
                || p.Phone.Contains(TextFilt.Text)
                || p.Email.ToLower().Contains(TextFilt.Text.ToLower())).ToList();

            switch (ComboSort.SelectedIndex)
            {
                case 1:
                    {
                        if (CheckOrder.IsChecked == false)
                            currentData = currentData.OrderBy(p => p.Title).ToList();
                        else
                            currentData = currentData.OrderByDescending(p => p.Title).ToList();
                        break;
                    }
                case 2:
                    {
                        if (CheckOrder.IsChecked == false)
                            currentData = currentData.OrderBy(p => p.Discount).ToList();
                        else
                            currentData = currentData.OrderByDescending(p => p.Discount).ToList();
                        break;
                    }
                case 3:
                    {
                        if (CheckOrder.IsChecked == false)
                            currentData = currentData.OrderBy(p => p.Priority).ToList();
                        else
                            currentData = currentData.OrderByDescending(p => p.Priority).ToList();
                        break;
                    }
            }

            navigate.EndIndex = currentData.Count;

            if (navigate.UsedBySearch)
                BtnNextPage.Visibility = Visibility.Hidden;
            else
                BtnNextPage.Visibility = Visibility.Visible;

            for (int i = navigate.CountOutAgents; i > 0; i--)
            {
                try
                {
                    AgentView.ItemsSource = currentData.GetRange(navigate.StartIndex, i);
                    break;
                }
                catch (Exception)
                {

                }
            }
        }

        private void TextFilt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextFilt.Text != "Введите для поиска")
            {
                navigate.NumberPage = 1;
                ControlOutList();
            }
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            navigate.NumberPage = 1;
            ControlOutList();
        }

        private void ComboFilt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            navigate.NumberPage = 1;
            ControlOutList();
        }

        private void TextFilt_GotFocus(object sender, RoutedEventArgs e)
        {
            TextFilt.Text = null;
        }

        private void TextFilt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextFilt.Text == "")
                TextFilt.Text = "Введите для поиска";
        }

        private void CheckOrder_Checked(object sender, RoutedEventArgs e)
        {
            navigate.NumberPage = 1;
            ControlOutList();
        }

        private void CheckOrder_Unchecked(object sender, RoutedEventArgs e)
        {
            navigate.NumberPage = 1;
            ControlOutList();
        }

        private void AgentView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AgentView.SelectedItems.Count > 1)
                PriorityChangeBtn.Visibility = Visibility.Visible;
            else
                PriorityChangeBtn.Visibility = Visibility.Hidden;
        }

        private void PriorityChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            var transmittedAgents = AgentView.SelectedItems.Cast<Agent>().ToList();
            ChangePriority priority = new ChangePriority(transmittedAgents);
            if (priority.ShowDialog() == true)
                UpdateData();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Transition.Context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                UpdateData();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Transition.MainFrame.Navigate(new AddEditPage(null));
        }

        private void AgentView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AgentView.SelectedItems.Count == 1)
                Transition.MainFrame.Navigate(new AddEditPage(AgentView.SelectedItem as Agent));
            else
                MessageBox.Show("Для изменения данных необходимо выделить только одну запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #region Кнопки, предназначенные для навигации по List<Agent>

        private void BtnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            navigate.NumberPage -= 1;
            ControlOutList();
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            navigate.NumberPage += 1;
            navigate.GetIndex();

            UpdateData();

            if (navigate.HasPreviousPage)
                BtnPreviousPage.Visibility = Visibility.Visible;
            if (navigate.NumberPage == navigate.TotalPage)
                BtnNextPage.Visibility = Visibility.Hidden;
        }

        private void BtnOnePage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnTwoPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnThreePage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnFourPage_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        private void ControlOutList()
        {
            navigate.GetIndex();

            if (!navigate.HasPreviousPage)
                BtnPreviousPage.Visibility = Visibility.Hidden;
            if (navigate.HasNextPage)
                BtnNextPage.Visibility = Visibility.Visible;

            UpdateData();
        }
    }
}
