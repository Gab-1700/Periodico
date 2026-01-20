using Periodico.Models;

namespace Periodico.ViewModels
{
    public class PortadaVM
    {
        public Noticia? Principal { get; set; }   // SOLO 1

        public List<Noticia> Nacionales { get; set; }
        public List<Noticia> Internacionales { get; set; }
        public List<Noticia> Economia { get; set; }
        public List<Noticia> Deportes { get; set; }
    }
}
