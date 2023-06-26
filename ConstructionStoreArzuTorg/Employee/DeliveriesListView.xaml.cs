using ConstructionStoreArzuTorg.Add;
using ConstructionStoreArzuTorg.ClassConnection;
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
using Microsoft.Office.Interop.Word;
using ConstructionStoreArzuTorg.Manager;

namespace ConstructionStoreArzuTorg.Employee
{
    /// <summary>
    /// Логика взаимодействия для DeliveriesListView.xaml
    /// </summary>
    public partial class DeliveriesListView : System.Windows.Window
    {
        public DeliveriesListView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           UpdateView();
        }

        public void UpdateView()
        {

            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                //подключение поставки
                var first = db.Поставки.ToList()
                    .GroupJoin(
                    db.Сотрудник.ToList(),
                    cl => cl.Сотрудник,
                    ci => ci.ID_Сотрудника,
                    (cl, ci) => new { cl, ci }).SelectMany(
                    x => x.ci.DefaultIfEmpty(),
                    (one, two) => new DeliveriesUpd()
                    {
                        Дата = one.cl.Дата,
                        ФамилияСотрудника = two.Имя + " " + two.Фамилия,
                        ID = one.cl.ID,
                        Сумма = Math.Round(one.cl.Сумма,2)

                    }).ToList();

                //подключение поставщика
                var second = db.Поставки.ToList()
                    .GroupJoin(
                    db.Поставщик.ToList(),
                    cl => cl.Поставщик,
                    ci => ci.ID_Поставщика,
                    (cl, ci) => new { cl, ci }).SelectMany(
                    x => x.ci.DefaultIfEmpty(),
                    (one, two) => new DeliveriesUpd()
                    {
                        НаименованиеПоставщика = two.Наименование
                    }).ToList();

                for (int i = 0; i < first.Count; i++) { first[i].НаименованиеПоставщика = second[i].НаименованиеПоставщика; }
                postavkiGrid.ItemsSource = first;
                tovarsGrid.ItemsSource = null;
            }
        }
        //получение списка продуктов
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
                //подключение единиц измерения товара
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

       
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new AddDeliveriesView().Show();
            Close();
        }
        //удаление поставки
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = postavkiGrid.SelectedItem as DeliveriesUpd;

            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var item = db.Поставки.Where(x => x.ID == selectedItem.ID).FirstOrDefault();
                db.Поставки.Remove(item);
                db.SaveChanges();

                UpdateView();

            }
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            //создане товарной накладной
            var selectedItem = postavkiGrid.SelectedItem as DeliveriesUpd;
            try
            {
                using (ConstructionStoreEntities db = new ConstructionStoreEntities())
                {


                    var joinedDataProduct = GetProductUpds().Where(x => x.Post == selectedItem.ID).ToList();
                    var tovars = db.Товар.ToList();
                    var result = joinedDataProduct.GroupBy(t => t).GroupBy(t => t.Count()).ToArray();
                    var uniqueElements = result[0].Count();


                    Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
                    Microsoft.Office.Interop.Word._Document wordDocument = null;
                    wordApplication.Visible = true;
                    //указываем путь к документу

                    //var templatePathObj = @"D:\Проекты\ConstructionStoreArzuTorgNew-master\ConstructionStoreArzuTorg\DeliverReport.docx";

                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    var relativePath = "DeliverReport.docx";
                    var templatePathObj = System.IO.Path.Combine(baseDirectory, relativePath);

                    try
                    {
                        wordDocument = wordApplication.Documents.Add(templatePathObj);
                    }
                    catch (Exception exception)
                    {
                        if (wordDocument != null)
                        {
                            wordDocument.Close(false);
                            wordDocument = null;
                        }
                        wordApplication.Quit();
                        wordApplication = null;
                        throw;
                    }
                    //получаем поставщика, поставки и сотрудника
                    var needObject = db.Поставки.Where(x => x.ID == selectedItem.ID).FirstOrDefault();
                    var provider = db.Поставщик.Where(x => x.ID_Поставщика == needObject.Поставщик).FirstOrDefault();
                    var worker = db.Сотрудник.Where(x => x.ID_Сотрудника == needObject.Сотрудник).FirstOrDefault();



                    var needCount = uniqueElements + 1;

                    wordApplication.Selection.Find.Execute("{Table}");
                    Microsoft.Office.Interop.Word.Range wordRange = wordApplication.Selection.Range;


                    //делаем нужное количество столбцов
                    var wordTable = wordDocument.Tables.Add(wordRange,
                        needCount, 7);


                    wordTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                    wordTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;
                    wordTable.Range.Font.Name = "Times New Roman";
                    wordTable.Range.Font.Size = 12;

                    //задаём название заголовка столбцов
                    wordTable.Cell(1, 1).Range.Text = "Наименование товара";
                    wordTable.Cell(1, 2).Range.Text = "Категория";
                    wordTable.Cell(1, 3).Range.Text = "Единица измерения";
                    wordTable.Cell(1, 4).Range.Text = "Количество";
                    wordTable.Cell(1, 5).Range.Text = "Стоимость";
                    wordTable.Cell(1, 6).Range.Text = "Сумма НДС";
                    wordTable.Cell(1, 7).Range.Text = "Стоимость с НДС";


                    for (int i = 0; i < joinedDataProduct.Count; i++)
                    {
                        wordTable.Cell(i + 2, 1).Range.Text = joinedDataProduct[i].Название;
                        wordTable.Cell(i + 2, 2).Range.Text = joinedDataProduct[i].НазваниеКатегории;
                        wordTable.Cell(i + 2, 3).Range.Text = joinedDataProduct[i].ЕдиницаИзмерения;
                        wordTable.Cell(i + 2, 4).Range.Text = joinedDataProduct[i].Count.ToString();
                        wordTable.Cell(i + 2, 5).Range.Text = Math.Round(joinedDataProduct[i].Стоимость, 2).ToString() + " BYN";
                        wordTable.Cell(i + 2, 6).Range.Text = Math.Round(joinedDataProduct[i].SumNDS, 2).ToString() + " BYN";
                        wordTable.Cell(i + 2, 7).Range.Text = Math.Round(joinedDataProduct[i].SumWithNDS, 2).ToString() + " BYN";
                    }

                    //считаем НДС и сумму НДС
                    decimal nds = needObject.Сумма * 20 / 120;
                    decimal sumNoNDS = needObject.Сумма - nds;


                    Random random = new Random();
                    //передаём значения в документ
                    var items = new Dictionary<string, string>
                {
                    { "{Date}", needObject.Дата.ToString("dd.MM.yyyy")  },
                    { "{Provider}", provider.Наименование },
                    { "{Worker}", worker.Фамилия + " " + worker.Имя + " " + worker.Отчество },
                    { "{Number}", provider.Расчётный_счёт.ToString() },
                    { "{Sum}", Math.Round(needObject.Сумма,2).ToString() + " BYN" },
                    { "{SumNDS}", Math.Round(nds,2).ToString() + " BYN" },
                    { "{SumNoNDS}", Math.Round(sumNoNDS, 2).ToString() + " BYN" },
                    { "{ID}",  random.Next(1000000, 9999999) + needObject.ID.ToString() }
                };


                    foreach (var item in items)
                    {
                        Microsoft.Office.Interop.Word.Find find = wordApplication.Selection.Find;
                        find.Text = item.Key;
                        find.Replacement.Text = item.Value;

                        object wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
                        object replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;

                        find.Execute(
                            FindText: Type.Missing,
                            MatchCase: false,
                            MatchWholeWord: false,
                            MatchWildcards: false,
                            MatchSoundsLike: Type.Missing,
                            MatchAllWordForms: false,
                            Forward: true,
                            Wrap: wrap,
                            Format: false,
                            ReplaceWith: Type.Missing, Replace: replace);
                    }
                }
            }
            catch 
            {
                MessageBox.Show("Ошибка создания документа");
            }
            
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new ManagerMenuView().Show();
            Close();
        }

        private void postavkiGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = postavkiGrid.SelectedItem as DeliveriesUpd;
            if (selectedItem != null)
            {
                var needList = GetProductUpds().Where(x => x.Post == selectedItem.ID).ToList();
                tovarsGrid.ItemsSource = needList;

            }
        }
        //получение списка продуктов в поставке
        public List<ProductUpd> GetProductUpds()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                //присоединение параметров к товару
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
                   .Join(db.ПоставленныеТовары,
                      joinResult => joinResult.Tovar.ID_Товара,
                      post => post.Товар,
                      (joinResult, post) => new { Tovar = joinResult.Tovar, Param = joinResult.Param, Post = post, PT = joinResult.PT})
                  .Select(x => new ProductUpd
                  {
                      Название = x.Tovar.Название,
                      НазваниеКатегории = x.Tovar.Категория.Название,
                      Размеры = x.Tovar.РазмерыТовара.Размер,
                      ЕдиницаИзмерения = x.Tovar.Единицы_измерения.Название,
                      Сезонность = x.Param.Название_сезона,
                      Стоимость = x.Tovar.Стоимость,
                      Post = x.Post.Поставка,
                      Count = x.Post.Количество,
                      SumToReceipt = x.Tovar.Стоимость * x.Post.Количество,
                      SumNDS = ((x.Tovar.Стоимость * x.Post.Количество) * 20) / 100,
                      SumWithNDS = (((x.Tovar.Стоимость * x.Post.Количество) * 20) / 100) + (x.Tovar.Стоимость * x.Post.Количество)
                  }).ToList();
            }
        }
    }
}
