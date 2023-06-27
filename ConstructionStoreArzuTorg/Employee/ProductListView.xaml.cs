using ConstructionStoreArzuTorg.Add;
using ConstructionStoreArzuTorg.ClassConnection;
using ConstructionStoreArzuTorg.Edit;
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

namespace ConstructionStoreArzuTorg.Employee
{
    /// <summary>
    /// Логика взаимодействия для ProductListView.xaml
    /// </summary>
    public partial class ProductListView : Window
    {
        public ProductListView()
        {
            InitializeComponent();
            UpdateView();
        }
        //загрузка данных в дата грид
        public void UpdateView()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                grid.ItemsSource = GetProduct();
            }
        }
        //получение данных о товарах
        public List<ProductUpd> GetProduct()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                //подключение категории
                var firstJoin = db.Товар.ToList().GroupJoin(
                    db.Категория.ToList(),
                    cl => cl.ID_Категории,
                    ci => ci.ID_Категории,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (product, category) => new ProductUpd
                    {
                        ID_Товара = product.cl.ID_Товара,
                        Название = product.cl.Название,
                        НазваниеКатегории = category.Название,
                       

                    }).ToList();

                //подключение размеров товара
                var secondJoin = db.Товар.ToList().GroupJoin(
                    db.РазмерыТовара.ToList(),
                    cl => cl.ID_Размеров,
                    ci => ci.ID_Размеров,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (product, dimensions) => new ProductUpd
                    {
                        Размеры = db.РазмерыТовара.Where(x => x.ID_Размеров == dimensions.ID_Размеров).FirstOrDefault().Размер,
                    }).ToList();
                //подключение единиц измерения
                var thirdJoin = db.Товар.ToList().GroupJoin(
                    db.Единицы_измерения.ToList(),
                    cl => cl.ID_Единицы_измерения,
                    ci => ci.ID_Измерений,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (product, units) => new ProductUpd
                    {
                        ЕдиницаИзмерения = db.Единицы_измерения.Where(x => x.ID_Измерений == units.ID_Измерений).FirstOrDefault().Название,
                    }).ToList();
                //подключение скидки
                var lastJoin = db.Товар.ToList().GroupJoin(
                    db.Сезонность.ToList(),
                    cl => cl.Сезонность,
                    ci => ci.ID,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (product, units) => new ProductUpd
                    {
                        Стоимость = product.cl.Стоимость,
                        Сезонность = db.Сезонность.Where(x => x.ID == units.ID).FirstOrDefault().Название_сезона + " " + db.Сезонность.Where(x => x.ID == units.ID).FirstOrDefault().Процент,
                        СерийныйНомер = product.cl.Серийный_номер,
                        Гарантия = product.cl.Гарантия,
                        Стоимость_со_скидкой = product.cl.Стоимость_со_скидкой,
                    }).ToList();


                //занесение подключенных данных
                for (int i = 0; i < firstJoin.Count; i++)
                {
                    firstJoin[i].Размеры = secondJoin[i].Размеры;
                    firstJoin[i].ЕдиницаИзмерения = thirdJoin[i].ЕдиницаИзмерения;
                    firstJoin[i].Стоимость = Math.Round(lastJoin[i].Стоимость,2);
                    firstJoin[i].Сезонность = lastJoin[i].Сезонность;
                    firstJoin[i].СерийныйНомер = lastJoin[i].СерийныйНомер;
                    firstJoin[i].Гарантия = lastJoin[i].Гарантия;
                    firstJoin[i].Стоимость_со_скидкой = Math.Round((decimal)lastJoin[i].Стоимость_со_скидкой,2);
                }


                return firstJoin;
            }
        }

        private void SortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var list = GetProduct();
            grid.ItemsSource = list.Where(x => x.Название.ToLower().Contains(SortTextBox.Text)).ToList();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new EmployeeMenu().Show();
            this.Close();
        }

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grid.SelectedItem as ProductUpd;
                if (selectedItem != null)
                {
                    new EditProductView(selectedItem).Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Выберите запись которую хотите изменить");
                    return;
                }
                
            }
            catch
            {
                MessageBox.Show("Ошибка изменения товара");
                return;
            }
            
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            new AddProductView().Show();
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedElement = grid.SelectedItem as ProductUpd;
                if (selectedElement != null)
                {
                    using (ConstructionStoreEntities db = new ConstructionStoreEntities())
                    {
                        var findElement = db.Товар.Where(x => x.ID_Товара == selectedElement.ID_Товара).FirstOrDefault();
                        db.Товар.Remove(findElement);
                        db.SaveChanges();
                    }
                    UpdateView();
                }
                else
                {
                    MessageBox.Show("Выберите запись которую хотите удалить");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка удаления товара");
                return;
            }
            
        }
    }
}
