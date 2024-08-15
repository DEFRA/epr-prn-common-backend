﻿using EPR.PRN.Backend.Obligation.Enums;

namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IRecyclingTargetDataService
    {
        Task<Dictionary<int, Dictionary<MaterialType, double>>> GetRecyclingTargetsAsync();
    }
}