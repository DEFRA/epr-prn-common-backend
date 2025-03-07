namespace EPR.PRN.Backend.Obligation.Interfaces
{
    public interface IMaterialCalculationService
    {
        int Calculate(double target, int tonnage);
        (int remelt, int remainder) CalculateGlass(double target, double remeltTarget, int tonnage);
    }
}
