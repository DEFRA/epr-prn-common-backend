using EPR.PRN.Backend.Data.DataModels.Registrations;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.API.Commands;
public class CreateReprocessorOutputCommand : IRequest
{
    [BindNever]
    [SwaggerIgnore]
    public Guid ReprocessorOutputId { get; set; }
    public int RegistrationMaterialId { get; set; }
    public decimal SentToOtherSiteTonnes { get; set; }
    public decimal ContaminantTonnes { get; set; }
    public decimal ProcessLossTonnes { get; set; }
    public decimal TotalOutputTonnes { get; set; }
    public List<ReprocessorRawMaterialorProducts> RawMaterialorProducts { get; set; } = new();
}
public class ReprocessorRawMaterialorProducts
{
    public string RawMaterialNameorProductName { get; set; }
    public decimal TonneValue { get; set; }   
}