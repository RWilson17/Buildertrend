using System;

namespace BuildertrendMVC.ViewModels
{
    public class DashboardQuoteViewModel
    {
        public string QuoteNumber { get; set; }
        public string Estado { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Total { get; set; }
    }
}