using GraduateThesis.ApplicationCore.Repository;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;

namespace GraduateThesis.Repository.BLL.Implements;

public class CommitteeMemberRepository : AsyncSubRepository<CommitteeMember, CommitteeMemberInput, CommitteeMemberOutput, string>, ICommitteeMemberRepository
{
    private HufiGraduateThesisContext _context;

    public CommitteeMemberRepository(HufiGraduateThesisContext context)
        :base(context, context.CommitteeMembers)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {
        
    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new CommitteeMemberOutput
        {
            Id= s.Id,
            
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new CommitteeMemberOutput
        {

        };
    }

    protected override void SetMapperToCreate(CommitteeMemberInput input, CommitteeMember entity)
    {
        
    }

    protected override void SetMapperToUpdate(CommitteeMemberInput input, CommitteeMember entity)
    {
        
    }

    protected override void SetOutputMapper(CommitteeMember entity, CommitteeMemberOutput output)
    {
        
    }
}
