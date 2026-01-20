using Periodico.Models;

namespace Periodico.ViewModels
{
    public class NoticiaVM
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public DateOnly Fecha { get; set; }
        public IFormFile Imagen { get; set; }
        public TipoNoticia Tipo { get; set; }
    }
}
