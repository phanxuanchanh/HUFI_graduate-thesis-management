﻿using GraduateThesis.ApplicationCore.AppSettings;

namespace GraduateThesis.Web;

public static class AppConfiguration
{
    public static void ConfigConnectionString(string connectionString)
    {
        AppDefaultValue.ConnectionString = connectionString;
    }

    public static void ConfigDefaultMessage()
    {
        AppDefaultValue.SuccessMsg = "Đã thực hiện thành công!";
        AppDefaultValue.AlreadyExistsMsg = "Dữ liệu đã tồn tại trong hệ thống!";
        AppDefaultValue.InvalidDataMsg = "Dữ liệu đầu vào không hợp lệ!";
        AppDefaultValue.NotFoundMsg = "Không tìm thấy dữ liệu này trong hệ thống!";
        AppDefaultValue.FailedMsg = "Đã thực hiện thất bại!";

        AppDefaultValue.AccAuthSuccessMsg = "Đã thực hiện thành công!";
        AppDefaultValue.AccAuthWrongPwdMsg = "Mật khẩu không khớp!";
        AppDefaultValue.AlreadyExistsMsg = "Đã tồn tại tài khoản trong hệ thống!";
        AppDefaultValue.AccAuthInvalidDataMsg = "Không hợp lệ!";
        AppDefaultValue.AccAuthLockedMsg = "Tài khoản đã bị khóa!";
        AppDefaultValue.AccAuthNotActivatedMsg = "Tài khoản chưa được kích hoạt!";
        AppDefaultValue.AccAuthFailedMsg = "Thất bại!";
    }
}