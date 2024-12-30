using System;
using System.Collections.Generic;

namespace BankApiTest.Models;

public partial class Currency
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Symbol { get; set; }

    public string Rate { get; set; } = null!;

    public string? Description { get; set; }

    public decimal RateFloat { get; set; }
}
