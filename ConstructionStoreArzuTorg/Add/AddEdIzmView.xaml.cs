using ConstructionStoreArzuTorg.Manager;
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

namespace ConstructionStoreArzuTorg.Add
{
    /// <summary>
    /// Логика взаимодействия для AddEdIzmView.xaml
    /// </summary>
    public partial class AddEdIzmView : Window
    {
        public AddEdIzmView()
        {
            InitializeComponent();
        }

        private void AddPositionButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var control in grid.Children)
            {
                if (control is TextBox)
                {
                    var textbox = (TextBox)control;
                    if (textbox.Text == string.Empty)
                    {
                        MessageBox.Show("Ошибка");
                        return;
                    }

                }
            }
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                Единицы_измерения единицы = new Единицы_измерения();
                единицы.Название = NameTextBox.Text;
                db.Единицы_измерения.Add(единицы);
                db.SaveChanges();
            }
            new EdIzmListView().Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new EdIzmListView().Show();
            Close();
        }
    }
}
