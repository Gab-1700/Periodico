using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Periodico.Data;
using Periodico.Models;
using Periodico.ViewComponents;
using Periodico.ViewModels;
using System.Diagnostics;

namespace Periodico.Controllers.Models
{
    //[Route("Noticia")]
    public class NoticiaController : Controller
    {
        private PeriodicoContext _context;
        private readonly ILogger<NoticiaController> _logger;

        public NoticiaController(PeriodicoContext context, ILogger<NoticiaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("Noticia/Mostrar/{id?}")]
        public async Task<IActionResult> Mostrar(int? id)
        {
            _logger.LogInformation($"==>id:{id}");
            var noticia = await _context.Noticias.FirstOrDefaultAsync(n => n.Id == id);
            return View(noticia);
        }


        [HttpGet("Noticia")]
        public async Task<IActionResult> MostrarNoticias()
        {
            var noticias = await UltimasNoticias(50);
            return View("/Views/Noticia/Index.cshtml",noticias);
        }


        [HttpGet("Noticia/{tipoNoticia?}")]
        public async Task<IActionResult> MostrarNoticiasPorTipo(TipoNoticia? tipoNoticia)
        {
            if (tipoNoticia == null)
            {
                return View("/Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            var resultado=await GetNoticiasPorTipo(tipoNoticia.Value, 50);
            return View("Views/Noticia/Index.cshtml",resultado);
        }


        [HttpGet("Noticia/{tipoNoticia?}/{id?}")]
        public async Task<IActionResult> MostrarNoticiaPorTipo(TipoNoticia? tipoNoticia, int? id)
        {
            if (id == null)
            {
                return View("/Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            var resultado = await NoticiaPorId(id.Value);
            if(resultado!= null && resultado.Tipo==tipoNoticia) { 
                return View("Views/Shared/Noticia/_Previsualizar.cshtml",resultado);
            }
            else { return View("/Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); }
            
        }


        public async Task<IActionResult> MostrarPortada()
        {
            var model = new PortadaVM
            {
                Principal = await _context.Noticias
                    .AsNoTracking()
                    .Where(n => n.Tipo == TipoNoticia.Principal)
                    .OrderByDescending(n => n.Fecha)
                    .FirstOrDefaultAsync(),

                Nacionales = await GetNoticiasPorTipo(TipoNoticia.Nacional, 2),
                Internacionales = await GetNoticiasPorTipo(TipoNoticia.Internacional, 2),
                Economia = await GetNoticiasPorTipo(TipoNoticia.Economia, 5),
                Deportes = await GetNoticiasPorTipo(TipoNoticia.Deportes, 4)
            };
            return View("/Views/Noticia/NoticiaPortada.cshtml", model);
        }

        private async Task<List<Noticia>> GetNoticiasPorTipo(TipoNoticia tipo, int cantidad)
        {
            return await _context.Noticias
                .AsNoTracking()
                .Where(n => n.Tipo == tipo)
                .OrderByDescending(n => n.Fecha)
                .Take(cantidad)
                .ToListAsync();

        }

        private async Task<Noticia?> NoticiaPorId(int id)
        {
            return await _context.Noticias
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        private async Task<List<Noticia>> UltimasNoticias(int cantidad)
        {
            return await _context.Noticias
           .AsNoTracking()
           .OrderByDescending(n => n.Fecha)
           .Take(cantidad)
           .ToListAsync();
        }

    }
}
