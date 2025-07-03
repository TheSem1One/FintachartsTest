namespace Fintacharts.Infrastructure.Options
{
    public class ConnectionOptions
    {
        public const string SectionName = "ConnectionString";
        public string ApiDatabase { get; set; } = null!;
    }
}
