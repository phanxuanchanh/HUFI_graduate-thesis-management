﻿using GraduateThesis.ExtensionMethods;
using GraduateThesis.Generics;
using GraduateThesis.Models;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DAL;
using GraduateThesis.Repository.DTO;
using System;
using System.Threading.Tasks;

namespace GraduateThesis.Repository.BLL.Implements;

public class FacultyStaffRepository : SubRepository<FacultyStaff, FacultyStaffInput, FacultyStaffOutput, string>, IFacultyStaffRepository
{
    private HufiGraduateThesisContext _context;

    internal FacultyStaffRepository(HufiGraduateThesisContext context)
        : base(context, context.FacultyStaffs)
    {
        _context = context;
    }

    protected override void ConfigureIncludes()
    {

    }

    protected override void ConfigureSelectors()
    {
        PaginationSelector = s => new FacultyStaffOutput
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email,
            Phone = s.Phone
        };

        ListSelector = PaginationSelector;
        SingleSelector = s => new FacultyStaffOutput
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email,
            Phone = s.Phone,
        };

        SimpleImportSelector = r => new FacultyStaff
        {
            Id = r[0] as string,
            FullName = r[1] as string,
            Email = r[2] as string,
            Phone = r[3] as string,
            Description = r[4] as string,
            CreatedAt = DateTime.Now
        };
    }


    public ForgotPasswordModel CreateNewPassword(NewPasswordModel newPasswordModel)
    {
        throw new NotImplementedException();
    }

    public Task<ForgotPasswordModel> CreateNewPasswordAsync(NewPasswordModel newPasswordModel)
    {
        throw new NotImplementedException();
    }

    public AccountVerificationModel ForgotPassword(ForgotPasswordModel forgotPasswordModel)
    {
        throw new NotImplementedException();
    }

    public Task<AccountVerificationModel> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
    {
        throw new NotImplementedException();
    }

    public SignInResultModel SignIn(SignInModel signInModel)
    {
        FacultyStaff facultyStaff = _context.FacultyStaffs.Find(signInModel.Code);
        if (facultyStaff == null)
            return new SignInResultModel { Status = SignInStatus.NotFound };

        string passwordAndSalt = $"{signInModel.Password}>>>{facultyStaff.Salt}";

        if (!BCrypt.Net.BCrypt.Verify(passwordAndSalt, facultyStaff.Password))
            return new SignInResultModel { Status = SignInStatus.WrongPassword };

        return new SignInResultModel { Status = SignInStatus.Success };
    }

    public async Task<SignInResultModel> SignInAsync(SignInModel signInModel)
    {
        FacultyStaff facultyStaff = await _context.FacultyStaffs.FindAsync(signInModel.Code);
        if (facultyStaff == null)
            return new SignInResultModel { Status = SignInStatus.NotFound };

        string passwordAndSalt = $"{signInModel.Password}>>>{facultyStaff.Salt}";

        if (!BCrypt.Net.BCrypt.Verify(passwordAndSalt, facultyStaff.Password))
            return new SignInResultModel { Status = SignInStatus.WrongPassword };

        return new SignInResultModel { Status = SignInStatus.Success };
    }

    public NewPasswordModel VerifyAccount(AccountVerificationModel accountVerificationModel)
    {
        throw new NotImplementedException();
    }

    public Task<NewPasswordModel> VerifyAccountAsync(AccountVerificationModel accountVerificationModel)
    {
        throw new NotImplementedException();
    }
}
