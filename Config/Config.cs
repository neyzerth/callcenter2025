public class Config
{
    public ConfigSqlServer SqlServer { get; set; }
    public ConfigPaths Paths { get; set; }
    public static Config Configuration { get; set; }
}