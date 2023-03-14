using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Linq;

namespace ContosoUniversity.Pages.Students
{
    public class ExperimentModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;
        private readonly IConfiguration Configuration;

        public ExperimentModel(SchoolContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }
        public string DataSort { get; set; }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public int[] SelectionID { get; set; }
        public List<Student> StudentsSelected = new List<Student> { };
        public List<Student> Students { get; set; } = default!;

        /* public async Task OnGetAsync()
         {
             if (_context.Student != null)
             {
                 Student = await _context.Student.ToListAsync();
             }
         }*/
        /* public async Task */
        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex, string DataSort)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            /*// using System;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
*/
            CurrentFilter = searchString;
            IQueryable<Student> studentsIQ = from s in _context.Students
                                             select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                studentsIQ = studentsIQ.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(DataSort))
            {
                studentsIQ = studentsIQ.Where(s => s.LastName.StartsWith(DataSort)
                                       || s.FirstMidName.StartsWith(DataSort));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentsIQ = studentsIQ.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.LastName);
                    break;
            }
            Students = await studentsIQ.AsNoTracking().ToListAsync();
            var pageSize = Configuration.GetValue("PageSize", 4);
            //Students=await _context.Students.ToListAsync();
            /*Students = await PaginatedList<Student>.CreateAsync(
                studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);*/
        }
        public async Task OnPostAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex, string DataSort, int[]? SelectedStudents)
        {
            SelectionID = SelectedStudents;
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            /*// using System;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
*/
            CurrentFilter = searchString;
            IQueryable<Student> studentsIQ = from s in _context.Students
                                             select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                studentsIQ = studentsIQ.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(DataSort))
            {
                studentsIQ = studentsIQ.Where(s => s.LastName.StartsWith(DataSort)
                                       || s.FirstMidName.StartsWith(DataSort));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentsIQ = studentsIQ.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.LastName);
                    break;
            }
            foreach (var item in SelectedStudents)
            {
                StudentsSelected.Add(_context.Students.Find(item));
            }
            Students = await studentsIQ.AsNoTracking().ToListAsync();
            var pageSize = Configuration.GetValue("PageSize", 4);
            //Students=await _context.Students.ToListAsync();
            /*Students = await PaginatedList<Student>.CreateAsync(
                studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);*/
        }
    }
}
