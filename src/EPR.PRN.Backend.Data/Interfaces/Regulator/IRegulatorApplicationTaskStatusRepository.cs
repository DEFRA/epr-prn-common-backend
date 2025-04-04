﻿using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.Interfaces.Regulator
{
    public interface IRegulatorApplicationTaskStatusRepository
    {
        Task<RegulatorApplicationTaskStatus> GetTaskStatusByIdAsync(int id);
        Task UpdateStatusAsync(int id, StatusTypes status, string? comments);
    }
}