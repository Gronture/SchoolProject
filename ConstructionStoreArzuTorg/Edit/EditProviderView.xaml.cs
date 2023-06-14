using ConstructionStoreArzuTorg.Add;
using ConstructionStoreArzuTorg.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для EditProviderView.xaml
    /// </summary>
    public partial class EditProviderView : Window
    {
        Поставщик _provider;
        public EditProviderView(Поставщик provider)
        {
            InitializeComponent();
            _provider = provider;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = _provider.Наименование;
            RSTextBox.Text = _provider.Расчётный_счёт.ToString();
            NumPlatTextBox.Text = _provider.Учётный_номер_плательщика.ToString();
            NameBankTextBox.Text = _provider.Название_банка.ToString();
            CodeBankTextBox.Text = _provider.Код_банка.ToString();
            AddressTextBox.Text = _provider.Адрес.ToString();
            FIOTextBox.Text = _provider.ФИО.ToString();
            PosTextBox.Text = _provider.Должность.ToString();
            EmailTextBox.Text = _provider.Адрес.ToString();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
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

                var number = PhoneTextBox.Text;
                string patternphone = @"^\+375\d{9}$";
                bool isPhone = Regex.IsMatch(number, patternphone);

                var email = EmailTextBox.Text;
                string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                bool isMatch = Regex.IsMatch(email, pattern);

                if (isMatch)
                {
                    if (isPhone)
                    {
                        var needObject = db.Поставщик.Where(x => x.ID_Поставщика == _provider.ID_Поставщика).FirstOrDefault();
                        if (needObject != null)
                        {
                            needObject.Наименование = NameTextBox.Text;
                            needObject.Расчётный_счёт = RSTextBox.Text;
                            needObject.Учётный_номер_плательщика = NumPlatTextBox.Text;
                            needObject.Название_банка = NameBankTextBox.Text;
                            needObject.Код_банка = CodeBankTextBox.Text;
                            needObject.Адрес = AddressTextBox.Text;
                            needObject.ФИО = FIOTextBox.Text;
                            needObject.Телефон = PhoneTextBox.Text;
                            needObject.Должность = PosTextBox.Text;
                            needObject.Почта = EmailTextBox.Text;
                            db.SaveChanges();
                        }
                    }

                    else
                    {
                        MessageBox.Show("Ошибка при вводе телефона");
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка при вводе почты");
                }
            }
                
            new ProviderListView().Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ProviderListView().Show();
            Close();
        }
    }
}
