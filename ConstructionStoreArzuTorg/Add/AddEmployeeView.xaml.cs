using ConstructionStoreArzuTorg.Manager;
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
    /// Логика взаимодействия для AddEmployeeView.xaml
    /// </summary>
    public partial class AddEmployeeView : Window
    {
        public AddEmployeeView()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
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
                        MessageBox.Show("Не выбрана должность");
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
                    Сотрудник emp = new Сотрудник();
                    var position = db.Должность.Where(x => x.Название == PositionComboBox.Text).FirstOrDefault();

                    emp.Фамилия = FirstNameTextBox.Text;
                    emp.Имя = SecondNameTextBox.Text;
                    emp.Отчество = LastNameTextBox.Text;
                    emp.Телефон = number;
                    emp.ID_Должности = position.ID_Должности;

                    db.Сотрудник.Add(emp);
                    db.SaveChanges();

                    new EmployeeListView().Show();
                    Close();
                }
            }

            else
            {
                MessageBox.Show("Ошибка при заполнении поля Телефон");
                return;
            }
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new EmployeeListView().Show();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var position = new List<Должность>();
            using(ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var tmpList = db.Должность.ToList();
                position.AddRange(tmpList);
            }
            foreach (var emp in position)
            {
                PositionComboBox.Items.Add(emp.Название);
            }
        }

        private void PhoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
