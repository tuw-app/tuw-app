using System;
using System.Globalization;

namespace LoggerLibrary.CustomDateFormatterLib
{
    public class CustomDateFormatter : IFormatProvider
    {
        // https://github.com/serilog/serilog/wiki/Formatting-Output
        readonly IFormatProvider basedOn;
        readonly string shortDatePattern;
        public CustomDateFormatter(string shortDatePattern, IFormatProvider basedOn)
        {
            this.shortDatePattern = shortDatePattern;
            this.basedOn = basedOn;
        }
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(DateTimeFormatInfo))
            {
                var basedOnFormatInfo = (DateTimeFormatInfo)basedOn.GetFormat(formatType);
                var dateFormatInfo = (DateTimeFormatInfo)basedOnFormatInfo.Clone();
                dateFormatInfo.ShortDatePattern = this.shortDatePattern;
                return dateFormatInfo;
            }
            return this.basedOn.GetFormat(formatType);
        }
    }
}
