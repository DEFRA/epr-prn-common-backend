using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[TestClass]
public class RegistrationRepositoryTests
{
    private EprRegistrationsContext _context;
    private RegistrationRepository _repository;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EprRegistrationsContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EprRegistrationsContext(options);

        _repository = new RegistrationRepository(_context);
    }

    [TestMethod]
    public async Task UpdateSiteAddress_ShouldUpdateRegistrationWithNewAddresses_WhenAddressesHaveNoId()
    {
        // Arrange
        var registration = new Registration { Id = 1, ExternalId = "ExternalID-1" };
        _context.Registrations.Add(registration);
        await _context.SaveChangesAsync();

        var reprocessingAddress = new AddressDto
        {
            AddressLine1 = "123 Test St",
            AddressLine2 = "Test Area",
            TownCity = "Testville",
            Country = "Testland",
            PostCode = "TST 123",
            GridReference = "GB1234567890",
            County = "UK",
            NationId = 1,
        };

        var legalDocAddress = new AddressDto
        {
            AddressLine1 = "456 Doc Rd",
            AddressLine2 = "Doc Area",
            TownCity = "DocCity",
            Country = "Docland",
            PostCode = "DOC 456",
            GridReference = "GB1234567890",
            County = "UK",
            NationId = 1,
        };

        // Act
        await _repository.UpdateSiteAddress(registration.Id, reprocessingAddress, legalDocAddress);

        // Assert
        var updatedRegistration = await _context.Registrations.FirstAsync(r => r.Id == registration.Id);
        updatedRegistration.ReprocessingSiteAddressId.Should().NotBeNull();
        updatedRegistration.LegalDocumentAddressId.Should().NotBeNull();

        var reprocAddress = await _context.LookupAddresses.FindAsync(updatedRegistration.ReprocessingSiteAddressId);
        var legalAddress = await _context.LookupAddresses.FindAsync(updatedRegistration.LegalDocumentAddressId);

        reprocAddress.AddressLine1.Should().Be("123 Test St");
        legalAddress.AddressLine1.Should().Be("456 Doc Rd");
    }

    [TestMethod]
    public async Task UpdateSiteAddress_ShouldThrowException_WhenRegistrationNotFound()
    {
        // Arrange
        var reprocessingAddress = new AddressDto();
        var legalDocAddress = new AddressDto();

        // Act
        Func<Task> act = async () => await _repository.UpdateSiteAddress(999, reprocessingAddress, legalDocAddress);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Registration not found.");
    }

    [TestMethod]
    public async Task UpdateSiteAddress_ShouldReuseAddressIds_WhenAddressDtosContainIds()
    {
        // Arrange
        var registration = new Registration { Id = 2, ExternalId = "ExternalID-2" };
        var lookupAddress1 = new LookupAddress
        {
            Id = 101,
            AddressLine1 = "123 Test St",
            AddressLine2 = "Test Area",
            TownCity = "Testville",
            Country = "Testland",
            PostCode = "TST 123",
            GridReference = "GB1234567890",
            County = "UK",
            NationId = 1,
        };

        var lookupAddress2 = new LookupAddress
        {
            Id = 102,
            AddressLine1 = "456 Doc Rd",
            AddressLine2 = "Doc Area",
            TownCity = "DocCity",
            Country = "Docland",
            PostCode = "DOC 456",
            GridReference = "GB1234567890",
            County = "UK",
            NationId = 1,
        };


        _context.Registrations.Add(registration);
        _context.LookupAddresses.AddRange(lookupAddress1, lookupAddress2);
        await _context.SaveChangesAsync();

        var reprocessingAddress = new AddressDto { Id = 101 };
        var legalDocAddress = new AddressDto { Id = 102 };

        // Act
        await _repository.UpdateSiteAddress(registration.Id, reprocessingAddress, legalDocAddress);

        // Assert
        var updatedRegistration = await _context.Registrations.FindAsync(registration.Id);
        updatedRegistration.ReprocessingSiteAddressId.Should().Be(101);
        updatedRegistration.LegalDocumentAddressId.Should().Be(102);
    }
}
