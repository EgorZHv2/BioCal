using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BioCal.Interfaces;
using BioCal.Pages;
using CsvHelper;
using CsvHelper.Configuration;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;

namespace BioCal.ViewModels
{
    public class MainVM:BaseVM
    {
        IInputPageVM InputDataContext;

        FirstPersonInputPage firstPersonInput = new FirstPersonInputPage();
        SecondPersonInputPage secondPersonInput = new SecondPersonInputPage();
       
        private bool showdata;
        public bool ShowData
        {
            get { return showdata; }
            set
            {
                showdata = value;
                OnPropertyChanged();
                if (ShowData)
                {
                    X = new List<string>();
                    foreach (Stats stat in List)
                    {
                        X.Add(stat.Date);
                    }
                    
                }
                else
                {
                   
                    X = new List<string>();
                    for (int i = 1; i <= List.Count; i++)
                    {
                        X.Add(i.ToString());
                    }
                   
                }
            }
        }

         private List<string> statistics = new List<string>();

        public List<string> Statistics
        {
            get { return statistics; }
            set { statistics = value;
                OnPropertyChanged();
            }
        }

        private bool pageselector;
        public bool PageSelector
        {
            get { return pageselector; }
            set { pageselector = value;
                OnPropertyChanged();
                if (PageSelector)
                {
                    InputPage = firstPersonInput;
                }
                else
                {
                    InputPage = secondPersonInput;
                }
            }
        }
        
        public MainVM()
        {
            PageSelector = true;
            
        }
        private SeriesCollection series = new SeriesCollection();
        public SeriesCollection Series
        {
            get { return series; }
            set {
                series = value;
                OnPropertyChanged();
            }
        }
        private List<string> x = new List<string>();
        public List<string> X
        {
            get { return x; }
            set { x = value;
                OnPropertyChanged();
            }
        }
        private Page inputpage;
        public Page InputPage
        {
            get { return inputpage; }
            set { inputpage = value;
                OnPropertyChanged();
                InputDataContext = InputPage.DataContext as IInputPageVM;
            }
        }
       


         private   List<Stats> list = new List<Stats>();

         public List<Stats> List
        {
            get { return list; }
            set { list = value;
                OnPropertyChanged();
            }
        }

        public ICommand GenerateGraph
        {
            get
            {
                return new DelegateCommand(obj =>
                 {

                     if (InputDataContext.BirthDate == null)
                     {
                         MessageBox.Show("Введите дату рождения");
                         return;
                     }
                     if (InputDataContext.StartDate == null)
                     {
                         MessageBox.Show("Введите дату отсчёта");
                         return;
                     }
                     if (InputDataContext.Duration == null)
                     {
                         MessageBox.Show("Введите длительность прогноза");
                         return;
                     }
                     List = new List<Stats>();

                     X = new List<string>();
                     Series = new SeriesCollection();
                     List = Bio.GenerateList(InputDataContext.BirthDate, InputDataContext.StartDate, InputDataContext.Duration);

                     ChartValues<double> StrengthValues = new ChartValues<double>();
                     ChartValues<double> AgilityValues = new ChartValues<double>();
                     ChartValues<double> IntelligenceValues = new ChartValues<double>();


                     List<string> statlist = new List<string>();

                     double maxstrenght = double.MinValue;
                     double maxagility = double.MinValue;
                     double maxintelligence = double.MinValue;
                     double maxaver = double.MinValue;
                     foreach (Stats stat in List)
                     {
                         StrengthValues.Add(stat.Strength);
                         AgilityValues.Add(stat.Agility);
                         IntelligenceValues.Add(stat.Intelligence);
                         if (stat.Strength > maxstrenght)
                         {
                             maxstrenght = stat.Strength;

                         }
                         if (stat.Agility > maxagility)
                         {
                             maxagility = stat.Agility;

                         }
                         if (stat.Intelligence > maxintelligence)
                         {
                             maxintelligence = stat.Intelligence;

                         }
                         if (stat.Average > maxaver)
                         {
                             maxaver = stat.Average;

                         }

                     }

                     statlist.Add($"Дата рождения - {InputDataContext.BirthDate.ToShortDateString()}");
                     statlist.Add($"Длительность прогноза - {InputDataContext.Duration}");
                     statlist.Add($"Период с {List[0].Date} по {List[List.Count - 1].Date}");
                     statlist.Add($"Физические максимумы ({maxstrenght}): {List.FirstOrDefault(obj => obj.Strength == maxstrenght).Date}");
                     statlist.Add($"Эмоционалньые максимумы ({maxagility}): {List.FirstOrDefault(obj => obj.Agility == maxagility).Date}");
                     statlist.Add($"Интеллектуальные максимумы ({maxintelligence}): {List.FirstOrDefault(obj => obj.Intelligence == maxintelligence).Date}");
                     statlist.Add($"Средние максимумы ({maxaver}): {List.FirstOrDefault(obj => obj.Average == maxaver).Date}");
                     Statistics = statlist;

                     Series.Add(new LineSeries
                     {
                         Title = "Физические ритмы",
                         Values = StrengthValues
                     });
                     Series.Add(new LineSeries
                     {
                         Title = "Эмоциональные ритмы",
                         Values = AgilityValues
                     });
                     Series.Add(new LineSeries
                     {
                         Title = "Интелектуальные ритмы",
                         Values = IntelligenceValues
                     });

                     if (ShowData)
                     {
                         X = new List<string>();
                         foreach (Stats stat in List)
                         {
                             X.Add(stat.Date);
                         }
                     }
                     else
                     {

                         X = new List<string>();
                         for (int i = 1; i <= List.Count; i++)
                         {
                             X.Add(i.ToString());
                         }
                     }

                 });
            }
        }

       
           
        
        public ICommand GenetateСompatibility
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    List = new List<Stats>();

