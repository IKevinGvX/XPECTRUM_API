using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using xpectrum_api.data;
using xpectrum_api.models;

namespace Xpectrum_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ticketsController : ControllerBase
    {
        private readonly xpectrumContext _context;

        public ticketsController(xpectrumContext context)
        {
            _context = context;
        }

        // GET: api/tickets
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<ticket>>> Gettickets()
        {
            return await _context.tickets.ToListAsync();
        }

        // GET: api/tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ticket>> Getticket(int id)
        {
            var ticket = await _context.tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        // PUT: api/tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putticket(int id, ticket ticket)
        {
            if (id != ticket.ticketid)
            {
                return BadRequest();
            }

            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ticketExists(id))
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

        // POST: api/tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ticket>> Postticket(ticket ticket)
        {
            _context.tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getticket", new { id = ticket.ticketid }, ticket);
        }

        // DELETE: api/tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteticket(int id)
        {
            var ticket = await _context.tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ticketExists(int id)
        {
            return _context.tickets.Any(e => e.ticketid == id);
        }
    }
}
