using GraduateThesis.Common.Authorization;
using System;

namespace GraduateThesis.WebExtensions
{
    public class AccountRole : RoleBase
    {
        public const string Student = nameof(Student);
        public const string Lecture = nameof(Lecture);
        public const string CouncilMember = nameof(CouncilMember);
        public const string FacultyStaff = nameof(FacultyStaff);
    }
}
