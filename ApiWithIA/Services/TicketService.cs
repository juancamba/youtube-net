using ApiWithIA.Data;
using ApiWithIA.Models;
using Microsoft.EntityFrameworkCore;
namespace ApiWithIA.Services;
public class TicketService
{
    private readonly AppDbContext _db;

    public TicketService(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Ticket ticket)
    {
        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync();
    }
    public async Task<List<Ticket>> GetTicketsAsync(string categoria, DateTime fechaInicio, DateTime fechaFin)
    {
        var query = _db.Tickets.AsQueryable();

        if(!string.IsNullOrEmpty(categoria))
            query = query.Where(t => t.Categoria.ToLower() == categoria.ToLower());

        query = query.Where(t => t.Fecha >= fechaInicio && t.Fecha <= fechaFin);

        return await query.ToListAsync();
    }
}