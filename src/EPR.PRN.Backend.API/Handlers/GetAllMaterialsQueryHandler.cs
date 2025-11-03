using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Services.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

/// <summary>
/// Handler for the <see cref="GetAllMaterialsQuery"/>.
/// </summary>
/// <param name="materialService">Service for handling materials.</param>
public class GetAllMaterialsQueryHandler(
    IMaterialService materialService
) : IRequestHandler<GetAllMaterialsQuery, IList<MaterialDto>>
{
    /// <inheritdoc />>.
    public async Task<IList<MaterialDto>> Handle(GetAllMaterialsQuery request, CancellationToken cancellationToken)
        => await materialService.GetAllMaterialsAsync(cancellationToken);
}