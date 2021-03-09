using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrsServer.Data;
using PrsServer.Models;

namespace PrsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly PrsDbContext _context;

        public RequestsController(PrsDbContext context)
        {
            _context = context;
        }


        //PUT: api/Requests/REVIEW/5
        [HttpPut("Review/{id}")]
        public async Task<IActionResult> SetStatustoReview(int id, Request request) {
            //var request = await _context.Request.FindAsync(id);
            if(request == null) {
                return NotFound();
            }
            request.Status = (request.Total <= 50) ? "APPROVED" : "REVIEW";
            return await PutRequest(request.Id, request);
        }

        //PUT: api/Requests/APRROVE/5
        [HttpPut("Approve/{id}")]
        public async Task<IActionResult> SetStatustoApprove(int id, Request request) {
            if(request == null) {
                return NotFound();
            }
            request.Status = "APPROVED";
            return await PutRequest(request.Id, request);
        }

        //PUT: api/Requests/REJECT/5
        [HttpPut("Reject/{id}")]
        public async Task<IActionResult> SetStatustoReject(int id, Request request) {
            //var request = await _context.Request.FindAsync(id);
            if (request == null) {
                return NotFound();
            }
            request.Status = "REJECTED";
            //if(request.Status == "REJECTED") {
            //    request.RejectionReason.ToString();
            //}
            return await PutRequest(request.Id, request);
        }

        //GET: api/Requests/Review
        [HttpGet("Review/{id}")]
        public async Task<IEnumerable<Request>> GetRequestinReview (int id) {
            return await _context.Requests
                .Where(r => r.Status == "REVIEW" && r.UserId != id)
                                          .ToListAsync();
        }
        
        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequest()
        {
            return await _context.Requests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests
                .Include(rl => rl.RequestLines)
                .ThenInclude(p => p.Product)
                .ThenInclude(v => v.Vendor)
                .Include(u => u.User)
                .SingleOrDefaultAsync(r => r.Id == id)
                ;

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Requests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Request.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Request>> DeleteRequest(int id)
        {
            var request = await _context.Request.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Request.Remove(request);
            await _context.SaveChangesAsync();

            return request;
        }

        private bool RequestExists(int id)
        {
            return _context.Request.Any(e => e.Id == id);
        }
    }
}
