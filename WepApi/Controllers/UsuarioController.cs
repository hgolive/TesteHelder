using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WepApi.Models;
using WepApi.Regras;

namespace WepApi.Controllers
{

    [EnableCors(origins: "*", methods: "*", headers: "*")]
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {

        private TesteHelderEntities db = new TesteHelderEntities();

        [AcceptVerbs("POST")]
        [Route("CadastrarUsuario")]
        public object CadastrarUsuario(Models.usuario usuario)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ValidarCPF regra = new ValidarCPF();

            if (regra.validar(usuario.cpf))
            {
                usuario.ID = Guid.NewGuid();
                db.usuario.Add(usuario);
                db.SaveChanges();
            }
            else
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                          Request.CreateErrorResponse((HttpStatusCode)400, new HttpError("CPF invalido!")));
            }

          
            return Json(new { Message = "Usuario Cadastrado com sucesso!"});

        }
        [AcceptVerbs("POST")]
        [Route("ConsultarUsuario")]
        public object ConsultarUsuario(usuario usuario)
        {
            List<usuario> lista = null;
            if (usuario != null)
            {
               
                if (usuario.nome == null)
                {
                    if (usuario.cpf == null)
                    {
                        return new System.Web.Http.Results.ResponseMessageResult(
                            Request.CreateErrorResponse((HttpStatusCode)400, new HttpError("Informe o nome ou CPF do usuário!")));
                    }
                    else
                    {
                        lista = db.usuario.Where(s => s.cpf == usuario.cpf).ToList();
                    }
                }
                else
                {


                    lista = db.usuario.Where(s => s.nome == usuario.nome).ToList();

                }
            }
            else
            {
                lista = db.usuario.ToList();
            }

           

            return Json(new { Message = "Lista Usuário", Lista = lista });

        }
        [AcceptVerbs("POST")]
        [Route("DeletarUsuario/{ID}")]
        public object DeletarUsuario(Guid ID)
        {
            usuario usuario = db.usuario.Find(ID);
            if (usuario == null)
            {
                return NotFound();
            }

            db.usuario.Remove(usuario);
            db.SaveChanges();


            return Json(new { Message = "Usuário Excluido " });

        }
        [AcceptVerbs("POST")]
        [Route("AtualizarUsuario")]
        public object AtualizarUsuario(usuario usuario)
        {



            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

       
            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
              
            }

            return Json(new { Message = "Lista ", Feiras = "" });

        }

        [AcceptVerbs("GET")]
        [Route("consultarID/{idUsuario}")]
        public object ConsultarUsuarioID(Guid idUsuario)
        {

           usuario usuario = db.usuario.Where(ss => ss.ID == idUsuario).FirstOrDefault();

            return Json(usuario);

        }
        private bool usuarioExists(Guid id)
        {
            return db.usuario.Count(e => e.ID == id) > 0;
        }

    }
}
