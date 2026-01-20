using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Periodico.Data;

namespace Periodico.ViewComponents
{
    public class TiposNoticiasViewComponent : ViewComponent
    {
        private readonly PeriodicoContext _context;
        public TiposNoticiasViewComponent(PeriodicoContext context) {
            _context = context;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tipos = await _context.Noticias
                .Select(n => n.Tipo)
                .Distinct()
                .OrderBy(c=>c)
                .ToListAsync();

            return View(tipos);
        }
    }
}
