using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Services.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

/// <summary>
/// Handler for the <see cref="GetMaterialsByRegistrationIdQuery"/>.
/// </summary>
/// <param name="materialService">Service for handling materials.</param>
public class GetMaterialsByRegistrationIdQueryHandler(
    IMaterialService materialService
) : IRequestHandler<GetMaterialsByRegistrationIdQuery, IList<MaterialDto>>
{
    /// <inheritdoc />>.
    public async Task<IList<MaterialDto>> Handle(GetMaterialsByRegistrationIdQuery request, CancellationToken cancellationToken)
        => await materialService.GetMaterialsByRegistrationIdQuery(request.RegistrationId);
}