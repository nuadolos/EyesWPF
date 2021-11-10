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

            AgentView.ItemsSource = Transition.Context.Agent.ToList();
        }

        public void UpdateData()
        {
            var currentData = Transition.Context.Agent.ToList();

            if (ComboFilt.SelectedIndex > 0)
                currentData = currentData.Where(p => p.AgentType.Title == (ComboFilt.SelectedValue as AgentType).Title).ToList();

            if (TextFilt.Text != "Введите для поиска")
                currentData = currentData.Where(p => p.Title.ToLower().Contains(TextFilt.Text.ToLower()) 
                || p.Phone.Contains(TextFilt.Text)
                || p.Email.ToLower().Contains(TextFilt.Text.ToLower())).ToList();

            switch (ComboSort.SelectedIndex)
            {
                case 0:
                    AgentView.ItemsSource = currentData;
                    break;
                case 1:
                    if (CheckOrder.IsChecked == false)
                        AgentView.ItemsSource = currentData.OrderBy(p => p.Title);
                    else
                        AgentView.ItemsSource = currentData.OrderByDescending(p => p.Title);
                    break;
                case 2:
                    if (CheckOrder.IsChecked == false)
                        AgentView.ItemsSource = currentData.OrderBy(p => p.Discount);
                    else
                        AgentView.ItemsSource = currentData.OrderByDescending(p => p.Discount);
                    break;
                case 3:
                    if (CheckOrder.IsChecked == false)
                        AgentView.ItemsSource = currentData.OrderBy(p => p.Priority);
                    else
                        AgentView.ItemsSource = currentData.OrderByDescending(p => p.Priority);
                    break;
            }
        }

        private void TextFilt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextFilt.Text != "Введите для поиска")
                UpdateData();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void ComboFilt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void TextFilt_GotFocus(object sender, RoutedEventArgs e)
        {
            TextFilt.Text = null;
        }

        private void TextFilt_LostFocus(object sender, RoutedEventArgs e)
        {
            TextFilt.Text = "Введите для поиска";
        }

        private void CheckOrder_Checked(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void CheckOrder_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateData();
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
                AgentView.ItemsSource = Transition.Context.Agent.ToList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Transition.MainFrame.Navigate(new AddEditPage(null));
        }

        private void AgentView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Transition.MainFrame.Navigate(new AddEditPage(AgentView.SelectedItem as Agent));
        }
    }
}
