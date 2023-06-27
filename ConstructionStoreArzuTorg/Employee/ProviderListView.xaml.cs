using ConstructionStoreArzuTorg.Add;
using ConstructionStoreArzuTorg.Edit;
using ConstructionStoreArzuTorg.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ConstructionStoreArzuTorg.Employee
{
    /// <summary>
    /// Логика взаимодействия для ProviderListView.xaml
    /// </summary>
    public partial class ProviderListView : Window
    {
        public ProviderListView()
        {
            InitializeComponent();
            UpdateView();
        }

        public void UpdateView()
        {
            using(ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                grid.ItemsSource = db.Поставщик.ToList(); 
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new AddProvider().Show();
            Close();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grid.SelectedItem as Поставщик;
                if (selectedItem != null)
                {
                    new EditProviderView(selectedItem).Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Выберите запись которую хотите изменить");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при изменении поставщика");
                return;
            }
            
            
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedElement = grid.SelectedItem as Поставщик;
                if (selectedElement != null)
                {
                    using (ConstructionStoreEntities db = new ConstructionStoreEntities())
                    {
                        var findElement = db.Поставщик.Where(x => x.ID_Поставщика == selectedElement.ID_Поставщика).FirstOrDefault();
                        db.Поставщик.Remove(findElement);
                        db.SaveChanges();
                    }
                    UpdateView();
                }
                else
                {
                    MessageBox.Show("Выберите запись которую хотите удалить");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при удалении поставщика");
                return;
            }
            
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ManagerMenuView().Show();
            Close();
        }
    }
}
