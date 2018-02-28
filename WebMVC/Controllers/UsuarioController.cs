using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class UsuarioController : Controller
    {

        private string WebApi = "http://localhost:49479/api/";
    
        static HttpClient client = new HttpClient();

        private  string AddUsuario(usuario usuario)
        {
            var result = "";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebApi + "usuario/CadastrarUsuario");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";


            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(usuario);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                 result = streamReader.ReadToEnd();
            }

            return result.ToString();

        }

        private string EditUsuario(usuario usuario)
        {
            var result = "";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebApi + "usuario/AtualizarUsuario");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";


            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(usuario);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return result.ToString();

        }


        // GET: Usuario
        public ActionResult CadastroUsuario()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CadastroUsuario(usuario usuario)
        {
            if (validar(usuario.cpf))
            {
                AddUsuario(usuario);
                return RedirectToAction("ConsultaUsuario");
            }
            else
            {
                ViewBag.Alerta = "CPF invalido!";
                return View(); ;
            }

        }
        // GET: ModeloU/Edit/5
        public ActionResult EditarUsuario(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var json = new WebClient().DownloadString(WebApi + "Usuario/consultarID/" + id.ToString());

            usuario usuario = new JavaScriptSerializer().Deserialize<usuario>(json);

            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }


       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(usuario usuario)
        {
            if (validar(usuario.cpf))
            {
                EditUsuario(usuario);
                return RedirectToAction("ConsultaUsuario");
            }
            else
            {
                ViewBag.Alerta = "CPF invalido!";
                return View(); ;
            }
        }



     
        public ActionResult ConsultaUsuario()
        {
            return View();
        }

        public bool validar(string cpf)
        {

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}