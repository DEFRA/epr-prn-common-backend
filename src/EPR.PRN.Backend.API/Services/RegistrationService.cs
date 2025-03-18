using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Helpers;


namespace EPR.PRN.Backend.API.Services
{
    public class RegistrationService(IRegistrationRepository repository, ILogger<RegistrationService> logger) : IRegistrationService
    {
        public async Task<RegistrationDto?> GetByIdAsync(int id)
        {
            var registrationDto = new RegistrationDto();
            var addressDto = new List<AddressDto>();

            var registration = await repository.GetByIdAsync(id);

            if (registration is null)
            {
                logger.LogInformation("Registration was not found {id}.", id);
                throw new NotFoundException($"Registration with id {id} was not found in system");
            }

            registrationDto.RegistrationStatusId = registration.RegistrationStatusId;
            registrationDto.ApplicationTypeId = registration.ApplicationTypeId;
            registrationDto.OrganisatonId = registration.OrganisationId;
            registrationDto.UpdatedBy = registration.UpdatedBy;
            registrationDto.CreatedBy = registration.CreatedBy;
            registrationDto.UpdatedDate = registration.UpdatedDate;
            registrationDto.CreatedDate = registration.CreatedDate;

            registrationDto.Addresses = GetAddresses(registration);

            return registrationDto;
        }


        #region private methods
        private static List<AddressDto> GetAddresses(Registration registration)
        {
            var addressDto = new List<AddressDto>();

            if(registration.BusinessAddress != null)
            {
                addressDto.Add(GetAddressDto(registration.BusinessAddress, RegistrationAddressConstants.BusinessAddress));
            }

            if (registration.ReprocessingSiteAddress != null)
            {
                addressDto.Add(GetAddressDto(registration.ReprocessingSiteAddress, RegistrationAddressConstants.ReprocessingSiteAddress));
            }

            if (registration.LegalDocumentAddress != null)
            {
                addressDto.Add(GetAddressDto(registration.LegalDocumentAddress, RegistrationAddressConstants.LegalDocumentAddress));
            }
            return addressDto;
        }
        private static AddressDto GetAddressDto(Address address, string type) {
            return new AddressDto
            {
                AddressType = type,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                County = address.County,
                Postcode = address.Postcode,
                TownCity = address.TownCity,
                GridReference = address.GridReference,  
                NationId = address.NationId,
            };
        }
        #endregion
    }
}
