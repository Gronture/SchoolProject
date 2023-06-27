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
    /// Логика взаимодействия для EdIzmListView.xaml
    /// </summary>
    public partial class EdIzmListView : Window
    {
        public EdIzmListView()
        {
            InitializeComponent();
            UpdateView();
        }
        public void UpdateView()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                grid.ItemsSource = db.Единицы_измерения.ToList();
            }
        }
        private void AddPositionButton_Click(object sender, RoutedEventArgs e)
        {
            new AddEdIzmView().Show();
            Close();
        }

        private void DeletePositionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedElement = grid.SelectedItem as Единицы_измерения;
                if (selectedElement != null)
                {
                    using (ConstructionStoreEntities db = new ConstructionStoreEntities())
                    {
                        var findElement = db.Единицы_измерения.Where(x => x.ID_Измерений == selectedElement.ID_Измерений).FirstOrDefault();
                        db.Единицы_измерения.Remove(findElement);
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
                MessageBox.Show("Ошибка при удалении единицы измерения");
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
