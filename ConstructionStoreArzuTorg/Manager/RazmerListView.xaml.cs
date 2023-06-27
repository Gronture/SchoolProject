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
    /// Логика взаимодействия для RazmerListView.xaml
    /// </summary>
    public partial class RazmerListView : Window
    {
        public RazmerListView()
        {
            InitializeComponent();
            UpdateView();
        }
        public void UpdateView()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                grid.ItemsSource = db.РазмерыТовара.ToList();
            }
        }

        private void AddPositionButton_Click(object sender, RoutedEventArgs e)
        {
            new AddRazmerView().Show();
            Close();
        }

        private void DeletePositionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedElement = grid.SelectedItem as РазмерыТовара;
                if (selectedElement != null)
                {
                    using (ConstructionStoreEntities db = new ConstructionStoreEntities())
                    {
                        var findElement = db.РазмерыТовара.Where(x => x.ID_Размеров == selectedElement.ID_Размеров).FirstOrDefault();
                        db.РазмерыТовара.Remove(findElement);
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
                MessageBox.Show("Ошибка при удалении записи");
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
