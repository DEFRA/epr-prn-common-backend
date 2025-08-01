﻿using EPR.PRN.Backend.API.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Commands;
public class UpdateApplicationRegistrationTaskStatusCommand : IRequest
{
    public string TaskName { get; set; } = string.Empty;

    public TaskStatuses Status { get; set; }

    [BindNever]
    [SwaggerIgnore]
    public Guid RegistrationId { get; set; }
}