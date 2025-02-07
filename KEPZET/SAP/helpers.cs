
//zarovnanie kodu ctrl+k ctrl+f
using System;

namespace KEPZET.sap
{

    public class PN_DATE_TIME
    {

        public string PN;
        public string GrDate;
        public string GrTime;
        public DateTime dateTime;

        /*
        public DateTime GetDateTime
        {
            get
            {
                DateTime date, time;
                if (!DateTime.TryParse(this.GrDate, out date)) return new DateTime(0);
                if (!DateTime.TryParse(this.GrTime, out time)) return new DateTime(0);
                return new DateTime(date.Year,date.Month,date.Day,time.Hour,time.Minute,time.Second);
            }
        }*/
        
        public int posX;
        public int posY;

        public DateTime GetGrDateTime
        {
            get
            {
                DateTime date, time;
                if (!DateTime.TryParse(this.GrDate, out date)) return new DateTime(0);
                if (!DateTime.TryParse(this.GrTime, out time)) return new DateTime(0);
                return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
            }
        }


    }

    class Helpers
    {
        public static string odstranNuly(string pString)
        {

            int index = 0;
            foreach (char ch in pString)
            {
                if (ch == '0') index++;
                else break;
            }

            return pString.Substring(index);

        }
    }
}