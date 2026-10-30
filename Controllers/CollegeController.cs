using CollegePortal.Domain;
using CollegePortal.Models;
using CollegePortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollegePortal.Controllers;

[Authorize]
public class CollegeController(CollegeStore store) : Controller
{
    public IActionResult Index() => View(store);

    public IActionResult Students()
    {
        ViewBag.Groups = store.Groups;
        return View(store.Students);
    }

    public IActionResult Teachers() => View(store.Teachers);

    public IActionResult Groups() => View(store.Groups);

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddStudent(string name, string surname, int age, string groupName)
    {
        try
        {
            store.AddStudent(name, surname, age, groupName);
            TempData["Notice"] = "Студент добавлен.";
        }
        catch (ArgumentException exception)
        {
            TempData["Error"] = exception.Message;
        }

        return RedirectToAction(nameof(Students));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddGroup(string name, int course)
    {
        try
        {
            store.AddGroup(name, course);
            TempData["Notice"] = "Группа создана.";
        }
        catch (Exception exception) when (exception is ArgumentException or InvalidOperationException)
        {
            TempData["Error"] = exception.Message;
        }

        return RedirectToAction(nameof(Groups));
    }

    public IActionResult Grades() => View(new GradePageViewModel
    {
        Grades = store.GradeBook.Grades,
        Students = store.Students,
        Subjects = store.Subjects
    });

    [Authorize(Roles = "Admin,Teacher")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddGrade(int studentId, string subject, int value)
    {
        var student = store.Students.FirstOrDefault(item => item.StudentId == studentId);
        if (student is null)
        {
            TempData["Error"] = "Студент не найден.";
        }
        else
        {
            try
            {
                store.GradeBook.AddGrade(student.StudentId, student.GetFullName(), subject, value);
                TempData["Notice"] = "Оценка добавлена.";
            }
            catch (ArgumentException exception)
            {
                TempData["Error"] = exception.Message;
            }
        }

        return RedirectToAction(nameof(Grades));
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveGrade(int id)
    {
        store.GradeBook.RemoveGrade(id);
        return RedirectToAction(nameof(Grades));
    }

    public IActionResult Attendance() => View(new AttendancePageViewModel
    {
        Records = store.Attendance.Records,
        Students = store.Students,
        Subjects = store.Subjects,
        Statistics = store.Attendance.GetStatistics()
    });

    [Authorize(Roles = "Admin,Teacher")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddAttendance(int studentId, string subject, DateTime date, bool present)
    {
        var student = store.Students.FirstOrDefault(item => item.StudentId == studentId);
        if (student is null || string.IsNullOrWhiteSpace(subject))
        {
            TempData["Error"] = "Укажите студента и предмет.";
        }
        else
        {
            store.Attendance.Add(student.StudentId, student.GetFullName(), subject, date, present);
            TempData["Notice"] = "Посещаемость отмечена.";
        }

        return RedirectToAction(nameof(Attendance));
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateAttendance(int id, bool present)
    {
        store.Attendance.Update(id, present);
        return RedirectToAction(nameof(Attendance));
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Admin() => View(new AdminPageViewModel
    {
        CollegeName = store.CollegeName,
        AcademicYear = store.AcademicYear,
        StudentsCount = store.Students.Count,
        TeachersCount = store.Teachers.Count,
        GroupsCount = store.Groups.Count,
        AccountsCount = store.Accounts.Count
    });

    [Authorize(Roles = "Admin,Teacher")]
    public IActionResult Report(string type)
    {
        var report = type switch
        {
            "students" => store.BuildStudentsReport(),
            "teachers" => store.BuildTeachersReport(),
            "attendance" => store.BuildAttendanceReport(),
            _ => null
        };

        return report is null ? NotFound() : Content(report, "text/plain; charset=utf-8");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateSettings(string collegeName, int academicYear)
    {
        try
        {
            store.UpdateSettings(collegeName, academicYear);
            TempData["Notice"] = "Настройки сохранены.";
        }
        catch (ArgumentException exception)
        {
            TempData["Error"] = exception.Message;
        }

        return RedirectToAction(nameof(Admin));
    }
}