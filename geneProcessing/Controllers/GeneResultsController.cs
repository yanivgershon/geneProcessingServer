using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UploadFilesServer.Models;

namespace UploadFilesServer.Context
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneResultsController : ControllerBase
    {
        private readonly GenesParserContext _context;

        public GeneResultsController(GenesParserContext context)
        {
            _context = context;
        }

        // GET: api/GeneResults
        [HttpGet]
        /*
        public async Task<ActionResult<IEnumerable<GeneResult>>> GetGeneResults()
        {
            try
            {
                return await _context.GeneResults.ToListAsync();
            }
            catch(Exception ee)
            {

            }
        }
        */
        public List<GeneResult> GetGeneResults()
        {
            try
            {
                return _context.GeneResults.ToList();
            }
            catch (Exception ee)
            {
                List<GeneResult> geneResults = new List<GeneResult>();
                geneResults.Add( new GeneResult {GeneName=ee.Message,Text=ee.StackTrace });
                return geneResults;
            }
        }
        // GET: api/GeneResults/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GeneResult>> GetGeneResult(int id)
        {
            var geneResult = await _context.GeneResults.FindAsync(id);

            if (geneResult == null)
            {
                return NotFound();
            }

            return geneResult;
        }

        // PUT: api/GeneResults/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeneResult(int id, GeneResult geneResult)
        {
            if (id != geneResult.Id)
            {
                return BadRequest();
            }

            _context.Entry(geneResult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneResultExists(id))
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

        // POST: api/GeneResults
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GeneResult>> PostGeneResult(GeneResult geneResult)
        {
            _context.GeneResults.Add(geneResult);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeneResult", new { id = geneResult.Id }, geneResult);
        }

        // DELETE: api/GeneResults/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GeneResult>> DeleteGeneResult(int id)
        {
            var geneResult = await _context.GeneResults.FindAsync(id);
            if (geneResult == null)
            {
                return NotFound();
            }

            _context.GeneResults.Remove(geneResult);
            await _context.SaveChangesAsync();

            return geneResult;
        }

        private bool GeneResultExists(int id)
        {
            return _context.GeneResults.Any(e => e.Id == id);
        }
    }
}
