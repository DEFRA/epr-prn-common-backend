namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IMaterialCalculationService
    {
        int Calculate(double target, double tonnage);
        (int remelt, int remainder) CalculateGlass(double target, double remeltTarget, double tonnage);
    }
}
