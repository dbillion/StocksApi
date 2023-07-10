using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AlphaVantage.Net.Core.Client;
using AlphaVantage.Net.Stocks;
using AlphaVantage.Net.Stocks.Client;
using AlphaVantage.Net.Common.Size;
using AlphaVantage.Net.Common.Intervals;
using Microsoft.AspNetCore.Cors;
using NodaTime;
using Interval = AlphaVantage.Net.Common.Intervals.Interval;
using Microsoft.IdentityModel.Tokens;
using StocksApi.Models;
using StocksApi.ProjectModels;
using StocksApi.Controllers;


namespace StocksApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StockController : ControllerBase
	{

		StockDbContext stockDbEntities = null;
		List<StockModel> stockModels = new List<StockModel>();
		private async Task<object> GetStockClosingPrice(string stockId,
		DateTime startDate, DateTime endDate)
		{
			string apiKey = "2";
			using var client = new AlphaVantageClient(apiKey);
			using var stocksClient = client.Stocks();
			StockTimeSeries stockTs = await stocksClient.
			GetTimeSeriesAsync(stockId, Interval.Daily, OutputSize.
			Compact, isAdjusted: true);

			var query = stockTs.DataPoints.Where(c => c.Time >= startDate
&& c.Time <= endDate);
			return query.LastOrDefault().ClosingPrice;
		}
		[Route("~/api/GetDashboardPortfolioData")]
		[HttpGet]
		public List<StockModel> GetDashboardPortfolioData()
		{
			var startDate = DateTime.UtcNow.AddDays(-5);
			var endDate = DateTime.UtcNow;
			stockDbEntities = new StockDbContext();
			var stockmasters = stockDbEntities.StockMasters
			.Where(x => x.IsActive == true).ToList();
			stockmasters.ForEach(x =>
			{
				var stockClosingData = GetStockClosingPrice(x.StockId,
				startDate, endDate).Result;
				if (stockClosingData != null)
				{
					StockModel stockModel = new StockModel();
					stockModel.StockId = x.StockId;
					stockModel.StockName = x.StockName;
					stockModel.BuyPrice = x.BuyPrice;
					stockModel.Qty = x.Qty;
					stockModel.TotalInvested = x.Qty * x.BuyPrice;
					stockModel.CurrentValue = x.Qty * (decimal)
					stockClosingData;
					stockModel.TotalGain = stockModel.CurrentValue -
					stockModel.TotalInvested;
					stockModels.Add(stockModel);
				}
			});
			return stockModels.OrderBy(c => c.TotalGain).ToList();
		}


		[Route("~/api/GetStock")]
		[HttpGet]
		public List<StockModel> GetStock()
		{
			stockDbEntities = new StockDbContext();
			List<StockModel> stockModels = new List<StockModel>();
			var query = stockDbEntities.StockMasters.ToList();
			query.ForEach(x =>
			{
				StockModel stockModel = new StockModel();
				stockModel.StockId = x.StockId;
				stockModel.StockName = x.StockName;
				stockModels.Add(stockModel);
			});
			return stockModels;
		}
		[Route("~/api/AlphaAdvantage/AddStock")]
		[HttpPost]
		public IActionResult AddStock(string stockId)
		{
			var stockModel = new StockModel();
			stockModel.StockId = stockId;
			try
			{
				stockDbEntities = new StockDbContext();
				StockMaster stockMaster = new StockMaster();
				stockMaster.StockId = stockModel.StockId;
				stockMaster.StockName = stockModel.StockName;
				stockMaster.BuyPrice = stockModel.BuyPrice;
				stockMaster.Qty = stockModel.Qty;
				stockMaster.IsActive = stockModel.IsActive;
				stockDbEntities.StockMasters.Add(stockMaster);
				stockDbEntities.SaveChanges();
			}


			catch (Exception ex)
			{
				return NotFound();
			}
			return Ok(true);
		}
		[HttpGet]
		public List<StockMaster> GetActiveStock()
		{
			stockDbEntities = new StockDbContext();
			var stockmasters = stockDbEntities.StockMasters.Where(x =>
			x.IsActive == true).ToList();
			return stockmasters;
		}
		[Route("~/api/GetStockData/{ticker}/{start}/{end}/{period}")]
		[HttpGet]
		public async Task<List<StockModel>> GetStockData(string ticker = "",
		string start = "", string end = "", string period = "")
		{
			var p = AlphaVantage.Net.Common.Intervals.Interval.Monthly;
			var stockClosingData = GetStocksDataByDate(ticker, Convert.
			ToDateTime(start), Convert.ToDateTime(end), period).Result;
			List<StockModel> models = new List<StockModel>();
			foreach (var r in stockClosingData)
			{
				models.Add(new StockModel
				{
					StockId = ticker,
					StockName = ticker,
					Date = r.Time.ToString("yyyy-MM-dd"),
					Open = r.OpeningPrice,
					High = r.HighestPrice,
					Low = r.LowestPrice,


					Close = r.ClosingPrice,
					Volume = r.Volume
				});
			}
			return models.ToList();
		}
		private async Task<IEnumerable<StockDataPoint>>
		GetStocksDataByDate(string stockId, DateTime startDate, DateTime
		endDate, string period = "")
		{
			StockTimeSeries stockTs = null;
			string apiKey = "2";
			using var client = new AlphaVantageClient(apiKey);
			using var stocksClient = client.Stocks();
			if ((period == "daily") || (period.IsNullOrEmpty()))
			{
				stockTs = await stocksClient.GetTimeSeriesAsync(stockId,
				Interval.Daily, OutputSize.Compact, isAdjusted: true);
			}
			else if (period == "monthly")
			{
				stockTs = await stocksClient.GetTimeSeriesAsync(stockId, Interval.Monthly,
				OutputSize.Compact, isAdjusted: true);
			}
			var query = stockTs.DataPoints.Where(c => c.Time >= startDate
			&& c.Time <= endDate);
			return query.ToList();
		}
		[Route("~/api/GetGainerLoserStockData/{ticker}/{period}")]
		[HttpGet]
		public async Task<List<StockModel>> GetGainerLoserStockData(string
		ticker = "", string period = "")
		{
			var p = AlphaVantage.Net.Common.Intervals.Interval.Monthly;


			var startDate = DateTime.Now.AddMonths(-11);
			var endDate = DateTime.Now;
			var stockClosingData = GetStocksDataByDate(ticker, startDate,
			endDate).Result;
			List<StockModel> models = new List<StockModel>();
			foreach (var r in stockClosingData)
			{
				models.Add(new StockModel
				{
					StockId = ticker,
					StockName = ticker,
					Date = r.Time.ToString("yyyy-MM-dd"),
					Open = r.OpeningPrice,
					High = r.HighestPrice,
					Low = r.LowestPrice,
					Close = r.ClosingPrice,
					Volume = r.Volume
				});
			}
			return models.ToList();
		}

	}
}
