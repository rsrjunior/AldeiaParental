using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AldeiaParental.Data;
using AldeiaParental.Models;
using System.Text;

namespace AldeiaParental.Pages_CheckDocuments
{
    public class IndexModel : PageModel
    {
        private readonly AldeiaParental.Data.ApplicationDbContext _context;

        public IndexModel(AldeiaParental.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PersonalDocument> PersonalDocument { get; set; }

        public async Task OnGetAsync()
        {
            PersonalDocument = await _context.PersonalDocument
                .Include(p => p.User).ToListAsync();
        }
        public async Task<IActionResult> OnGetCsv()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Tipo,Número,Verificação,Usuário");
            List<PersonalDocument> _documents = await _context.PersonalDocument.ToListAsync<PersonalDocument>();
            foreach (var doc in _documents)
            {
                builder.Append($"{doc.DocumentType},{doc.DocumentNumber},");
                builder.Append($"{(doc.Valid==null?"Indefinido":doc.Valid==true?"Válido":"Inválido")},");
                builder.AppendLine($"{doc.UserId}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "userdocuments.csv");
        }
    }
}