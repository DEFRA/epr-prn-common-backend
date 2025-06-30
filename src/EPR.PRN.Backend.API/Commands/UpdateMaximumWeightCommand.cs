using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Commands;

/// <summary>
/// Defines a command for updating the maximum weight a site is capable of processing for a registration material.
/// </summary>
[ExcludeFromCodeCoverage]
public class UpdateMaximumWeightCommand : IRequest
{
    /// <summary>
    /// The unique identifier for the registration material.
    /// </summary>
    [SwaggerIgnore]
    [BindNever]
    public Guid RegistrationMaterialId { get; set; }

    /// <summary>
    /// The maximum weight in tonnes.
    /// </summary>
    public required decimal WeightInTonnes { get; set; }

    /// <summary>
    /// The ID of the period within which the max weight is applicable.
    /// </summary>
    public int PeriodId { get; set; }
}