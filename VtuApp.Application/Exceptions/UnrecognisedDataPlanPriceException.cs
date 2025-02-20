namespace VtuApp.Application.Exceptions;

public class UnrecognisedDataPlanPriceException : Exception
{
    public string DataPlan { get; }
    public UnrecognisedDataPlanPriceException(string dataPlan) : base($"The Price for the Data Plan {dataPlan} was not found")
    {
        DataPlan = dataPlan;
    }
}
