using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BioCal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Stats> stats = new List<Stats>();
        private SeriesCollection series = new SeriesCollection();
       
        public MainWindow()
        {
            InitializeComponent();
            Dates.ItemsSource = stats;
        }

        DateTime birthdate;
        DateTime startdate;
        private void GenerateGraph_Click(object sender, RoutedEventArgs e)
        {
            if(Duration.Text == String.Empty || Duration.Text == null)
            {
                MessageBox.Show("Введите длительность прогноза");
                e.Handled = true;
            }
            if(BirthDate.Text == String.Empty || BirthDate.Text == null)
            {
                MessageBox.Show("Введите дату рождения");
                e.Handled = true;
            }
            if(StartDate.Text == String.Empty || StartDate.Text == null)
            {
                MessageBox.Show("Введите дату отсчёта");
                e.Handled = true;
            }
            birthdate = Convert.ToDateTime(BirthDate.Text);
            startdate = Convert.ToDateTime(StartDate.Text);
            stats.Clear();
            for (int i = 0; i < Convert.ToInt32(Duration.Text); i++)
            {
                
                stats.Add(new Stats()
                {
                    Date = startdate.ToShortDateString(),
                    Strength = Math.Round(Math.Sin((3.14*2*(startdate-birthdate).Days)/23),4),
                    Agility = Math.Round(Math.Sin((3.14 * 2 * (startdate - birthdate).Days) / 28),4),
                    Intelligence = Math.Round(Math.Sin((3.14 * 2 * (startdate - birthdate).Days) / 33),4),
                    Sum = Math.Round(Math.Sin((3.14 * 2 * (startdate - birthdate).Days) / 23), 4) 
                    + Math.Round(Math.Sin((3.14 * 2 * (startdate - birthdate).Days) / 28), 4) 
                    + Math.Round(Math.Sin((3.14 * 2 * (startdate - birthdate).Days) / 33), 4)

                });
                startdate = startdate.AddDays(1);
            }
            Dates.Items.Refresh();
            double maxstrenght = double.MinValue;           
            double maxagility = double.MinValue;
            double maxintelligence = double.MinValue;
            double maxsum = double.MinValue;
            StatBirthDate.Text = "Дата рождения - " + birthdate.ToShortDateString();
            StatDuration.Text = "Длительность прогноза - " + Duration.Text;
            StatPeriod.Text = "Период с " + stats[0].Date + " по " + stats[stats.Count - 1].Date;
            
            foreach(Stats stat in stats)
            {
                if(stat.Strength > maxstrenght)
                {
                    maxstrenght = stat.Strength;
                    StatMaxStrenght.Text = $"Физические максимумы ({maxstrenght}): " + stat.Date;
                    
                }
                if(stat.Agility > maxagility)
                {
                    maxagility = stat.Agility;
                    StatMaxAgility.Text = $"Эмоциональные максимумы ({maxagility}): " + stat.Date;
                }
                if(stat.Intelligence > maxintelligence)
                {
                    maxintelligence = stat.Intelligence;
                    StatMaxIntelligence.Text = $"Интеллектуальные максимумы ({maxintelligence}): " + stat.Date;
                }
                if (stat.Sum > maxsum)
                {
                    maxsum = stat.Sum;
                    StatMaxSum.Text = $"Суммарные максимумы ({maxsum}): " + stat.Date;
                }                   
            }
            ChartValues<double> StrengthValues = new ChartValues<double>();
            ChartValues<double> AgilityValues = new ChartValues<double>();
            ChartValues<double> IntelligenceValues = new ChartValues<double>();
            foreach(Stats stat in stats)
            {
                StrengthValues.Add(stat.Strength);
                AgilityValues.Add(stat.Agility);
                IntelligenceValues.Add(stat.Intelligence);
            }

            series.Add(new LineSeries
            {
                Title = "Физические ритмы",
                Values = StrengthValues
            });
            series.Add(new LineSeries
            {
                Title = "Эмоциональные ритмы",
                Values = AgilityValues
            });
            series.Add(new LineSeries
            {
                Title = "Интеллектуальные ритмы",
                Values = IntelligenceValues
            });
            
            chart.Series = series;
            chart.Update();

        }

        private void ExportToWord_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
