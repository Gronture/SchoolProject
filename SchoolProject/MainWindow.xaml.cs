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
using SchoolProject;
namespace SchoolProject
{
    /// <summary>
    /// Логика взаимодействия для MAINWINDOW.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Students_Click(object sender, RoutedEventArgs e)
        {
            StudentView studentView = new StudentView();
            studentView.Show();
            this.Close();
        }

        private void EducationPlane_Click(object sender, RoutedEventArgs e)
        {
            EducationPlaneView educationPlaneView = new EducationPlaneView();
            educationPlaneView.Show();
            this.Close();
        }

        private void Electives_Click(object sender, RoutedEventArgs e)
        {
            ElectivesView electivesView = new ElectivesView();
            electivesView.Show();
            this.Close();
        }

        private void Directory_Click(object sender, RoutedEventArgs e)
        {
            DirectoryView directoryView = new DirectoryView();
            directoryView.Show();
            this.Close();
        }

        private void Teacher_Click(object sender, RoutedEventArgs e)
        {
            TeacherView teacherView = new TeacherView();
            teacherView.Show();
            this.Close();
        }

        private void Position_Click(object sender, RoutedEventArgs e)
        {
            PositionView positionView = new PositionView();
            positionView.Show();
            this.Close();
        }
    }
}
