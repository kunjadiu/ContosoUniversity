using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public EditModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            //var student =  await _context.Student.FirstOrDefaultAsync(m => m.ID == id);
            var student = await _context.Student.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }
            Student = student;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var studentToUpdate = await _context.Student.FindAsync(id);
            if (studentToUpdate==null)
            {
                return NotFound();
            }

           // _context.Attach(Student).State = EntityState.Modified;
            if(await TryUpdateModelAsync<Student>(studentToUpdate,"student",
                s=>s.FirstMidName, s => s.LastName,s=>s.EnrollmentDate))
            { 
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            /*try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(Student.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }*/

            return Page();
        }

        private bool StudentExists(int id)
        {
          return _context.Student.Any(e => e.ID == id);
        }
    }
}
