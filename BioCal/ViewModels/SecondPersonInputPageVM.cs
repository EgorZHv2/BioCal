using BioCal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioCal.ViewModels
{
    public class SecondPersonInputPageVM:BaseVM,IInputPageVM
    {
       public SecondPersonInputPageVM()
        {
            BirthDate = new DateTime(2002, 03, 28);
            StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Duration = 30;
        }
        private DateTime birthdate;

        public DateTime BirthDate
        {
            get { return birthdate; }
            set
            {
                birthdate = value;
                OnPropertyChanged();
            }
        }

        private DateTime startdate;

        public DateTime StartDate
        {
            get { return startdate; }
            set
            {
                startdate = value;
                OnPropertyChanged();
            }
        }

        private int duration;
        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged();
            }
        }
    }
}
