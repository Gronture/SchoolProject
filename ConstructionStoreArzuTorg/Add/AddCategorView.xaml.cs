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
    /// Логика взаимодействия для AddCategorView.xaml
    /// </summary>
    public partial class AddCategorView : Window
    {
        public AddCategorView()
        {
            InitializeComponent();
        }
        
        private void AddPositionButton_Click(object sender, RoutedEventArgs e)
        {
            // проверка на заполнение TextBox
            foreach (var control in grid.Children)
            {
                if (control is TextBox)
                {
                    var textbox = (TextBox)control;
                    if (textbox.Text == string.Empty)
                    {
                        MessageBox.Show("Не заполнены текстовые поля");
                        return;
                    }

                }
            }
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                //добавление категории
                Категория категория = new Категория();
                категория.Название = NameTextBox.Text;
                db.Категория.Add(категория);
                db.SaveChanges();
            }
            //переход на окно категорий
            new CategorListView().Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new CategorListView().Show();
            Close();
        }
    }
}
