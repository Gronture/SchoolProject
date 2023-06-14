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

namespace ConstructionStoreArzuTorg.Add
{
    /// <summary>
    /// Логика взаимодействия для AddClientView.xaml
    /// </summary>
    public partial class AddClientView : Window
    {
        public AddClientView()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ClientListView().Show();
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // проверка на заполнение TextBox
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
                    //добавление клиента
                    Клиент client = new Клиент();
                    client.Фамилия = FirstNameTextBox.Text;
                    client.Имя = SecondNameTextBox.Text;
                    client.Отчество = LastNameTextBox.Text;
                    client.Телефон = PhoneTextBox.Text;
                    client.Адрес = AddressTextBox.Text;
                    db.Клиент.Add(client);
                    db.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Ошибка при заполнении поля Телефон");
            }
            
            //переход на окно клиентов
            new ClientListView().Show();
            Close();
        }

    }
}
