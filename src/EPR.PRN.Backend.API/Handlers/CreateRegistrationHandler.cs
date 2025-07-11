using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

[ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of create registration user story")]
public class CreateRegistrationHandler(IRegistrationRepository repository, IMapper mapper)
    : IRequestHandler<CreateRegistrationCommand, CreateRegistrationDto>
{
    public async Task<CreateRegistrationDto> Handle(CreateRegistrationCommand command, CancellationToken cancellationToken)
    {
        var registration = await repository.CreateRegistrationAsync(command.ApplicationTypeId, command.OrganisationId, command.ReprocessingSiteAddress);

        var taskTypeNameMap = new Dictionary<int, string>
        {
            { 1, ApplicantRegistrationTaskNames.SiteAddressAndContactDetails },
            { 2, ApplicantRegistrationTaskNames.AddressForServiceofNotices }
        };

        var taskType = taskTypeNameMap.TryGetValue(command.ApplicationTypeId, out var name)
            ? name
            : throw new InvalidOperationException($"Unknown ApplicationTypeId: {command.ApplicationTypeId}");

        await repository.UpdateRegistrationTaskStatusAsync(taskType, registration.ExternalId, TaskStatuses.Started);

        return mapper.Map<CreateRegistrationDto>(registration);
    }
}