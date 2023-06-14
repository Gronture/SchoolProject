using ConstructionStoreArzuTorg.ClassConnection;
using ConstructionStoreArzuTorg.Employee;
using Syncfusion.Linq;
using Syncfusion.Windows.Gauge;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для AddProductToDeliveries.xaml
    /// </summary>
    public partial class AddProductToDeliveries : Window
    {
        Поставки _поставки;
        public AddProductToDeliveries(Поставки поставки)
        {
            InitializeComponent();
            _поставки = поставки;
            LoadData();
        }


        public void ClearComboBox()
        {
            ProductComboBox.Text = string.Empty;
            RazmerComboBox.Text = string.Empty;
            CategorComboBox.Text = string.Empty;
            EdIzmComboBox.Text = string.Empty;
            ColvoTextBox.Text = string.Empty;
        }

        public void UpdateView()
        {
            using(ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                tovarsGrid.ItemsSource = db.Товар
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
                     .Join(db.ПоставленныеТовары,
                      joinResult => joinResult.Tovar.ID_Товара,
                      post => post.Товар,
                      (joinResult, post) => new { Tovar = joinResult.Tovar, Param = joinResult.Param, Post = post, PT = joinResult.PT })
                  .Select(x => new ProductUpd
                  {
                      Название = x.Tovar.Название,
                      НазваниеКатегории = x.Tovar.Категория.Название,
                      Размеры = x.Tovar.РазмерыТовара.Размер,
                      ЕдиницаИзмерения = x.Tovar.Единицы_измерения.Название,
                      Сезонность = x.Tovar.Сезонность1.Название_сезона,
                      Post = x.Post.Поставка,
                      Count = x.Post.Количество
                  }).Where(x => x.Post == _поставки.ID).ToList();
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using(ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var tovars = db.Товар.ToList();
                var names = new List<string>();

                foreach (var item in tovars) { names.Add(item.Название); }
                ProductComboBox.ItemsSource = names.Distinct();
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
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
            try
            {
                using (ConstructionStoreEntities db = new ConstructionStoreEntities())
                {
                    var products = db.Товар.ToList();
                    var nameProd = ProductComboBox.SelectedItem.ToString();
                   
                    var categoria = db.Категория.Where(x => x.Название == CategorComboBox.SelectedItem.ToString()).FirstOrDefault();
                    var size = db.РазмерыТовара.Where(x => x.Размер == RazmerComboBox.SelectedItem.ToString()).FirstOrDefault();
                    var unitOfWork = db.Единицы_измерения.Where(x => x.Название == EdIzmComboBox.SelectedItem.ToString()).FirstOrDefault();


                
                    var needitems = db.Товар.Where(x => x.Название == nameProd).ToList();

                    var needProduct = needitems.Where(x => x.ID_Категории == categoria.ID_Категории &&
                    x.ID_Размеров == size.ID_Размеров &&
                    x.ID_Единицы_измерения == unitOfWork.ID_Измерений).FirstOrDefault();

                    if (needProduct != null)
                    {
                        ПоставленныеТовары prod = new ПоставленныеТовары();
                        prod.Поставка = _поставки.ID;
                        prod.Количество = int.Parse(ColvoTextBox.Text);
                        prod.Товар = needProduct.ID_Товара;

                        db.ПоставленныеТовары.Add(prod);
                        db.SaveChanges();

                        UpdateView();
                        
                        ClearComboBox();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка");
                    }
                }
            }
            catch
            {

            }
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
           using(ConstructionStoreEntities db = new ConstructionStoreEntities())
           {
                var selectedItem = tovarsGrid.SelectedItem as ProductUpd;
                var productName = selectedItem.Название;

                var categoria = db.Категория.Where(x => x.Название == selectedItem.НазваниеКатегории).FirstOrDefault();
                var size = db.РазмерыТовара.Where(x => x.Размер == selectedItem.Размеры).FirstOrDefault();  
                var unitOfWork = db.Единицы_измерения.Where(x => x.Название == selectedItem.ЕдиницаИзмерения).FirstOrDefault();

                var needProduct = db.Товар.Where(x =>
                x.ID_Категории == categoria.ID_Категории &&
                x.ID_Размеров == size.ID_Размеров &&
                x.ID_Единицы_измерения == unitOfWork.ID_Измерений &&
                x.Название == productName).FirstOrDefault();

                var itemToRemove = db.ПоставленныеТовары.Where(x =>
                x.Поставка == _поставки.ID &&
                x.Количество == selectedItem.Count &&
                x.Товар == needProduct.ID_Товара).FirstOrDefault();

                db.ПоставленныеТовары.Remove(itemToRemove);
                db.SaveChanges();

                UpdateView();            
           }
        }

        private void AddDeliverButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = tovarsGrid.SelectedItem as DeliveriesUpd;
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {


                var joinedDataProduct = GetProductUpds().Where(x => x.Post == _поставки.ID).ToList();
                var tovars = db.Товар.ToList();
                var result = joinedDataProduct.GroupBy(t => t).GroupBy(t => t.Count()).ToArray();
                var uniqueElements = result[0].Count();


                Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word._Document wordDocument = null;
                wordApplication.Visible = true;

                var templatePathObj = @"D:\Проекты\ConstructionStoreArzuTorgNew-master\ConstructionStoreArzuTorg\AddDocumentToPost.doc";

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




                var needObject = db.Поставки.Where(x => x.ID == _поставки.ID).FirstOrDefault();
                var поставщик = db.Поставщик.Where(x => x.ID_Поставщика == needObject.Поставщик).FirstOrDefault();
                var worker = db.Сотрудник.Where(x => x.ID_Сотрудника == needObject.Сотрудник).FirstOrDefault();



                var needCount = uniqueElements + 1;

                wordApplication.Selection.Find.Execute("{Table}");
                Microsoft.Office.Interop.Word.Range wordRange = wordApplication.Selection.Range;



                var wordTable = wordDocument.Tables.Add(wordRange,
                    needCount, 7);


                wordTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                wordTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;
                wordTable.Range.Font.Name = "Times New Roman";
                wordTable.Range.Font.Size = 12;


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
                    wordTable.Cell(i + 2, 5).Range.Text = joinedDataProduct[i].Стоимость.ToString() + " BYN";
                    wordTable.Cell(i + 2, 6).Range.Text = joinedDataProduct[i].SumNDS.ToString() + " BYN";
                    wordTable.Cell(i + 2, 7).Range.Text = joinedDataProduct[i].SumWithNDS.ToString() + " BYN";
                }


                decimal nds = needObject.Сумма * 20 / 120;
                decimal sumNoNDS = needObject.Сумма - nds;
                //decimal cena = sumNoNDS / joinedDataProduct.Count;


                Random random = new Random();

                var items = new Dictionary<string, string>
                {
                    { "{Date}", needObject.Дата.ToString("dd.MM.yyyy")  },
                    { "{NameProvider}",  поставщик.Наименование },
                    { "{PositionProvider}",  поставщик.Должность },
                    { "{ID}",  поставщик.ID_Поставщика.ToString() },
                    { "{Address}",  поставщик.Адрес },
                    { "{FIO}",  поставщик.ФИО },
                    { "{RS}",  поставщик.Расчётный_счёт },
                    { "{Bank}",  поставщик.Название_банка },
                    { "{UNP}",  поставщик.Учётный_номер_плательщика },
                    { "{Number}",  random.Next(1000000, 9999999) + needObject.ID.ToString() }
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



            new DeliveriesListView().Show();
            Close();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            using(ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var needItem = db.Поставки.Where(x => x.ID == _поставки.ID).FirstOrDefault();
                db.Поставки.Remove(needItem);
                db.SaveChanges();

                new DeliveriesListView().Show();
                Close();
            }
        }


        public void LoadData()
        {
            using(ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var namesProducts = new List<string>();
                var categoriesNames = new List<string>();
                var unitOfWorkNames = new List<string>();
                var sizesNames = new List<string>(); 

                var tovars = db.Товар.ToList();
               

                foreach (var i in tovars)
                    namesProducts.Add(i.Название);          

                ProductComboBox.ItemsSource = namesProducts.Distinct();


            }
        }

        private void ProductComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (ProductComboBox.SelectedItem == null)
                return;
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var selectedItem = ProductComboBox.SelectedItem.ToString();

                var needCategories = db.Категория.ToList();
                var needUnitOfWork = db.Единицы_измерения.ToList();
                var needSizes = db.РазмерыТовара.ToList();
            

                var categories = new List<string>();
                var unitOfWork = new List<string>();
                var size = new List<string>();

                var needProducts = db.Товар.Where(x => x.Название == selectedItem).ToList().Distinct().ToList();

                foreach(var i in needProducts)
                {
                    var needCategor = needCategories.Where(x => x.ID_Категории == i.ID_Категории).FirstOrDefault();
                    categories.Add(needCategor.Название);

                    var needUnit = needUnitOfWork.Where(x => x.ID_Измерений == i.ID_Единицы_измерения).FirstOrDefault();
                    unitOfWork.Add(needUnit.Название);

                    var needSize = needSizes.Where(x => x.ID_Размеров == i.ID_Размеров).FirstOrDefault();
                    size.Add(needSize.Размер);
                }
                

                CategorComboBox.ItemsSource = categories.Distinct();
                EdIzmComboBox.ItemsSource = unitOfWork.Distinct(); 
                RazmerComboBox.ItemsSource = size.Distinct();
                

            }
        }

        private void CategorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategorComboBox.SelectedItem == null)
                return;
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var selectedItem = CategorComboBox.SelectedItem.ToString();
                var categor = db.Категория.Where(x => x.Название == selectedItem).FirstOrDefault();

                var needProducts = db.Товар.Where(x => x.ID_Категории == categor.ID_Категории).ToList();

                var sizes = db.РазмерыТовара.ToList();

                var newSizes = new List<string>();

                for (int i = 0; i < needProducts.Count; i++)
                {
                   // var unitOfWork = db.Единицы_измерения.Where(x => x.ID_Измерений == needProducts[i].ID_Категории).FirstOrDefault();
                    var size = sizes.Where(x => x.ID_Размеров == needProducts[i].ID_Размеров).FirstOrDefault();
                    newSizes.Add(size.Размер);

                }

                RazmerComboBox.ItemsSource = newSizes.Distinct();

            }
        }

        private void RazmerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RazmerComboBox.SelectedItem == null)
                return;
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var selectedSize = RazmerComboBox.SelectedItem.ToString();
                var selectedCategor = CategorComboBox.SelectedItem.ToString();

                var categor = db.Категория.Where(x => x.Название == selectedCategor).FirstOrDefault();
                var size = db.РазмерыТовара.Where(x => x.Размер == selectedSize).FirstOrDefault();

                var needProducts = db.Товар.Where(x => x.ID_Размеров == size.ID_Размеров && x.ID_Категории == categor.ID_Категории).ToList();

                var unitofworks = db.Единицы_измерения.ToList();

                var newUnitOfWorks = new List<string>();
                for (int i = 0; i < needProducts.Count; i++)
                {
                    var unitOfWork = unitofworks.Where(x => x.ID_Измерений == needProducts[i].ID_Единицы_измерения).FirstOrDefault();
                    newUnitOfWorks.Add(unitOfWork.Название);
                    

                }
                EdIzmComboBox.ItemsSource = newUnitOfWorks.Distinct();


            }
        }

        private void EdIzmComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
                   .Join(db.ПоставленныеТовары,
                      joinResult => joinResult.Tovar.ID_Товара,
                      post => post.Товар,
                      (joinResult, post) => new { Tovar = joinResult.Tovar, Param = joinResult.Param, Post = post, PT = joinResult.PT })
                  .Select(x => new ProductUpd
                  {
                      ID_Товара = x.Tovar.ID_Товара,
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
