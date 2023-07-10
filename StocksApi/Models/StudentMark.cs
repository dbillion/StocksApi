using System;
using System.Collections.Generic;

namespace StocksApi.Models;

public partial class StudentMark
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? English { get; set; }

    public int? Maths { get; set; }

    public int? Science { get; set; }
}
