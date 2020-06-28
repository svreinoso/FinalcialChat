using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalcialChat.Models
{
    [DelimitedRecord(",")]
    [IgnoreFirst]
    public class CsvFields
    {
        public string Symbol { get; set; }
        [FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
        public DateTime Date { get; set; }
        [FieldConverter(typeof(TimeSpanConverter))]
        public TimeSpan Time { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public int Volume { get; set; }
    }

    public class TimeSpanConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            return TimeSpan.Parse(from);
        }

        public override string FieldToString(object fieldValue)
        {
            return ((TimeSpan)fieldValue).ToString();
        }

    }
}