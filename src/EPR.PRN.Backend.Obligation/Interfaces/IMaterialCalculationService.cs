namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IMaterialCalculationService
    {
        int Calculate(double target, int tonnage, int? numberOfDaysObligated, int complianceYear);
        (int remelt, int remainder) CalculateGlass(double target, double remeltTarget, int tonnage, int? numberOfDaysObligated, int complianceYear);
    }
}
