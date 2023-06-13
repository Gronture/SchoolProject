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
    /// Логика взаимодействия для AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        public AddOrder()
        {
            InitializeComponent();
            Load();
        }
        //подгрузка в комбобокс фамилию и имя клиента
        private void Load()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                ClientComboBox.ItemsSource = db.Клиент.Select(x => x.Фамилия + " " + x.Имя).ToList();

              
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //проверка на заполнение комбобоксов
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
                    //добавление заказа
                    var zakaz = new Заказ();
                    zakaz.ID_Клиента = db.Клиент.Where(x => x.Фамилия + " " + x.Имя == ClientComboBox.Text).FirstOrDefault().ID_Клиента;
                    var data = db.LogsInfo.ToList(); 
                    zakaz.ID_Сотрудника =data.Last().Worker;
                    zakaz.Дата_заказа = DateTime.Now;

                    db.Заказ.Add(zakaz);
                    db.SaveChanges();

                    new AddProductToOrder(zakaz).Show();
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
            //переход на окно заказов
            new OrderListView().Show();
            Close();
        }
    }
}
