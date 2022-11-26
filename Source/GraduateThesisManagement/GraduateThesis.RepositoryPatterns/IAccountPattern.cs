using GraduateThesis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.RepositoryPatterns
{
    public interface IAccountPattern
    {
        Task<AccountVerificationModel> ForgotPassword(ForgotPasswordModel forgotPasswordModel);
        Task<NewPasswordModel> VerifyAccount(AccountVerificationModel accountVerificationModel);
        Task<ForgotPasswordModel> CreateNewPassword(NewPasswordModel newPasswordModel);
    }
}
