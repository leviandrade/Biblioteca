using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Controllers
{
    public class EditorasController : Controller
    {
        // GET: Editoras
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lista()
        {
            var lista = CriarListaEditoras();
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


        public ActionResult Salvar(Models.Editoras Editora)
        {
            _Salvar(Editora);

            return Json(new { Sucesso = true });
        }

        private List<Models.Editoras> CriarListaEditoras()
        {
            //var lista = Session["Empregados"] as List<Models.Empregados>;
            string stringConexao = @"Data Source=DESKTOP-DL249A7\SQLSERVER14;Initial Catalog=Biblioteca;user Id=sa;Password=24052716";
            string sql = "Select id,Nome From Editoras";
            List<Models.Editoras> lista = new List<Models.Editoras>();
            using (var conn = new SqlConnection(stringConexao))
            {
                var cmd = new SqlCommand(sql, conn);
                Models.Editoras p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            p = new Models.Editoras();
                            p.id = (int)reader["id"];
                            p.Nome = reader["Nome"].ToString();
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
                lista = new List<Models.Editoras>();

            return lista
                .OrderBy(item => item.id)
                .ToList();
        }

        private Models.Editoras Selecionar(int? id)
        {
            if (!id.HasValue)
                return new Models.Editoras();

            var lista = CriarListaEditoras();
            var entidade = lista.Where(item => item.id == id.Value)
                .FirstOrDefault();
            return entidade;
        }

        private List<Models.Editoras> _Excluir(int id)
        {
            string stringConexao = @"Data Source=DESKTOP-DL249A7\SQLSERVER14;Initial Catalog=Biblioteca;user Id=sa;Password=24052716";
            using (var conn = new SqlConnection(stringConexao))
            {
                string sql = "Delete Editoras where id=@id";
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

            var lista = CriarListaEditoras();

            return lista;
        }

        private void _Salvar(Models.Editoras entidade)
        {
            string stringConexao = @"Data Source=DESKTOP-DL249A7\SQLSERVER14;Initial Catalog=Biblioteca;user Id=sa;Password=24052716";
            using (var conn = new SqlConnection(stringConexao))
            {
                var lista = CriarListaEditoras();
                if (entidade.id > 0)
                    lista = _Excluir(entidade.id);
                else
                    entidade.id = lista.Count == 0
                        ? 1
                        : lista.OrderByDescending(item => item.id).FirstOrDefault().id + 1;
                string sql = "INSERT INTO Editoras (id,Nome) VALUES (@id,@Nome)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", entidade.id);
                cmd.Parameters.AddWithValue("@Nome", entidade.Nome);
                //cmd.Parameters.AddWithValue("@Tipo", entidade.);
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