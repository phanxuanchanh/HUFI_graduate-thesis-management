using GraduateThesis.ApplicationCore.Models;

namespace GraduateThesis.ApplicationCore.Repository;

/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>

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
