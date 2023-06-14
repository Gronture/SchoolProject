using ConstructionStoreArzuTorg.ClassConnection;
using ConstructionStoreArzuTorg.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace ConstructionStoreArzuTorg.Other
{
    /// <summary>
    /// Логика взаимодействия для ReturnView.xaml
    /// </summary>
    public partial class ReturnView : Window
    {
        ЗаказанныеТовары product;
        public ReturnView()
        {
            InitializeComponent();
        }

        public ReturnView(ЗаказанныеТовары заказанныеТовары)
        {
            product = заказанныеТовары;
            InitializeComponent();
            LoadData();
        }


        public void LoadData()
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                var list = db.Товар
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
                     .Join(db.ЗаказанныеТовары,
                      joinResult => joinResult.Tovar.ID_Товара,
                      ord => ord.Товар,
                      (joinResult, ord) => new { Tovar = joinResult.Tovar, Param = joinResult.Param, Ord = ord, PT = joinResult.PT })
                  .Select(x => new ProductUpd
                  {
                      ID_Товара = x.Tovar.ID_Товара,
                      Название = x.Tovar.Название,
                      НазваниеКатегории = x.Tovar.Категория.Название,
                      Размеры = x.Tovar.РазмерыТовара.Размер,
                      ЕдиницаИзмерения = x.Tovar.Единицы_измерения.Название,
                      Сезонность = x.Param.Название_сезона,
                      Ord = x.Ord.Заказ,
                      Count = x.Ord.Количество,
                    
                  }).ToList();

             


                tovarTextBox.Text = list.FirstOrDefault(x => x.ID_Товара == product.Товар && x.Ord == product.Заказ).Название;
              

            }
        }

        private void BackBotton_Click(object sender, RoutedEventArgs e)
        {
            new OrderListView().Show();
            Close();
        }

        private void EditStatusButton_Click(object sender, RoutedEventArgs e)
        {
            using (ConstructionStoreEntities db = new ConstructionStoreEntities())
            {
                if (countTextBox.Text != string.Empty)
                {
                    try
                    {
                        var inTextBoxCount = int.Parse(countTextBox.Text);
                        if (inTextBoxCount <= 0)
                        {
                            countTextBox.Clear();
                        }



                        var itemToEdit = db.ЗаказанныеТовары.FirstOrDefault(x => x.ID == product.ID);
                        var productPrice = db.Товар.FirstOrDefault(x => x.ID_Товара == itemToEdit.Товар).Стоимость;


                        var supply = db.Заказ.FirstOrDefault(x => x.ID_Заказа == itemToEdit.Заказ);

                        if (itemToEdit != null)
                        {
                            itemToEdit.Количество -= inTextBoxCount;
                            itemToEdit.Сумма = itemToEdit.Количество * productPrice;

                            var item = new ВозвратТовара();

                            item.Количество = inTextBoxCount;
                            item.Заказ = itemToEdit.Заказ;
                            item.Заказанные_товары = itemToEdit.ID;
                            db.ВозвратТовара.Add(item);

                            var itemOnWarehouse = db.Склад.FirstOrDefault(x => x.Товар == itemToEdit.Товар);
                            itemOnWarehouse.Количество += inTextBoxCount;


                            supply.Сумма = (decimal)db.ЗаказанныеТовары.Where(x => x.Заказ == supply.ID_Заказа).Sum(x => x.Сумма);

                            if (itemToEdit.Количество <= 0)
                            {
                                var sameProduct = db.ЗаказанныеТовары
                                    .FirstOrDefault(x => x.Заказ == itemToEdit.Заказ && x.Товар == itemToEdit.Товар && x.ID != itemToEdit.ID && x.Количество > 0);
                                if (sameProduct != null)
                                {
                                    sameProduct.Количество -= Math.Abs(itemToEdit.Количество);
                                    db.ЗаказанныеТовары.Remove(itemToEdit);

                                }
                                else
                                {
                                    db.ЗаказанныеТовары.Remove(itemToEdit);
                                }
                                db.SaveChanges();
                            }
                            else
                            {
                                db.SaveChanges();
                            }
                        }

                        //else
                        //    itemToEdit.Статус = db.Статус.FirstOrDefault(x => x.Название == "Возврат").ID;

                        db.SaveChanges();

                        var listWithData = GetProducts(supply.ID_Заказа);

                        var tovars = db.Товар.ToList();
                        var result = listWithData.GroupBy(t => t).GroupBy(t => t.Count()).ToArray();
                        var uniqueElements = result[0].Count();

                        Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
                        Microsoft.Office.Interop.Word._Document wordDocument = null;
                        wordApplication.Visible = true;
                        //указываем путь к документу
                        var templatePathObj = @"D:\Проекты\ConstructionStoreArzuTorg-master\ConstructionStoreArzuTorg\ReturnDocument.docx";

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
                        var needObject = db.Заказ.Where(x => x.ID_Заказа == product.Заказ).FirstOrDefault();

                        var provider = db.Клиент.Where(x => x.ID_Клиента == needObject.ID_Клиента).FirstOrDefault();
                        var worker = db.Сотрудник.Where(x => x.ID_Сотрудника == needObject.ID_Сотрудника).FirstOrDefault();



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


                        for (int i = 0; i < listWithData.Count; i++)
                        {
                            wordTable.Cell(i + 2, 1).Range.Text = listWithData[i].Название;
                            wordTable.Cell(i + 2, 2).Range.Text = listWithData[i].НазваниеКатегории;
                            wordTable.Cell(i + 2, 3).Range.Text = listWithData[i].ЕдиницаИзмерения;
                            wordTable.Cell(i + 2, 4).Range.Text = listWithData[i].Count.ToString();
                            wordTable.Cell(i + 2, 5).Range.Text = Math.Round(listWithData[i].Стоимость, 2).ToString() + " BYN";
                            wordTable.Cell(i + 2, 6).Range.Text = Math.Round(listWithData[i].SumNDS, 2).ToString() + " BYB";
                            wordTable.Cell(i + 2, 7).Range.Text = Math.Round(listWithData[i].SumWithNDS, 2).ToString() + " BYN";
                        }

                        //    //считаем НДС и сумму НДС
                        decimal nds = needObject.Сумма * 20 / 120;
                        decimal sumNoNDS = needObject.Сумма - nds;


                        Random random = new Random();
                        //передаём значения в документ
                        var items = new Dictionary<string, string>
                            {
                                { "{Date}", needObject.Дата_заказа.ToString("dd.MM.yyyy")  },
                                { "{Provider}", provider.Фамилия + provider.Имя[0] + " " + provider.Отчество[0] },
                                { "{Worker}", worker.Фамилия + " " + worker.Имя + " " + worker.Отчество },
                                { "{Number}", random.Next(192992, 1258812).ToString() },
                                { "{Sum}", Math.Round(needObject.Сумма,2).ToString() + " BYN" },
                                { "{SumNDS}", Math.Round(nds,2).ToString() + " BYN" },
                                { "{SumNoNDS}", Math.Round(sumNoNDS, 2).ToString() + " BYN" },
                                { "{ID}",  random.Next(1000000, 9999999) + needObject.ID_Заказа.ToString() }
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
                    

                    


                        new OrderListView().Show();
                        Close();
                    }
                    catch
                    {
                        MessageBox.Show("Возникла ошибка");
                    }
                }

            }

          
        }

        public List<ProductUpd> GetProducts(int id)
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
                     .Join(db.ЗаказанныеТовары,
                      joinResult => joinResult.Tovar.ID_Товара,
                      ord => ord.Товар,
                      (joinResult, ord) => new { Tovar = joinResult.Tovar, Param = joinResult.Param, Ord = ord, PT = joinResult.PT })
                  .Select(x => new ProductUpd
                  {
                      ID_Товара = x.Tovar.ID_Товара,
                      Название = x.Tovar.Название,
                      НазваниеКатегории = x.Tovar.Категория.Название,
                      Размеры = x.Tovar.РазмерыТовара.Размер,
                      ЕдиницаИзмерения = x.Tovar.Единицы_измерения.Название,
                      Стоимость = x.Tovar.Стоимость,
                      Сезонность = x.Param.Название_сезона,
                      Ord = x.Ord.Заказ,
                      Count = x.Ord.Количество,
                      SumToReceipt = x.Tovar.Стоимость * x.Ord.Количество,
                      SumNDS = ((x.Tovar.Стоимость * x.Ord.Количество) * 20) / 100,
                      SumWithNDS = (((x.Tovar.Стоимость * x.Ord.Количество) * 20) / 100) + (x.Tovar.Стоимость * x.Ord.Количество)
                  }).Where(x => x.Ord == id).ToList();




              


            }
        }
    }
}
