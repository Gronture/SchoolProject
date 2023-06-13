using ConstructionStoreArzuTorg.Add;
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

namespace ConstructionStoreArzuTorg.Manager
{
    /// <summary>
    /// Логика взаимодействия для CategorListView.xaml
    /// </summary>
    public partial class CategorListView : Window
    {
        public CategorListView()
        {
            InitializeComponent();
            UpdateView();
        }

        public void UpdateView()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                grid.ItemsSource = db.Категория.ToList();
            }
        }
        private void AddPositionButton_Click(object sender, RoutedEventArgs e)
        {
            new AddCategorView().Show();
            Close();
        }

        private void DeletePositionButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedElement = grid.SelectedItem as Категория;
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var findElement = db.Категория.Where(x => x.ID_Категории == selectedElement.ID_Категории).FirstOrDefault();
                db.Категория.Remove(findElement);
                db.SaveChanges();
            }
            UpdateView();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ManagerMenuView().Show();
            Close();
        }
    }
}
