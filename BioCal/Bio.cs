using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioCal
{
    public static class Bio
    {
        public static List<Stats> GenerateList(DateTime birthdate, DateTime startdate, int duration)
        {
            List<Stats> list = new List<Stats>();
            for (int i = 0; i < duration; i++)
            {
                list.Add(new Stats()
                {
                    Date = startdate.ToShortDateString(),
                    Strength = Math.Round(Math.Sin((3.14 * 2 * (startdate - birthdate).Days) / 23), 4),
                    Agility = Math.Round(Math.Sin((3.14 * 2 * (startdate - birthdate).Days) / 28), 4),
                    Intelligence = Math.Round(Math.Sin((3.14 * 2 * (startdate - birthdate).Days) / 33), 4),
                    Sum = Math.Round(Math.Sin((3.14 * 2 * (startdate - birthdate).Days) / 23), 4)
                    + Math.Round(Math.Sin((3.14 * 2 * (startdate - birthdate).Days) / 28), 4)
                    + Math.Round(Math.Sin((3.14 * 2 * (startdate - birthdate).Days) / 33), 4)
                });
                startdate = startdate.AddDays(1);
            }
            return list;
        }
    }
    
}
