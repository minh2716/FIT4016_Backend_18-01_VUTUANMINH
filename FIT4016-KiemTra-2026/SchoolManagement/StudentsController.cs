using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class StudentsController : Controller
{
    private readonly SchoolDbContext _context;

    public StudentsController(SchoolDbContext context)
    {
        _context = context;
    }

    // READ
    public IActionResult Index(int page = 1)
    {
        int pageSize = 10;
        var students = _context.Students
            .Include(s => s.School)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return View(students);
    }

    // CREATE
    public IActionResult Create()
    {
        ViewBag.Schools = _context.Schools.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Create(Student student)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Schools = _context.Schools.ToList();
            return View(student);
        }

        _context.Students.Add(student);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // UPDATE
    public IActionResult Edit(int id)
    {
        var student = _context.Students.Find(id);
        ViewBag.Schools = _context.Schools.ToList();
        return View(student);
    }

    [HttpPost]
    public IActionResult Edit(Student student)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Schools = _context.Schools.ToList();
            return View(student);
        }

        _context.Students.Update(student);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // DELETE
    public IActionResult Delete(int id)
    {
        var student = _context.Students.Find(id);
        return View(student);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult ConfirmDelete(int id)
    {
        var student = _context.Students.Find(id);
        _context.Students.Remove(student);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
