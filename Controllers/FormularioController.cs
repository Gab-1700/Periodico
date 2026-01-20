using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Periodico.Controllers.Models;
using Periodico.Data;
using Periodico.Models;
using Periodico.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace Periodico.Controllers;

[Authorize]
public class FormularioController : Controller
{
    private readonly ILogger<NoticiaController> _logger;
    private PeriodicoContext _context;
    public FormularioController(PeriodicoContext context, ILogger<NoticiaController> logger)
    {
        _context = context;
        _logger = logger;
    }

    //metodo ...<formulario> que crea
    public IActionResult Index()
    {
        return View(new Noticia { Fecha = DateOnly.FromDateTime(DateTime.Now) });
    }
    //hacer cambios a bloquear
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var noticia = await _context.Noticias.FirstOrDefaultAsync(n => n.Id == id);
        return View("/Views/Formulario/Index.cshtml", noticia);
    }


    [HttpPost]
    public IActionResult Previsualizar(Noticia noticia)
    {
        return View(noticia);
    }

    //bloquear
    [HttpPost]
    public async Task<IActionResult> Guardar(NoticiaVM noticiaVM)
    {
        var noticia = new Noticia
        {
            Titulo = noticiaVM.Titulo,
            Contenido = noticiaVM.Contenido,
            Fecha = noticiaVM.Fecha,
            Tipo = noticiaVM.Tipo,
            Imagen = await GuardarArchivoAsync(noticiaVM.Imagen) ?? ""
        };

        if (noticiaVM.Id > 0)
        {
            noticia.Id = noticiaVM.Id;
            _context.Noticias.Update(noticia);
        }
        else
        {
            _context.Noticias.Add(noticia);
        }

        await _context.SaveChangesAsync();

        return View("/Views/Noticia/Mostrar.cshtml", noticia);
    }


    private async Task<string> GuardarArchivoAsync(IFormFile imagen)
    {
        if (imagen == null || imagen.Length == 0)
            return null; // No se subió ninguna imagen

        // Crear un nombre único para el archivo
        string nombreArchivo = Guid.NewGuid().ToString()
                              + Path.GetExtension(imagen.FileName);

        // Ruta física dentro de wwwroot/imagenes
        string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes");
        string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

        // Guardar el archivo en disco
        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        {
            await imagen.CopyToAsync(stream);
        }

        // Devolver la ruta relativa para la base de datos
        return "/imagenes/" + nombreArchivo;
    }

}
