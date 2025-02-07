using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEPZET
{

    delegate void dgEventLogAdd(string pText, LogType pType, int pIventID = 0);

            
class Helpers {


        public static EventLogEntryType getEventLogEntryType(LogType pLogtype) {

            if (pLogtype == LogType.info) return EventLogEntryType.Information;
            if (pLogtype == LogType.warning) return EventLogEntryType.Warning;
            if (pLogtype == LogType.error) return EventLogEntryType.Error;
            return (EventLogEntryType.Information);

        }



        public static string RemoveZeros(String pString) {
            
                return  pString.TrimStart(new Char[] { '0' });
                
            
        }

        public static string getPackDisplayText(int i, int count, List<palletSet> ps)
        {
            //generate pack display list
            string packText = "";
            if (i == 0)
                return packText;

            for (int j = 0; j < count; j++)
            {
                if (i > j)
                {
                    string pn = ps[j].palletNr;
                    pn = pn.Remove(0, pn.Length - 4);
                    packText += "G" + pn + "|";
                }
                else
                {
                    int k = j + 1;
                    string kStr = k.ToString();
                    while (kStr.Length <= 4)
                        kStr += " ";

                    packText += "B" + kStr + "|";

                }
            }
            return packText;
        }

        internal static string getPackDisplayText(int i, int count, List<palletSet> ps, string[] pos)
        {
            //generate pack display list
            string packText = "";
            if (i == 0)
                return packText;

            for (int j = 0; j < pos.Count(); j++)
            {

                string pn = pos[j];
                pn = pn.Remove(0, pn.Length - 4);
                if (ps.Contains(new palletSet(pos[j], null)))
                    packText += "G" + pn + "|";
                else
                    packText += "B" + pn + "|";

            }
            return packText;
        }
    }
    
    internal class palletSet : IEquatable<palletSet>
    {
        public palletSet(string p, string f)
        {
            this.palletNr = p;
            this.fifoTime = f;
        }
        //public Int32 id {get; set; }
        public string palletNr { get; set; }
        public string fifoTime { get; set; }

        public bool Equals(palletSet other)
        {
            if (this.palletNr == other.palletNr)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
    }

    public enum LogType
    {
        info = 0,
        warning = 1,
        error = 2
    }
}
