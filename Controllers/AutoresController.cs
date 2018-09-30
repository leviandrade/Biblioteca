using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Biblioteca.Models;


namespace Biblioteca.Controllers
{
    public class AutoresController : Controller
    {
        // GET: Autores
        public ActionResult Index()
        {
            var b = recursividade(10);
            List<Autores> fundList = CriarListaAutores().ToList();
            ViewBag.Funds = new SelectList(fundList,"id","Nome");
            return View();
        }

        public ActionResult Lista()
        {
            var lista = CriarListaAutores();
            DateTime teste = DateTime.Now;
            var b = teste.ToString("MM/dd/yyyy");
            var c = String.Format("{0:MM/dd/yyyy}", teste);   
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult teste()
        //{
        //    string stringConexao = @"Data Source=DESKTOP-DL249A7\SQLSERVER14;Initial Catalog=Biblioteca;user Id=sa;Password=24052716";
        //    string sql = "Select id,Nome,Sobrenome,DataNascimento From Autores";
        //    List<Models.Autores> lista = new List<Models.Autores>();
        //    using (var conn = new SqlConnection(stringConexao))
        //    {
        //        var cmd = new SqlCommand(sql, conn);
        //        Models.Autores p = null;
        //        try
        //        {
        //            conn.Open();
        //            using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        //            {
        //                while (reader.Read())
        //                {
        //                    p = new Models.Autores();
        //                    p.id = (int)reader["id"];
        //                    p.Nome = reader["Nome"].ToString();
        //                    p.Sobrenome = reader["Sobrenome"].ToString();
        //                    p.DataNascimento = (DateTime)reader["DataNascimento"];
        //                    lista.Add(p);
        //                }
        //            }
        //        }

        //        catch (Exception e)
        //        {
        //            throw e;
        //        }
        //    }


        //    return Json(lista);
        //}

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


        public ActionResult Salvar(Models.Autores Autor)
        {
            _Salvar(Autor);

            return Json(new { Sucesso = true });
        }

        public ActionResult EntrarSistema(Models.Usuarios Usuarios)
        {
            var lista = ListaUsuarios();
            lista = lista.Where(x => x.Nome == Usuarios.Nome && x.Senha == Usuarios.Senha).ToList();
            if (lista.Count > 0)
                return View();
            else
                return Json(new { Sucesso = false });
        }

        public List<Models.Usuarios> ListaUsuarios()
        {
            string stringConexao = @"Data Source=den1.mssql1.gear.host;Initial Catalog=biblioteca4;user Id=biblioteca4;Password=Ga6g!w8m88_L	";
            string sql = "Select id,Nome,Senha From Usuarios";
            List<Models.Usuarios> lista = new List<Models.Usuarios>();
            using (var conn = new SqlConnection(stringConexao))
            {
                var cmd = new SqlCommand(sql, conn);
                Models.Usuarios p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            p = new Models.Usuarios();
                            p.id = (int)reader["id"];
                            p.Nome = reader["Nome"].ToString();
                            p.Senha = reader["Senha"].ToString();
                            lista.Add(p);
                        }
                    }
                }

                catch (Exception e)
                {
                    throw e;
                }

                if (lista == null)
                    lista = new List<Models.Usuarios>();

                return  lista
                    .OrderBy(item => item.id)
                    .ToList();

                
            }
        }

        private List<Models.Autores> CriarListaAutores()
        {
            //var lista = Session["Empregados"] as List<Models.Empregados>;
            string stringConexao = @"@"Data Source=den1.mssql1.gear.host;Initial Catalog=biblioteca4;user Id=biblioteca4;Password=Ga6g!w8m88_L";
            string sql = "Select id,Nome,Sobrenome,DataNascimento From Autores";
            List<Models.Autores> lista = new List<Models.Autores>();
            using (var conn = new SqlConnection(stringConexao))
            {
                var cmd = new SqlCommand(sql, conn);
                Models.Autores p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            p = new Models.Autores();
                            p.id = (int)reader["id"];
                            p.Nome = reader["Nome"].ToString();
                            p.Sobrenome = reader["Sobrenome"].ToString();
                            p.DataNascimento = (DateTime)reader["DataNascimento"];
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
                lista = new List<Models.Autores>();

            return lista
                .OrderBy(item => item.id)
                .ToList();
        }

        private Models.Autores Selecionar(int? id)
        {
            if (!id.HasValue)
                return new Models.Autores();

            var lista = CriarListaAutores();
            var entidade = lista.Where(item => item.id == id.Value)
                .FirstOrDefault();
            return entidade;
        }

        private List<Models.Autores> _Excluir(int id)
        {
            string stringConexao = @"Data Source=DESKTOP-DL249A7\SQLSERVER14;Initial Catalog=Biblioteca;user Id=sa;Password=24052716";
            using (var conn = new SqlConnection(stringConexao))
            {
                string sql = "Delete Autores where id=@id";
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

            var lista = CriarListaAutores();

            return lista;
        }

        private void _Salvar(Models.Autores entidade)
        {
            string stringConexao = @"Data Source=DESKTOP-DL249A7\SQLSERVER14;Initial Catalog=Biblioteca;user Id=sa;Password=24052716";
            using (var conn = new SqlConnection(stringConexao))
            {
                var lista = CriarListaAutores();
                if (entidade.id > 0)
                    lista = _Excluir(entidade.id);
                else
                    entidade.id = lista.Count == 0
                        ? 1
                        : lista.OrderByDescending(item => item.id).FirstOrDefault().id + 1;
                string sql = "INSERT INTO Autores (id,Nome,Sobrenome,DataNascimento) VALUES (@id,@Nome,@Sobrenome,@DataNascimento)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", entidade.id);
                cmd.Parameters.AddWithValue("@Nome", entidade.Nome);
                cmd.Parameters.AddWithValue("@Sobrenome", entidade.Sobrenome);
                cmd.Parameters.AddWithValue("@DataNascimento", entidade.DataNascimento);
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

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult teste(DateTime? datateste)
        { 
            var b= 0;
            return View();
        }
        int a = 0;
        int recursividade(int num) //declaras a função que recebe um valor inteiro
        {
            if (num < 0) // Se o número é menor que zero então retornar erro.
                return (a);

            if ((num == 0) || (num == 1)) // Se o numero é igual a 0 ou 1, então o seu factorial é 1.
                return (a);
            else      // Caso contrário, invocar recursivamente a função.
            {
                a++;
                return recursividade(num - 1); //invoca a função recursividade dizendo que a variavel "num" passa a ser num-1
            }
            var b = num;
        }
    }
}
