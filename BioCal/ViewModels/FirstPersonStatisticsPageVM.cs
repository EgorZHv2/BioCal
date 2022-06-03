using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioCal.Interfaces;

namespace BioCal.ViewModels
{
    public class FirstPersonStatisticsPageVM:BaseVM,IStatisticsPageVM
    {
       private List<string> statistics = new List<string>();

        public List<string> Statistics
        {
            get { return statistics; }
            set { statistics = value;
                OnPropertyChanged();
            }
        }

    }
}
