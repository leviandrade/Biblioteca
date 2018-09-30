using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biblioteca.Models
{
    public class Livros
    {
        public int id { get; set; }
        public string Nome { get; set; }
        public string ISBN { get; set; }
        public DateTime DataPublicacao { get; set; }
        public double Preco { get; set; }
        public int idEditora { get; set; }
        public int idAutor { get; set; }
    }
}