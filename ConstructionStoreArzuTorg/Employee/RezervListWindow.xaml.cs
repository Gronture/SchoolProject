﻿using ConstructionStoreArzuTorg.ClassConnection;
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
    /// Логика взаимодействия для RezervListWindow.xaml
    /// </summary>
    public partial class RezervListWindow : Window
    {
        public RezervListWindow()
        {
            InitializeComponent();
            Update();
        }

        public void Update()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                grid.ItemsSource = db.Резервация.ToList()
                      .GroupJoin(
                      db.Клиент.ToList(),
                      cl => cl.Клиент,
                      ci => ci.ID_Клиента,
                      (cl, ci) => new { cl, ci }).SelectMany(
                      x => x.ci.DefaultIfEmpty(),
                      (one, two) => new RezervUpd()
                      {
                          Цена = Math.Round(one.cl.Цена, 2),
                          Дата = one.cl.Дата,
                          ID = one.cl.ID,
                          ФамилияКлиента = two.Имя + " " + two.Фамилия,
                          DateString = one.cl.Дата.ToShortDateString(),
                          НазваниеСтатуса = db.Статус.FirstOrDefault(x => x.ID == one.cl.Статус).Название
                          
                      }).ToList();
            }
        }
        private void CreateRezervButton_Click(object sender, RoutedEventArgs e)
        {
            new RezervWindow().Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new EmployeeMenu().Show();
            Close();
        }

        public List<ProductUpd> GetProduct()
        {

            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
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

                var lastJoin = db.Товар.ToList().GroupJoin(
                    db.Сезонность.ToList(),
                    cl => cl.Сезонность,
                    ci => ci.ID,
                    (cl, ci) => new { cl, ci })
                    .SelectMany(x => x.ci.DefaultIfEmpty(),
                    (product, units) => new ProductUpd
                    {
                        Стоимость = product.cl.Стоимость,
                        Сезонность = db.Сезонность.Where(x => x.ID == units.ID).FirstOrDefault().Название_сезона,
                        СерийныйНомер = product.cl.Серийный_номер,
                        Гарантия = product.cl.Гарантия,
                        Стоимость_со_скидкой = product.cl.Стоимость_со_скидкой,
                    }).ToList();


                for (int i = 0; i < firstJoin.Count; i++)
                {
                    firstJoin[i].Размеры = secondJoin[i].Размеры;
                    firstJoin[i].ЕдиницаИзмерения = thirdJoin[i].ЕдиницаИзмерения;
                    firstJoin[i].Стоимость = lastJoin[i].Стоимость;
                    firstJoin[i].Сезонность = lastJoin[i].Сезонность;
                    firstJoin[i].СерийныйНомер = lastJoin[i].СерийныйНомер;
                    firstJoin[i].Гарантия = lastJoin[i].Гарантия;
                    firstJoin[i].Стоимость_со_скидкой = lastJoin[i].Стоимость_со_скидкой;
                }
                return firstJoin;
            }
        }
        public List<ProductUpd> GetProductUpds()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                return db.Товар
                  .Join(db.Единицы_измерения,
                      tovar => tovar.ID_Единицы_измерения,
                      pt => pt.ID_Измерений,
                      (tovar, pt) => new { Tovar = tovar, PT = pt })
                  .Join(db.РазмерыТовара,
                      joinResult => joinResult.Tovar.ID_Размеров,
                      param => param.ID_Размеров,
                      (joinResult, param) => new { Tovar = joinResult.Tovar, PT = joinResult.PT, Param = param })
                   .Join(db.Категория,
                      joinResult => joinResult.Tovar.ID_Категории,
                      param => param.ID_Категории,
                      (joinResult, param) => new { Tovar = joinResult.Tovar, PT = joinResult.PT, Param = param })
                   .Join(db.Сезонность,
                      joinResult => joinResult.Tovar.Сезонность,
                      param => param.ID,
                      (joinResult, param) => new { Tovar = joinResult.Tovar, PT = joinResult.PT, Param = param })
                   .Join(db.РезервацияТоваров,
                      joinResult => joinResult.Tovar.ID_Товара,
                      rezerv => rezerv.Товар,
                      (joinResult, rezerv) => new { Tovar = joinResult.Tovar, Param = joinResult.Param, Rezerv = rezerv, PT = joinResult.PT })
                  .Select(x => new ProductUpd
                  {
                      Название = x.Tovar.Название,
                      НазваниеКатегории = x.Tovar.Категория.Название,
                      Размеры = x.Tovar.РазмерыТовара.Размер,
                      ЕдиницаИзмерения = x.Tovar.Единицы_измерения.Название,
                      Сезонность = x.Param.Название_сезона,
                      Rezerv = x.Rezerv.Резервация.ID,
                      Count = x.Rezerv.Количество
                  }).ToList();
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = grid.SelectedItem as RezervUpd;

            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var needItem = db.Резервация.FirstOrDefault(x => x.ID == selectedItem.ID);
                new EditStatusInRezervWindow(needItem).Show();
                Close();
            }
        }

       
        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = grid.SelectedItem as RezervUpd;
            if (selectedItem != null)
            {   
                var needList = GetProductUpds().Where(x => x.Rezerv == selectedItem.ID).ToList();
                tovarsGrid.ItemsSource = needList;
            }
            
        }

        private void DeleteRezervButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = grid.SelectedItem as RezervUpd;

            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var item = db.Резервация.Where(x => x.ID == selectedItem.ID).FirstOrDefault();
                db.Резервация.Remove(item);
                db.SaveChanges();

                Update();

            }
        }
    }
}
