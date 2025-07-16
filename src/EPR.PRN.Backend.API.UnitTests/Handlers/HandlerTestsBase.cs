using EPR.PRN.Backend.API.UnitTests.Mapper.Registration;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

public class HandlerTestsBase<TRepository> : IMappingTestBase where TRepository : class, IRepositoryMarker
{
    public IMappingTestBase Mapper => this;

    public Mock<TRepository> MockRepository { get; set; } = new();
}