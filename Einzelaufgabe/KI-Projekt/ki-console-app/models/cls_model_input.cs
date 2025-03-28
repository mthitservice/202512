public class ModelInput
{
    public DateTime record_day { get; set; }

    public float elevation { get; set; }
    public float distanceInMeter { get; set; }
    public float TimeInSecond { get; set; }
    public float gender { get; set; }

    public float Month { get; set; }

    public float Age { get; set; }

    public float Year { get; set; }


}

public class ModelOutput
{
    public float[] ForecastedRentals { get; set; }

    public float[] LowerBoundRentals { get; set; }

    public float[] UpperBoundRentals { get; set; }
}