using GraduateThesis.Models;
using System.Threading.Tasks;

namespace GraduateThesis.RepositoryPatterns
{
    public interface IAccountPattern
    {
        Task<SignInResultModel> SignInAsync(SignInModel signInModel);
        Task<AccountVerificationModel> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel);
        Task<NewPasswordModel> VerifyAccountAsync(AccountVerificationModel accountVerificationModel);
        Task<ForgotPasswordModel> CreateNewPasswordAsync(NewPasswordModel newPasswordModel);

        SignInResultModel SignIn(SignInModel signInModel);
        AccountVerificationModel ForgotPassword(ForgotPasswordModel forgotPasswordModel);
        NewPasswordModel VerifyAccount(AccountVerificationModel accountVerificationModel);
        ForgotPasswordModel CreateNewPassword(NewPasswordModel newPasswordModel);
    }
}
