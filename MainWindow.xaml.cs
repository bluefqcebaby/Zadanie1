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

namespace Zadanie1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        public MainWindow()
        {

            InitializeComponent();
            Pleasant_rustle_DBEntities db = new Pleasant_rustle_DBEntities();
            lstAgents.ItemsSource = db.Agents.Select(x => new
            {
                id = x.ID_Agent,
                typeAndName = x.Type_Agents.type + " | " + x.name,
                saleCount = x.Product_Sale.Count + " продаж за год",
                x.phone,
                priority = "Приоритетность: " + x.priority.ToString(),
            }).ToList();

            var filterList = db.Type_Agents.Select(x => new
            {
                ID_Type_agent = x.ID,
                Name = x.type
            }).ToList();

            filterList.Add(new
            {
                ID_Type_agent = 0,
                Name = "Все"
            });

            filterList = filterList.OrderBy(x => x.ID_Type_agent).ToList();

            Filter.ItemsSource = filterList;

            db.Dispose();
        }

        public void sort()
        {
            Pleasant_rustle_DBEntities db = new Pleasant_rustle_DBEntities();
            var list = db.Agents.Select(x => new
            {
                id = x.ID_Agent,
                typeAndName = x.Type_Agents.type + " | " + x.name,
                saleCount = x.Product_Sale.Count + " продаж за год",
                x.phone,
                priority = "Приоритетность: " + x.priority.ToString(),
                numPriority = x.priority,
                idType = x.Type_Agents.ID,
            }).ToList();

            if (!String.IsNullOrEmpty(Search.Text))
            {
                list = list.Where(x => x.typeAndName.ToLower().Contains(Search.Text.ToLower())).ToList();
            }

            switch (Sort.SelectedIndex)
            {
                case 0:
                    list = list.OrderBy(x => x.typeAndName).ToList(); break;
                case 1:
                    list = list.OrderByDescending(x => x.typeAndName).ToList(); break;
                case 2:
                    list = list.OrderBy(x => x.saleCount).ToList(); break;
                case 3:
                    list = list.OrderByDescending(x => x.saleCount).ToList(); break;
                case 4:
                    list = list.OrderBy(x => x.numPriority).ToList(); break;
                case 5:
                    list = list.OrderByDescending(x => x.numPriority).ToList(); break;
            }

            //FilterModel selectedItem = Filter.SelectedItem as new {
            //    ID_Type_age
            //    Name
            //};

           
            MessageBox.Show((Filter.SelectedItem as FilterModel).Name);
            


            //if (selectedItem.ID_Type_agent != 0)
            //{
            //    list = list.Where(x => x.idType == selectedItem.ID_Type_agent).ToList();
            //}

            lstAgents.ItemsSource = list;
        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sort();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            sort();
        }
    }
}
