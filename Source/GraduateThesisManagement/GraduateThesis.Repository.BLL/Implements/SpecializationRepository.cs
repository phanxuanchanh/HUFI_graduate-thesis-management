using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;

namespace GraduateThesis.Repository.BLL.Implements;

public class SpecializationRepository : SubRepository<Specialization, SpecializationInput, SpecializationOutput, string>, ISpecializationRepository
{
    internal SpecializationRepository(HufiGraduateThesisContext context) 
        : base(context, context.Specializations)
    {

    }

    protected override void ConfigureIncludes()
    {
        
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new SpecializationOutput
        {

        };

        ListSelector = PaginationSelector;

        SingleSelector = s => new SpecializationOutput
        {

        };
    }
}
