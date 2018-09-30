using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Controllers
{
    public class LivrosController : Controller
    {
        // GET: Livros
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista()
        {
            var lista = CriarListaLivros();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Editar(int? id)
        {
            var entidade = Selecionar(id);

            return Json(entidade, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Excluir(int id)
        {
            _Excluir(id);

            return Json(new { Sucesso = true });
        }


        public ActionResult Salvar(Models.Livros Livro)
        {
            _Salvar(Livro);

            return Json(new { Sucesso = true });
        }

        private List<Models.Livros> CriarListaLivros()
        {
            //var lista = Session["Empregados"] as List<Models.Empregados>;
            string stringConexao = @"Data Source=DESKTOP-DL249A7\SQLSERVER14;Initial Catalog=Biblioteca;user Id=sa;Password=24052716";
            string sql = "Select id,Nome,ISBN,DataPublicacao,Preco,idEditoras,idAutores From Livros";
            List<Models.Livros> lista = new List<Models.Livros>();
            using (var conn = new SqlConnection(stringConexao))
            {
                var cmd = new SqlCommand(sql, conn);
                Models.Livros p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            p = new Models.Livros();
                            p.id = (int)reader["id"];
                            p.Nome = reader["Nome"].ToString();
                            p.ISBN = reader["ISBN"].ToString();
                            p.idEditora = (int)reader["idEditoras"];
                            p.Preco = (double)reader["Preco"];
                            p.idAutor = (int)reader["idAutores"];
                            lista.Add(p);
                        }
                    }
                }

                catch (Exception e)
                {
                    throw e;
                }
            }
            if (lista == null)
                lista = new List<Models.Livros>();

            return lista
                .OrderBy(item => item.id)
                .ToList();
        }

        private Models.Livros Selecionar(int? id)
        {
            if (!id.HasValue)
                return new Models.Livros();

            var lista = CriarListaLivros();
            var entidade = lista.Where(item => item.id == id.Value)
                .FirstOrDefault();
            return entidade;
        }

        private List<Models.Livros> _Excluir(int id)
        {
            string stringConexao = @"Data Source=DESKTOP-DL249A7\SQLSERVER14;Initial Catalog=Biblioteca;user Id=sa;Password=24052716";
            using (var conn = new SqlConnection(stringConexao))
            {
                string sql = "Delete Livros where id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            var lista = CriarListaLivros();

            return lista;
        }

        private void _Salvar(Models.Livros entidade)
        {
            string stringConexao = @"Data Source=DESKTOP-DL249A7\SQLSERVER14;Initial Catalog=Biblioteca;user Id=sa;Password=24052716";
            using (var conn = new SqlConnection(stringConexao))
            {
                var lista = CriarListaLivros();
                if (entidade.id > 0)
                    lista = _Excluir(entidade.id);
                else
                    entidade.id = lista.Count == 0
                        ? 1
                        : lista.OrderByDescending(item => item.id).FirstOrDefault().id + 1;
                string sql = "INSERT INTO Livros (id,Nome,ISBN,DataPublicacao,Preco,idEditoras,idAutores) VALUES (@id,@Nome,@ISBN,@DataPublicacao,@Preco,@idEditoras,@idAutores)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", entidade.id);
                cmd.Parameters.AddWithValue("@Nome", entidade.Nome);
                cmd.Parameters.AddWithValue("@ISBN", entidade.ISBN);
                cmd.Parameters.AddWithValue("@DataPublicacao", entidade.DataPublicacao);
                cmd.Parameters.AddWithValue("@Preco", entidade.Preco);
                cmd.Parameters.AddWithValue("@idEditoras", entidade.idEditora);
                cmd.Parameters.AddWithValue("@idAutores", entidade.idAutor);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }

            }

        }

    }
}