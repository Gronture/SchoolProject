using ConstructionStoreArzuTorg.Employee;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using System.Collections.ObjectModel;
using Syncfusion.Windows.Shared.Resources;
using System;

namespace ConstructionStoreArzuTorg.Add
{
    /// <summary>
    /// Логика взаимодействия для AddProductView.xaml
    /// </summary>
    public partial class AddProductView : Window
    {
        public AddProductView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var firstlist = db.Категория.ToList();
                foreach (var item in firstlist)
                    CategoryComboBox.Items.Add(item.Название);

                var secontlist = db.РазмерыТовара.ToList();
                foreach (var item in secontlist)
                    DimensionsComboBox.Items.Add(item.Размер);

                var thristlist = db.Единицы_измерения.ToList();
                foreach (var item in thristlist)
                    UnitComboBox.Items.Add(item.Название);

                var list = db.Сезонность.ToList();
                var data = list.Select(x => x.Название_сезона + " / скидка " + x.Процент);
                NameSeasonComboBox.ItemsSource = data;


            }
        }
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
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


                var numbers = NameSeasonComboBox.SelectedItem.ToString().Where(x => Char.IsDigit(x)).ToList();
                string combinedNumber = string.Join("", numbers);
                int result = int.Parse(combinedNumber);


                string firstWord = NameSeasonComboBox.SelectedItem.ToString().Split('/')[0].Trim();
                Сезонность item = new Сезонность();
                switch (firstWord)
                {
                    case "Лето":
                        item = db.Сезонность.FirstOrDefault(x => x.Название_сезона == "Лето" && x.Процент == result);
                        break;
                    case "Зима":
                        item = db.Сезонность.FirstOrDefault(x => x.Название_сезона == "Зима" && x.Процент == result);
                        break;
                    case "Весна":
                        item = db.Сезонность.FirstOrDefault(x => x.Название_сезона == "Весна" && x.Процент == result);
                        break;
                    case "Осень":
                        item = db.Сезонность.FirstOrDefault(x => x.Название_сезона == "Осень" && x.Процент == result);
                        break;

                }

                Товар product = new Товар();
                
                var category = db.Категория.Where(x => x.Название == CategoryComboBox.Text).FirstOrDefault();
                var dimensions = db.РазмерыТовара.Where(x => x.Размер == DimensionsComboBox.Text).FirstOrDefault();
                var unit = db.Единицы_измерения.Where(x => x.Название == UnitComboBox.Text).FirstOrDefault();
               
                product.ID_Категории = category.ID_Категории;
                product.ID_Размеров = dimensions.ID_Размеров;
                product.ID_Единицы_измерения = unit.ID_Измерений;
                product.Сезонность = item.ID;
                product.Название = NameTextBox.Text;
                product.Стоимость = int.Parse(PriceTextBox.Text);

                db.Товар.Add(product);
                db.SaveChanges();
            }
            new ProductListView().Show();
            Close();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ProductListView().Show();
            Close();
        }

        private void NameSeasonComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
               



            }
        }
    }
}
