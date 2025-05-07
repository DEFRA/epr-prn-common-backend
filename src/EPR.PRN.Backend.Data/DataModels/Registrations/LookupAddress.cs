﻿using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[ExcludeFromCodeCoverage]
public class LookupAddress
{
    public int Id { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string TownCity { get; set; }
    public string? County { get; set; }
    public string Country { get; set; }
    public string PostCode { get; set; }
    public int NationId { get; set; }
    public string GridReference { get; set; }
}