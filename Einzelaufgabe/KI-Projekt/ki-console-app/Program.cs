// See https://aka.ms/new-console-template for more information
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;

string rootDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../"));
string dbFilePath = Path.Combine(rootDir, "Data", "DailyDemand.mdf");
string modelPath = Path.Combine(rootDir, "MLModel.zip");

Console.WriteLine("Datenausgabe");



MyDbContext _context = new MyDbContext();

var data = _context.Records.Include(e => e.sportsevent).ThenInclude(d => d.disciplin).ToList();
foreach (var item in data)
{
    Console.WriteLine($"{item.firstname} {item.lastname} {item.record_day} {item.sportsevent.disciplin.name} {item.sportsevent.name} {item.Distance} {item.timeinSecond} {item.gender}");
}

Console.ReadLine();

MLContext mlContext = new MLContext();
String connectionString = "Server=172.16.0.2,1433;Database=BZE_TimeKeeper;User Id=BZE;Password=BZE2025!#;TrustServerCertificate=False;Encrypt=False";
DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<ModelInput>();

string query = "SELECT RentalDate, CAST(Year as REAL) as Year, CAST(TotalRentals as REAL) as TotalRentals FROM Rentals";

DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance,
                                connectionString,
                                query);

IDataView dataView = loader.Load(dbSource);

IDataView firstYearData = mlContext.Data.FilterRowsByColumn(dataView, "Year", upperBound: 1);
IDataView secondYearData = mlContext.Data.FilterRowsByColumn(dataView, "Year", lowerBound: 1);



var forecastingPipeline = mlContext.Forecasting.ForecastBySsa(
    outputColumnName: "ForecastedRentals",
    inputColumnName: "TotalRentals",
    windowSize: 7,
    seriesLength: 30,
    trainSize: 365,
    horizon: 7,
    confidenceLevel: 0.95f,
    confidenceLowerBoundColumn: "LowerBoundRentals",
    confidenceUpperBoundColumn: "UpperBoundRentals");


SsaForecastingTransformer forecaster = forecastingPipeline.Fit(firstYearData);

Evaluate(secondYearData, forecaster, mlContext);

var forecastEngine = forecaster.CreateTimeSeriesEngine<ModelInput, ModelOutput>(mlContext);

forecastEngine.CheckPoint(mlContext, modelPath);

Forecast(secondYearData, 7, forecastEngine, mlContext);


void Evaluate(IDataView testData, ITransformer model, MLContext mlContext)
{
    IDataView predictions = model.Transform(testData);
    IEnumerable<float> actual =
    mlContext.Data.CreateEnumerable<ModelInput>(testData, true)
        .Select(observed => observed.TimeInSecond);

    IEnumerable<float> forecast =
mlContext.Data.CreateEnumerable<ModelOutput>(predictions, true)
    .Select(prediction => prediction.ForecastedRentals[0]);
    var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);
    var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Error
    var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); // Root Mean Squared Error
    Console.WriteLine("Evaluation Metrics");
    Console.WriteLine("---------------------");
    Console.WriteLine($"Mean Absolute Error: {MAE:F3}");
    Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}\n");

}

void Forecast(IDataView testData, int horizon, TimeSeriesPredictionEngine<ModelInput, ModelOutput> forecaster, MLContext mlContext)
{
    ModelOutput forecast = forecaster.Predict();
    IEnumerable<string> forecastOutput =
    mlContext.Data.CreateEnumerable<ModelInput>(testData, reuseRowObject: false)
        .Take(horizon)
        .Select((ModelInput rental, int index) =>
        {
            string rentalDate = rental.record_day.ToShortDateString();
            float actualRentals = rental.TimeInSecond;
            float lowerEstimate = Math.Max(0, forecast.LowerBoundRentals[index]);
            float estimate = forecast.ForecastedRentals[index];
            float upperEstimate = forecast.UpperBoundRentals[index];
            return $"Date: {rentalDate}\n" +
            $"Actual Rentals: {actualRentals}\n" +
            $"Lower Estimate: {lowerEstimate}\n" +
            $"Forecast: {estimate}\n" +
            $"Upper Estimate: {upperEstimate}\n";

            Console.WriteLine("Rental Forecast");
            Console.WriteLine("---------------------");
            foreach (var prediction in forecastOutput)
            {
                Console.WriteLine(prediction);
            }
        });
}