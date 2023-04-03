using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Zadanie1
{
    public partial class MainWindow : Window
    {
        int currentPage = 1;
        int totalItems = 0;

        const int itemsPerPage = 10;

        List<AgentsView> agentsView = new();
        List<Agents> agents = new();
        List<AgentsView> sortedAgents = new();
        Agents selectedAgent;

        public MainWindow()
        {
            InitializeComponent();

            Pleasant_rustle_DBEntities db = new();
            agentsView = db.Agents.ToArray().Select(x => new AgentsView(x.ID_Agent, x.Type_Agents.type + " | " + x.name, x.Product_Sale.Count + " продаж за год", x.logo, x.Product_Sale.Count,
                x.phone, "Приоритетность: " + x.priority.ToString(), x.priority, x.name, x.Type_Agents.ID)).ToList();

            agents = db.Agents.ToList();

            lstAgents.ItemsSource = agentsView.Take(itemsPerPage);

            totalItems = agentsView.Count;

            List<FilterModel> filterList = db.Type_Agents.ToArray().Select(x => new FilterModel(x.ID, x.type)).ToList();

            filterList.Add(new FilterModel(id_type_agent: 0, name: "Все"));

            filterList = filterList.OrderBy(x => x.ID_Type_agent).ToList();

            Filter.ItemsSource = filterList;
            Filter.SelectedIndex = 0;

            Sort.SelectedIndex = 0;

            db.Dispose();
        }

        public void SortAgentList()
        {
            sortedAgents = agentsView;

            if (!String.IsNullOrEmpty(Search.Text))
            {
                sortedAgents = sortedAgents.Where(x => x.TypeAndName.ToLower().Contains(Search.Text.ToLower())).ToList();
            }

            switch (Sort.SelectedIndex)
            {
                case 0:
                    sortedAgents = sortedAgents.OrderBy(x => x.Name).ToList(); break;
                case 1:
                    sortedAgents = sortedAgents.OrderByDescending(x => x.Name).ToList(); break;
                case 2:
                    sortedAgents = sortedAgents.OrderBy(x => x.SaleNumber).ToList(); break;
                case 3:
                    sortedAgents = sortedAgents.OrderByDescending(x => x.SaleNumber).ToList(); break;
                case 4:
                    sortedAgents = sortedAgents.OrderBy(x => x.PriorityId).ToList(); break;
                case 5:
                    sortedAgents = sortedAgents.OrderByDescending(x => x.PriorityId).ToList(); break;
            }

            FilterModel selectedItem = Filter.SelectedItem as FilterModel;

            if (selectedItem is null)
            {
                Filter.SelectedIndex = 0;
            }

            if (selectedItem is not null && selectedItem.ID_Type_agent != 0)
            {
                sortedAgents = sortedAgents.Where(x => x.TypeId == selectedItem.ID_Type_agent).ToList();
            }

            currentPage = 1;
            CurrentPage.Text = currentPage.ToString();
            totalItems = sortedAgents.Count;
            SetAgents();
        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortAgentList();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            SortAgentList();
        }

        private void Previous_page_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage == 1) return;

            lstAgents.ItemsSource = agentsView.Take(itemsPerPage).ToList();

            currentPage--;
            SetAgents();
            CurrentPage.Text = currentPage.ToString();
        }

        private void Next_page_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage + 1 > Math.Ceiling(((Double)totalItems / (Double)itemsPerPage))) return;

            currentPage++;
            SetAgents();
            CurrentPage.Text = currentPage.ToString();
        }

        public void SetAgents()
        {
            lstAgents.ItemsSource = sortedAgents.Skip((currentPage - 1) * itemsPerPage).ToArray().Take(itemsPerPage).ToList();
        }

        private void addAgent_Click(object sender, RoutedEventArgs e)
        {
            AddAgent addAgent = new(agent: null);
            addAgent.Show();
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstAgents.SelectedItem == null)
                MessageBox.Show("Выберите агента, щелкнув по нему левой кнопкой мыши");
            else
            {
                Agents? selectedAgent = agents.Where(a => a.ID_Agent == ((AgentsView)lstAgents.SelectedItem).Id).FirstOrDefault();

                if (selectedAgent is null) return;

                AddAgent addAgent = new(selectedAgent);
                addAgent.ShowDialog();
            }
        }

        private void ListViewItem_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // TAKS/TODO: Доделать удаление (чтобы удалялось из базы, из списка, и список обновлялся после удаления)

            Pleasant_rustle_DBEntities db = new();

            var agentForRemoving = (AgentsView)lstAgents.SelectedItem;

            var agent = db.Agents.Where(p => p.ID_Agent == agentForRemoving.Id).FirstOrDefault();

            if (MessageBox.Show($"Вы точно хотите удалить {agent.name}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    db.Agents.Remove(agent);
                    db.SaveChanges();
                    MessageBox.Show("Данные удалены");
                    SetAgents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
