using TechTalk.SpecFlow;

namespace WindowsPhoneTestFramework.Test.EmuSteps
{
    public class ConfigurableDefinitionBase : Steps
    {
        private readonly IConfiguration _configuration;

        protected IConfiguration Configuration { get { return _configuration; } }

        public ConfigurableDefinitionBase()
            : this(new AppConfigFileBasedConfiguration())
        {            
        }

        public ConfigurableDefinitionBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}