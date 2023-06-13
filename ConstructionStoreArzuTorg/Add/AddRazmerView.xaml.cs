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
    /// Логика взаимодействия для AddRazmerView.xaml
    /// </summary>
    public partial class AddRazmerView : Window
    {
        public AddRazmerView()
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
                РазмерыТовара размеры = new РазмерыТовара();
                размеры.Размер = NameTextBox.Text;
                db.РазмерыТовара.Add(размеры);
                db.SaveChanges();
            }
            new RazmerListView().Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new RazmerListView().Show();
            Close();
        }
    }
}
