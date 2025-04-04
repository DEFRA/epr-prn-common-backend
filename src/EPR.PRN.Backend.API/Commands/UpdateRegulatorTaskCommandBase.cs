﻿using EPR.PRN.Backend.API.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.API.Commands;
public class UpdateRegulatorTaskCommandBase : IRequest<Unit>
{
    [Required]
    [BindNever]
    [SwaggerIgnore]
    public int Id { get; set; }
    [Required]
    public required StatusTypes Status { get; set; }
    [MaxLength(500)]
    public string? Comment { get; set; } = string.Empty;
}