                    X = new List<string>();
                    Series = new SeriesCollection();
                    var firstVM = firstPersonInput.DataContext as IInputPageVM;
                    var secondVM = secondPersonInput.DataContext as IInputPageVM;
                    if(firstVM.StartDate != secondVM.StartDate)
                    {
                        MessageBox.Show("Даты отсчёта не совпадают");
                        return;
                    }
                    if(firstVM.Duration != secondVM.Duration)
                    {
                        MessageBox.Show("Длительность прогноза не совпадает");
                        return;
                    }
                    List = Bio.GenerateCompabilityList(firstVM.BirthDate, secondVM.BirthDate, firstVM.StartDate,firstVM.Duration);

                    ChartValues<double> StrengthValues = new ChartValues<double>();
                    ChartValues<double> AgilityValues = new ChartValues<double>();
                    ChartValues<double> IntelligenceValues = new ChartValues<double>();


                    List<string> statlist = new List<string>();

                    double maxstrenght = double.MinValue;
                    double maxagility = double.MinValue;
                    double maxintelligence = double.MinValue;
                    double maxaver = double.MinValue;
                    foreach (Stats stat in List)
                    {
                        StrengthValues.Add(stat.Strength);
                        AgilityValues.Add(stat.Agility);
                        IntelligenceValues.Add(stat.Intelligence);
                        if (stat.Strength > maxstrenght)
                        {
                            maxstrenght = stat.Strength;

                        }
                        if (stat.Agility > maxagility)
                        {
                            maxagility = stat.Agility;

                        }
                        if (stat.Intelligence > maxintelligence)
                        {
                            maxintelligence = stat.Intelligence;

                        }
                        if (stat.Average > maxaver)
                        {
                            maxaver = stat.Average;

                        }

                    }

                    statlist.Add($"Дата рождения - {InputDataContext.BirthDate.ToShortDateString()}");
                    statlist.Add($"Длительность прогноза - {InputDataContext.Duration}");
                    statlist.Add($"Период с {List[0].Date} по {List[List.Count - 1].Date}");
                    statlist.Add($"Физические максимумы ({maxstrenght}): {List.FirstOrDefault(obj => obj.Strength == maxstrenght).Date}");
                    statlist.Add($"Эмоционалньые максимумы ({maxagility}): {List.FirstOrDefault(obj => obj.Agility == maxagility).Date}");
                    statlist.Add($"Интеллектуальные максимумы ({maxintelligence}): {List.FirstOrDefault(obj => obj.Intelligence == maxintelligence).Date}");
                    statlist.Add($"Средние максимумы ({maxaver}): {List.FirstOrDefault(obj => obj.Average == maxaver).Date}");
                    Statistics = statlist;

                    Series.Add(new LineSeries
                    {
                        Title = "Физические ритмы",
                        Values = StrengthValues
                    });
                    Series.Add(new LineSeries
                    {
                        Title = "Эмоциональные ритмы",
                        Values = AgilityValues
                    });
                    Series.Add(new LineSeries
                    {
                        Title = "Интелектуальные ритмы",
                        Values = IntelligenceValues
                    });

                    if (ShowData)
                    {
                        X = new List<string>();
                        foreach (Stats stat in List)
                        {
                            X.Add(stat.Date);
                        }
                    }
                    else
                    {

                        X = new List<string>();
                        for (int i = 1; i <= List.Count; i++)
                        {
                            X.Add(i.ToString());
                        }
                    }
                });
            }
        }
        public ICommand ExportToCsv
        {
            get
            {
                return new DelegateCommand(async obj =>
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = "Results.csv";
                    saveFileDialog.ShowDialog();
                    string path = saveFileDialog.FileName;
                    CsvConfiguration configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ";"
                    };
                    using (StreamWriter streamWriter = new StreamWriter(path, false, System.Text.Encoding.UTF8))
                    using (CsvWriter csvWriter = new CsvWriter(streamWriter, configuration))
                    {
                        await csvWriter.WriteRecordsAsync(List);
                    }
                });
            }
        }
    }
}
