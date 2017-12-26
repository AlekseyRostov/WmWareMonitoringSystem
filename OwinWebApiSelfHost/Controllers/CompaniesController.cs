using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using OwinWebApiSelfHost.Model;

namespace OwinWebApiSelfHost.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompaniesController: ApiController
    {
        ApplicationDbContext _db
        {
            get
            {
                return Request.GetOwinContext().Get<ApplicationDbContext>();
            }
        }

        public IEnumerable<Company> Get()
        {
            return _db.Companies;
        }

        public async Task<Company> Get(int id)
        {
            var company = await _db.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return company;
        }

        public async Task<IHttpActionResult> Post(Company company)
        {
            if (company == null)
            {
                return BadRequest("Argument Null");
            }
            var companyExists = await _db.Companies.AnyAsync(c => c.Id == company.Id);

            if (companyExists)
            {
                return BadRequest("Exists");
            }

            _db.Companies.Add(company);
            await _db.SaveChangesAsync();
            return Ok();
        }

        public async Task<IHttpActionResult> Put(Company company)
        {
            if (company == null)
            {
                return BadRequest("Argument Null");
            }
            var existing = await _db.Companies.FirstOrDefaultAsync(c => c.Id == company.Id);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = company.Name;
            await _db.SaveChangesAsync();
            return Ok();
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            var company = await _db.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            _db.Companies.Remove(company);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}