using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace StocksApi.ProjectModels
{
    public class StockModel
    {
		public string StockId { get; set; }
		public string StockName { get; set; }
		public string Date { get; set; }
		public decimal Open { get; set; }
		public decimal High { get; set; }
		public decimal Low { get; set; }
		public decimal Close { get; set; }
		public decimal Volume { get; set; }
		public decimal? BuyPrice { get; set; }
		public decimal? Qty { get; set; }
		public bool IsActive { get; set; }
		public decimal? TotalInvested { get; set; }
		public decimal? CurrentValue { get; set; }
		public decimal? TotalGain { get; set; }
	}
}
