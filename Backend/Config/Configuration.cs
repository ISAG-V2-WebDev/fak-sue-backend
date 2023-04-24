namespace Backend.Config;

public class Configuration
{
    public static IConfiguration StaticConfig { get; private set; } = null!;

    public Configuration(IConfiguration configuration)
    {
        Console.WriteLine("Configuration Instatiated!!!");
        StaticConfig = configuration;
    }
}