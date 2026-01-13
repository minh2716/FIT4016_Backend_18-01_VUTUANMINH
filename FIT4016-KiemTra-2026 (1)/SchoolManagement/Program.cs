using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagement
{
    internal class Program
    {
        private const int PageSize = 10;

        private static void Main()
        {
            using var db = new SchoolDbContext();

            // Tạo database + bảng nếu chưa có (SQLite)
            db.Database.EnsureCreated();
            SeedSampleData(db);

            while (true)
            {
                Console.WriteLine("=== STUDENT MANAGEMENT ===");
                Console.WriteLine("1. Create student");
                Console.WriteLine("2. List students");
                Console.WriteLine("3. Update student");
                Console.WriteLine("4. Delete student");
                Console.WriteLine("0. Exit");
                Console.Write("Choose option: ");
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            CreateStudent(db);
                            break;
                        case "2":
                            ListStudents(db);
                            break;
                        case "3":
                            UpdateStudent(db);
                            break;
                        case "4":
                            DeleteStudent(db);
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine();
            }
        }

        private static void SeedSampleData(SchoolDbContext db)
        {
            if (!db.Schools.Any())
            {
                var schools = new List<School>();
                for (var i = 1; i <= 10; i++)
                {
                    schools.Add(new School
                    {
                        Name = $"School {i}",
                        Principal = $"Principal {i}",
                        Address = $"Address {i}"
                    });
                }

                db.Schools.AddRange(schools);
                db.SaveChanges();
            }

            if (!db.Students.Any())
            {
                var schoolIds = db.Schools.Select(s => s.Id).ToList();
                var firstSchoolId = schoolIds.First();

                for (var i = 1; i <= 20; i++)
                {
                    db.Students.Add(new Student
                    {
                        SchoolId = firstSchoolId,
                        FullName = $"Student {i}",
                        StudentId = $"STU{i:00000}",
                        Email = $"student{i}@example.com",
                        Phone = "0987654321"
                    });
                }

                db.SaveChanges();
            }
        }

        private static void CreateStudent(SchoolDbContext db)
        {
            Console.WriteLine("=== CREATE STUDENT ===");
            var student = new Student();

            Console.Write("Full name: ");
            student.FullName = Console.ReadLine() ?? string.Empty;

            Console.Write("Student ID (5-20 chars, unique): ");
            student.StudentId = Console.ReadLine() ?? string.Empty;

            Console.Write("Email (unique): ");
            student.Email = Console.ReadLine() ?? string.Empty;

            Console.Write("Phone (optional 10-11 digits): ");
            student.Phone = Console.ReadLine();

            Console.Write("School ID: ");
            if (!int.TryParse(Console.ReadLine(), out var schoolId))
            {
                Console.WriteLine("School ID must be a number.");
                return;
            }

            if (!db.Schools.Any(s => s.Id == schoolId))
            {
                Console.WriteLine("School does not exist.");
                return;
            }

            student.SchoolId = schoolId;

            var results = ValidateEntity(student);
            if (results.Any())
            {
                foreach (var error in results)
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return;
            }

            db.Students.Add(student);
            db.SaveChanges();
            Console.WriteLine("Student created successfully.");
        }

        private static void ListStudents(SchoolDbContext db)
        {
            Console.Write("Enter page number (starting from 1): ");
            if (!int.TryParse(Console.ReadLine(), out var page) || page < 1)
            {
                Console.WriteLine("Invalid page.");
                return;
            }

            var query = db.Students
                .Include(s => s.School)
                .OrderBy(s => s.Id);

            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)PageSize);

            var students = query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            Console.WriteLine($"Page {page}/{totalPages} - Total {total} students");
            Console.WriteLine("Full name | Student ID | Email | Phone | School");
            foreach (var s in students)
            {
                Console.WriteLine(
                    $"{s.FullName} | {s.StudentId} | {s.Email} | {s.Phone} | {s.School?.Name}");
            }
        }

        private static void UpdateStudent(SchoolDbContext db)
        {
            Console.Write("Enter student Id to update (PK): ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid Id.");
                return;
            }

            var student = db.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.WriteLine($"Current full name: {student.FullName}");
            Console.Write("New full name (leave empty to keep): ");
            var fullName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(fullName))
                student.FullName = fullName;

            Console.WriteLine($"Current email: {student.Email}");
            Console.Write("New email (leave empty to keep): ");
            var email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
                student.Email = email;

            Console.WriteLine($"Current phone: {student.Phone}");
            Console.Write("New phone (leave empty to keep): ");
            var phone = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(phone))
                student.Phone = phone;

            Console.WriteLine($"Current school Id: {student.SchoolId}");
            Console.Write("New school Id (leave empty to keep): ");
            var schoolText = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(schoolText))
            {
                if (!int.TryParse(schoolText, out var newSchoolId) ||
                    !db.Schools.Any(s => s.Id == newSchoolId))
                {
                    Console.WriteLine("Invalid or non-existing school Id.");
                    return;
                }

                student.SchoolId = newSchoolId;
            }

            student.UpdatedAt = DateTime.UtcNow;

            var results = ValidateEntity(student);
            if (results.Any())
            {
                foreach (var error in results)
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return;
            }

            db.SaveChanges();
            Console.WriteLine("Student updated successfully.");
        }

        private static void DeleteStudent(SchoolDbContext db)
        {
            Console.Write("Enter student Id to delete: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid Id.");
                return;
            }

            var student = db.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.Write($"Are you sure to delete '{student.FullName}'? (y/n): ");
            var confirm = Console.ReadLine();
            if (!string.Equals(confirm, "y", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Delete canceled.");
                return;
            }

            db.Students.Remove(student);
            db.SaveChanges();
            Console.WriteLine("Student deleted successfully.");
        }

        private static List<ValidationResult> ValidateEntity(object entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(entity, context, results, true);
            return results;
        }
    }
}
