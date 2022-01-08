using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using TaskManagement.Models;

namespace TaskManagement.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using TaskManagement.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Quote>("Quotes");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    //44364 4200
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class QuotesController : ODataController
    {
        private TaskDatEntities db = new TaskDatEntities();

        // GET: odata/Quotes
        [EnableQuery]
        public IQueryable<Quote> GetQuotes()
        {
            return db.Quotes;
        }

        // GET: odata/Quotes(5)
        [EnableQuery]
        public SingleResult<Quote> GetQuote([FromODataUri] int key)
        {
            return SingleResult.Create(db.Quotes.Where(quote => quote.QuoteID == key));
        }

        // PUT: odata/Quotes(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Quote> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Quote quote = db.Quotes.Find(key);
            if (quote == null)
            {
                return NotFound();
            }

            patch.Put(quote);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuoteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(quote);
        }

        // POST: odata/Quotes
        public IHttpActionResult Post(Quote quote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Quotes.Add(quote);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (QuoteExists(quote.QuoteID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(quote);
        }

        // PATCH: odata/Quotes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Quote> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Quote quote = db.Quotes.Find(key);
            if (quote == null)
            {
                return NotFound();
            }

            patch.Patch(quote);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuoteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(quote);
        }

        private void Validate(object p)
        {
            throw new NotImplementedException();
        }

        // DELETE: odata/Quotes(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Quote quote = db.Quotes.Find(key);
            if (quote == null)
            {
                return NotFound();
            }

            db.Quotes.Remove(quote);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuoteExists(int key)
        {
            return db.Quotes.Count(e => e.QuoteID == key) > 0;
        }
    }
}
