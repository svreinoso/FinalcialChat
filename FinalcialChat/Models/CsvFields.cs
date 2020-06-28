using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalcialChat.Models
{
    [DelimitedRecord(",")]
    public class CsvFields
    {
        public string Symbol { get; set; }
        [FieldConverter(ConverterKind.Date, "MM-dd-yyyy")]
        public DateTime Date { get; set; }
        [FieldConverter(ConverterKind.Date, "hh:mm:ss")]
        public TimeSpan Time { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public int Volume { get; set; }
    }
}