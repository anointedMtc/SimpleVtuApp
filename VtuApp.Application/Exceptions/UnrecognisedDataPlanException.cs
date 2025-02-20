namespace VtuApp.Application.Exceptions;

public class UnrecognisedDataPlanException : Exception
{
    public string DataPlan { get; }
    public UnrecognisedDataPlanException(string dataPlan) : base($"The Data Plan {dataPlan} is not valid")
    {
        DataPlan = dataPlan;
    }
}
