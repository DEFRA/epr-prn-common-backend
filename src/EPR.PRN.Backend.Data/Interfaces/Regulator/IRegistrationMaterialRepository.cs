﻿
namespace EPR.PRN.Backend.Data.Interfaces.Regulator;
using EPR.PRN.Backend.API.Common.Dto.Regulator;


public interface IRegistrationMaterialRepository
{
    Task<string> UpdateRegistrationOutCome(int RegistrationMaterialId, int StatusId, string? Comment,string RegistrationReferenceNumber);
    Task<RegistrationMaterialDto> GetMaterialsById(int RegistrationId);
    Task<RegistrationOverviewDto> GetRegistrationOverviewDetailById(int RegistrationId);
}
