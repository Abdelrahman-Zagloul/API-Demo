using API_Demo.Configuration;

namespace API_Demo.Settings
{
    public class ExternalLoginSettings
    {
        public ProviderSettings Google { get; set; }
        public ProviderSettings Github { get; set; }
        public ProviderSettings LinkedIn { get; set; }
        public ProviderSettings Microsoft { get; set; }
    }
}
