using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BioCal.Interfaces;
using BioCal.Pages;
using LiveCharts;
using LiveCharts.Wpf;

namespace BioCal.ViewModels
{
    public class MainVM:BaseVM
    {
        IInputPageVM InputDataContext;
        IStatisticsPageVM StatisticsDataContext;
        
       
        private bool showdata;
        //public bool ShowData
        //{
        //    get { return showdata; }
        //    set { showdata = value;
        //        OnPropertyChanged();
        //        if (ShowData)
        //        {
        //            X = new Axis()
        //            {
                        
        //            }
        //        }
        //    }
        //}
        public MainVM()
        {
            InputPage = new FirstPersonInputPage();
            StatisticsPage = new FirstPersonStatisticsPage();
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
        private Page statisticspage;
        public Page StatisticsPage
        {
            get { return statisticspage; }
            set { statisticspage = value;
                OnPropertyChanged();
                StatisticsDataContext = StatisticsPage.DataContext as IStatisticsPageVM;
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
                     if(InputDataContext.Duration == null)
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
                     double maxsum = double.MinValue;
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
                         if (stat.Sum > maxsum)
                         {
                             maxsum = stat.Sum;
                             
                         }
                         X.Add(stat.Date);
                     }
                    
                     statlist.Add($"Дата рождения - {InputDataContext.BirthDate.ToShortDateString()}");
                     statlist.Add($"Длительность прогноза - {InputDataContext.Duration}");
                     statlist.Add($"Период с {List[0].Date} по {List[List.Count - 1].Date}");
                     statlist.Add($"Физические максимумы ({maxstrenght}): {List.FirstOrDefault(obj => obj.Strength == maxstrenght).Date}");
                     statlist.Add($"Эмоционалньые максимумы ({maxagility}): {List.FirstOrDefault(obj => obj.Agility == maxagility).Date}");
                     statlist.Add($"Интеллектуальные максимумы ({maxintelligence}): {List.FirstOrDefault(obj => obj.Intelligence == maxintelligence).Date}");
                     statlist.Add($"Средние максимумы ({maxsum}): {List.FirstOrDefault(obj => obj.Sum == maxsum).Date}");
                     StatisticsDataContext.Statistics = statlist;

                    
                     
                     
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
                  
                     
                     MessageBox.Show($"Количество дат: {List.Count} Количество лейблов : {X.Count}");


                 });
            }
        }
    }
}
