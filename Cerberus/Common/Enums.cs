
namespace Cerberus
{
    public enum SensorType
    {
        None,
        Pressure,
        Temperature,
        Conductivity
    }

    public enum ModuleType
    {
        PressureModule,
        TemperatureConductivityModule
    }

    public enum ModuleParameterStatus
    {
        All,
        TemperatureOnly,
        ConductivityOnly
    }

    public enum LogRateInterval
    {
        Hours,
        Minutes,
        Seconds
    }

    public enum AlgorithmType
    {
        None,
        DifferentialPressure, 
        TransmembranePressure,
        DifferentialTemperature,
        CustomEquation      
    }

    public enum TrendType 
    { 
        None,
        SensorModule,
        Experiment 
    }
}
