using System;

namespace GraduateThesis.RepositoryPatterns
{
    public interface IRepositoryConfiguration
    {
        void ConfigureIncludes();
        void ConfigureSelectors();
    }
}
