namespace EPR.PRN.Backend.Obligation.Interfaces;

public interface IDateTimeProvider
{
	DateTime UtcNow { get; }

	int CurrentYear { get; }
}

