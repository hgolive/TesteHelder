using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WepApi.Models;

namespace WepApi.Controllers
{
    public class ModeloTesteController : ApiController
    {
        private TesteHelderEntities db = new TesteHelderEntities();

        // GET: api/ModeloTeste
        public IQueryable<usuario> Getusuario()
        {
            return db.usuario;
        }

        // GET: api/ModeloTeste/5
        [ResponseType(typeof(usuario))]
        public IHttpActionResult Getusuario(Guid id)
        {
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // PUT: api/ModeloTeste/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putusuario(Guid id, usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.ID)
            {
                return BadRequest();
            }

            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!usuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ModeloTeste
        [ResponseType(typeof(usuario))]
        public IHttpActionResult Postusuario(usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.usuario.Add(usuario);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (usuarioExists(usuario.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = usuario.ID }, usuario);
        }

        // DELETE: api/ModeloTeste/5
        [ResponseType(typeof(usuario))]
        public IHttpActionResult Deleteusuario(Guid id)
        {
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.usuario.Remove(usuario);
            db.SaveChanges();

            return Ok(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool usuarioExists(Guid id)
        {
            return db.usuario.Count(e => e.ID == id) > 0;
        }
    }
}