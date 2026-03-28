namespace Atelie.Core
{
    /// <summary>
    /// Define constantes de texto para os namespaces da solução
    /// </summary>
    public static class SolutionLayers
    {
        public const string Core = "Atelie.Core";
        public const string Domain = "Atelie.Domain";
        public const string Infrastructure = "Atelie.Infrastructure";
        public const string Web = "Atelie.Web";

        public static class DomainLayers
        {
            public const string Entities = Domain + ".Entities";
        }

        public static class InfrastructureLayers
        {
            public const string Abstractions = Infrastructure + ".Abstractions";
            public const string AbstractionsRepositories = Abstractions + ".IRepositories";
            public const string AbstractionsServices = Abstractions + ".IServices";

            public const string Persistence = Infrastructure + ".Persistence";
            public const string Mappings = Persistence + ".Mappings";

            public const string Repositories = Infrastructure + ".Repositories";

            public const string Services = Infrastructure + ".Services";
        }

        public static class WebLayers
        {
            public const string Controllers = Web + ".Controllers";
            public const string ViewModels = Web + ".ViewModels";
        }
    }
}
