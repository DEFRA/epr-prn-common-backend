using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

/// <summary>
/// Handles retrieval of material permit types.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GetMaterialsPermitTypesQueryHandler"/> class.
/// </remarks>
/// <param name="repository">The material repository.</param>
public class GetMaterialsPermitTypesQueryHandler(IRegistrationMaterialRepository repository) : IRequestHandler<GetMaterialsPermitTypesQuery, List<MaterialsPermitTypeDto>>
{
    private readonly IRegistrationMaterialRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <inheritdoc />
    public async Task<List<MaterialsPermitTypeDto>> Handle(GetMaterialsPermitTypesQuery request, CancellationToken cancellationToken)
    {
        var materialPermitTypes = await _repository.GetMaterialPermitTypes();

        if (materialPermitTypes is null || !materialPermitTypes.Any())
        {
            return [];
        }

        return materialPermitTypes.Select(x => new MaterialsPermitTypeDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
    }
}
