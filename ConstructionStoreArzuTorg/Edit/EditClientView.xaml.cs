using ConstructionStoreArzuTorg.Add;
using ConstructionStoreArzuTorg.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для EditClientView.xaml
    /// </summary>
    public partial class EditClientView : Window
    {
        Клиент _client;
        public EditClientView(Клиент client)
        {
            InitializeComponent();
            _client = client;
        }
        //загрузка данных в текст боксы
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FirstNameTextBox.Text = _client.Фамилия;
            SecondNameTextBox.Text = _client.Имя;
            LastNameTextBox.Text = _client.Отчество;
            PhoneTextBox.Text = _client.Телефон;
            AddressTextBox.Text = _client.Адрес;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ClientListView().Show();
            Close();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            //проверка на заполнение текстбоксов
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
            string pattern = @"^\+375\d{9}$";
            bool isMatch = Regex.IsMatch(number, pattern);
            if (isMatch)
            {
                using (ConstructionStoreEntities db = new ConstructionStoreEntities())
                {
                    //сохранение изменений клиента
                    var needObject = db.Клиент.Where(x => x.ID_Клиента == _client.ID_Клиента).FirstOrDefault();
                    if (needObject != null)
                    {
                        needObject.Фамилия = FirstNameTextBox.Text;
                        needObject.Имя = SecondNameTextBox.Text;
                        needObject.Отчество = LastNameTextBox.Text;
                        needObject.Телефон = PhoneTextBox.Text;
                        needObject.Адрес = AddressTextBox.Text;
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                MessageBox.Show("Ошибка при вводе телефона");
            }
            
            new ClientListView().Show();
            Close();
        }
    }
}
