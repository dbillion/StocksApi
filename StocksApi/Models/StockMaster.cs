using System;
using System.Collections.Generic;

namespace StocksApi.Models;

public partial class StockMaster
{
    public int Id { get; set; }

    public string? StockId { get; set; }

    public string? StockName { get; set; }

    public decimal? BuyPrice { get; set; }

    public decimal? Qty { get; set; }

    public bool? IsActive { get; set; }
}
