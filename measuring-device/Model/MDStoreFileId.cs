using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MeasureDeviceProject.Model
{
    /// <summary>
    /// Meghatározza milyen időköőnként lesznek a méréséek eltárolva forgó módszerrel
    /// </summary>
    public enum StorePeriod { EveryDay, EveryHour, EveryMinit }


    /// <summary>
    /// Az aktuális időpont alapján meghatározza a mérések fájl neveit
    /// </summary>
    public class MDStoreFileId
    {
        // A tárolás ezen időbélyeg alapján történik jelenleg
        private DateTime actulMeasureFileTimeStamp;
        
        // A tárolás peróodusa
        private StorePeriod storePeriod;

        // Az időbélyeg ás periódus alapján meghatározott fájl név
        private string measruringPeriodicFileName;
        public string GetMeasruringPeriodicFileName
        {
            get 
            {   
                return measruringPeriodicFileName; 
            }
        }

        // Meghatározza a fájl nevet is!
        public MDStoreFileId(DateTime measureDate, StorePeriod storePeriod)
        {
            this.actulMeasureFileTimeStamp = measureDate;
            this.storePeriod = storePeriod;
            DetermineMeasruringPeriodicFileName();
        }

        // Tárolja az új TimeStam-ot és meghatározza az új fájlnevet
        public void SetActulMeasureFileTimeStamp(DateTime measureDate)
        {
            this.actulMeasureFileTimeStamp = measureDate;
            DetermineMeasruringPeriodicFileName();
        }

        // Az utolsó mérés időbélyege még az aktuális forgóban van-e?
        public bool IsTheMesureTimeStampGood(DateTime actutalMeasure)
        { 
            // BUG 23:59
            if (storePeriod==StorePeriod.EveryMinit)
            {
                if (actulMeasureFileTimeStamp.Minute != actutalMeasure.Minute)
                    return false;
                else
                    return true;
            }
            else if (storePeriod == StorePeriod.EveryHour)
            {
                if (actutalMeasure.Hour != actulMeasureFileTimeStamp.Hour)
                    return false;
                else
                    return true;
            }
            else  //if (storePeriod == StorePeriod.EveryDay)
            {
                if (actutalMeasure.Day != actulMeasureFileTimeStamp.Day)
                    return false;
                else
                    return true;
            }

        }
        /// <summary>
        /// Az tárolás időbpont bélyeg és mérés forgó alapján meghatározza a fájl nevet
        /// </summary>
        /// <returns></returns>
        private void  DetermineMeasruringPeriodicFileName()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(actulMeasureFileTimeStamp.Year)
                        .Append("-")
                        .Append(actulMeasureFileTimeStamp.Month)
                        .Append("-")
                        .Append(actulMeasureFileTimeStamp.Day);                    
            switch (storePeriod)
            {
                case StorePeriod.EveryMinit:                    
                    sb.Append("-")
                         .Append(actulMeasureFileTimeStamp.Hour)
                        .Append("-")
                        .Append(actulMeasureFileTimeStamp.Minute);
                    break;
                case StorePeriod.EveryHour:
                    sb.Append("-").Append(actulMeasureFileTimeStamp.Hour);
                    break;
                case StorePeriod.EveryDay:
                    break;
            }
            sb.Append(".txt");
            measruringPeriodicFileName = sb.ToString();
        }

        public override string ToString()
        {
            return GetMeasruringPeriodicFileName;
        }
    }
}
