using AutoFixture.Kernel;
using AutoFixture;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.UnitTests;

[ExcludeFromCodeCoverage(Justification ="Autofixture getting stuck on circular references")]
public class NoCircularReferencesCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(behavior => fixture.Behaviors.Remove(behavior));
    }
}

[ExcludeFromCodeCoverage(Justification = "Autofixture getting stuck on circular references")]
public class IgnoreVirtualMembersCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new IgnoreVirtualMembers());
    }
}

[ExcludeFromCodeCoverage(Justification = "Autofixture getting stuck on circular references")]
public class IgnoreVirtualMembers : ISpecimenBuilder
{
    public object? Create(object request, ISpecimenContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        var propertyInfo = request as PropertyInfo;
        if (propertyInfo == null)
        {
            return new NoSpecimen();
        }

        if (propertyInfo.GetMethod != null && propertyInfo.GetMethod.IsVirtual)
        {
            return null;
        }

        return new NoSpecimen();
    }
}
