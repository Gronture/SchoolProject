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

namespace SchoolProject
{
    /// <summary>
    /// Логика взаимодействия для PositionView.xaml
    /// </summary>
    public partial class PositionView : Window
    {
        public PositionView()
        {
            InitializeComponent();
            UpdateView();
        }
        public void UpdateView()
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                grid.ItemsSource = db.Должность.ToList();
            }
        }
        public List<Должность> GetPosition()
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var returnList = db.Должность.ToList();
                return returnList;
            }
        }

        private void BackMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedElement = grid.SelectedItem as Должность;
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var findElement = db.Должность.Where(x => x.ID_Должности == selectedElement.ID_Должности).FirstOrDefault();
                db.Должность.Remove(findElement);
                db.SaveChanges();
            }
            UpdateView();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new AddPosition().Show();
            this.Close();
        }

        private void Redact_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = grid.SelectedItem as Должность;
            new EditPosition(selectedItem).Show();
            this.Close();
        }

        private void SortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var list = GetPosition();
            grid.ItemsSource = list.Where(x => x.Название.ToLower().Contains(SortTextBox.Text)).ToList();
        }
    }
}
