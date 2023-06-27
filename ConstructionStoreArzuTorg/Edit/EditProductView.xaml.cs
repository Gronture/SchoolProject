using ConstructionStoreArzuTorg.Add;
using ConstructionStoreArzuTorg.ClassConnection;
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

namespace ConstructionStoreArzuTorg.Edit
{
    /// <summary>
    /// Логика взаимодействия для EditProductView.xaml
    /// </summary>
    public partial class EditProductView : Window
    {
        ProductUpd _product = new ProductUpd();
        public EditProductView(ProductUpd product)
        {
            InitializeComponent();
            _product = product;
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
                DiscountComboBox.ItemsSource = data;
            }
            NameTextBox.Text = _product.Название;
            CategoryComboBox.Text = _product.НазваниеКатегории;
            DimensionsComboBox.Text = _product.Размеры;
            UnitComboBox.Text = _product.ЕдиницаИзмерения;
            PriceTextBox.Text = _product.Стоимость.ToString();
            DiscountComboBox.Text = _product.Сезонность.ToString();
        }

        private void AcceptProductButton_Click(object sender, RoutedEventArgs e)
        {
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
                if (control is ComboBox)
                {
                    var comboBox = (ComboBox)control;
                    if (comboBox.SelectedValue == null || comboBox.SelectedValue.ToString() == string.Empty)
                    {
                        MessageBox.Show("Не выбраны параметры товара");
                        return;
                    }
                }
            }
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var numbers = DiscountComboBox.SelectedItem.ToString().Where(x => Char.IsDigit(x)).ToList();
                string combinedNumber = string.Join("", numbers);
                int result = int.Parse(combinedNumber);


                string firstWord = DiscountComboBox.SelectedItem.ToString().Split('/')[0].Trim();
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
                var needObject = db.Товар.Where(x => x.ID_Товара == _product.ID_Товара).FirstOrDefault();
                if (needObject != null)
                {
                    needObject.ID_Категории = db.Категория.Where(x => x.Название == CategoryComboBox.Text).FirstOrDefault().ID_Категории;
                    needObject.ID_Размеров = db.РазмерыТовара.Where(x => x.Размер == DimensionsComboBox.Text).FirstOrDefault().ID_Размеров;
                    needObject.ID_Единицы_измерения = db.Единицы_измерения.Where(x => x.Название == UnitComboBox.Text).FirstOrDefault().ID_Измерений;
                    needObject.Сезонность = item.ID;
                    needObject.Название = NameTextBox.Text;
                    needObject.Стоимость = decimal.Parse(PriceTextBox.Text);
                    db.SaveChanges();
                }
            }
            new ProductListView().Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ProductListView().Show();
            Close();
        }
    }
}
