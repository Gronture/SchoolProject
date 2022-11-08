using SchoolProject.ClassConnection;
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

namespace SchoolProject
{
    /// <summary>
    /// Логика взаимодействия для TeacherView.xaml
    /// </summary>
    public partial class TeacherView : Window
    {
        public TeacherView()
        {
            InitializeComponent();
            UpdateView();
            UpdateComboBox();
        }


        public void UpdateView()
        {
            using(ElectivesEntities db = new ElectivesEntities())
            {
                grid.ItemsSource = GetTeacher();
            }
        }
        public List<TeacherUpd> GetTeacher()
        {
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var result = db.Преподаватель.ToList().GroupJoin(
                    db.Должность.ToList(),
                    cl => cl.ID_Должности,
                    ci => ci.ID_Должности,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (teacher, speciality) => new TeacherUpd
                    {
                        Код_Преподавателя = teacher.cl.Код_Преподавателя,
                        Фамилия = teacher.cl.Фамилия,
                        Имя = teacher.cl.Имя,
                        Отчество = teacher.cl.Отчество,
                        Должность = speciality?.Название,
                        Телефон = teacher.cl.Телефон,
                        Табельный_номер = teacher.cl.Табельный_номер,
                        Пол = teacher.cl.Пол,
                        Стаж = teacher.cl.Стаж,
                    }).ToList();
                
                return result;
            }
        }

        public void UpdateComboBox()
        {
            var positionlites = new List<Должность>();
            using (ElectivesEntities db = new ElectivesEntities())
            {
                var tmpList = db.Должность.ToList();
                positionlites.AddRange(tmpList);
            }

            foreach (var i in positionlites) { FindComboBox.Items.Add(i.Название); }
        }

        private void BackMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectElement = grid.SelectedItem as TeacherUpd;
            using(ElectivesEntities db = new ElectivesEntities())
            {
                var findElement = db.Преподаватель.Where(x => x.Код_Преподавателя == selectElement.Код_Преподавателя).FirstOrDefault();
                db.Преподаватель.Remove(findElement);
                db.SaveChanges();
            }
            UpdateView();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new AddTeacher().Show();
            this.Close();
        }

        private void Redact_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = grid.SelectedItem as TeacherUpd;
            new EditTeacher(selectedItem).Show();
            this.Close();
        }

        private void SortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var list = GetTeacher();
            grid.ItemsSource = list.Where(x => x.Фамилия.ToLower().Contains(SortTextBox.Text)).ToList();

        }
        private void ColorRow(DataGrid dg)
        {
            for (int i = 0; i < dg.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(i);

                if (row != null)
                {
                    int index = row.GetIndex();
                    var date = dg.Items.GetItemAt(index) as TeacherUpd;

                    if(FindComboBox.Text != string.Empty)
                    {
                        if (date.Должность == FindComboBox.Text)
                        {

                            row.Background = Brushes.Coral;
                        }

                        else
                        {
                            row.Background = Brushes.White;
                        }
                    }
              
                }
            }
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            if (SortTextBox.Text == string.Empty)
                ColorRow(grid);
        }
    }
}
