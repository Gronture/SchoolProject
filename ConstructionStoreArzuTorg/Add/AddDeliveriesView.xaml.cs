using ConstructionStoreArzuTorg.Employee;
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
    /// Логика взаимодействия для AddDeliveriesView.xaml
    /// </summary>
    public partial class AddDeliveriesView : Window
    {
        public AddDeliveriesView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using(ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
               
                ProviderComboBox.ItemsSource = db.Поставщик.Select(x => x.Наименование).ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var control in grid.Children)
            {
               
                if (control is ComboBox)
                {
                    var comboBox = (ComboBox)control;
                    if (comboBox.SelectedValue == null || comboBox.SelectedValue.ToString() == string.Empty)
                    {
                        MessageBox.Show("Ошибка");
                        return;
                    }
                }
                
            }
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                try
                {

                    var postavka = new Поставки();
                    postavka.Поставщик = db.Поставщик.Where(x => x.Наименование == ProviderComboBox.Text).FirstOrDefault().ID_Поставщика;
                    var data = db.LogsInfo.ToList();
                    postavka.Сотрудник =  data.Last().Worker;
                    postavka.Дата = DateTime.Now;

                    db.Поставки.Add(postavka);
                    db.SaveChanges();

                    new AddProductToDeliveries(postavka).Show();
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Ошибка");
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new DeliveriesListView().Show();
            Close();
        }

    }
}
