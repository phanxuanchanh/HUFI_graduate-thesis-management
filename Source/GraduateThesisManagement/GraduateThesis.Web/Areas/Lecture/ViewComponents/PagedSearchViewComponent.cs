﻿using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents;

[ViewComponent(Name = "FacultyStaffLayout_PagedSearch")]
public class PagedSearchViewComponent : ViewComponent
{
    public PagedSearchViewComponent()
    {

    }

    public IViewComponentResult Invoke(string action = "Index", bool enableOrder = true)
    {
        ViewData["Action"] = action;
        ViewData["EnableOrder"] = enableOrder;

        return View();
    }
}
