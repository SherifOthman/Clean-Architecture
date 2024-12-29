using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Persistence.Repositories;
public class EventRepository : BaseRepository<Event>, IEventRepository
{
    public EventRepository(GloboTicketDbContext dbContext) : base(dbContext)
    {

    }

    public Task<bool> IsEventNameAndDateUnique(string name, DateTime dateDate)
    {
        var matches = _dbContext.Events.AnyAsync(e =>
        e.Name.Equals(name)
        && e.Date.Equals(dateDate));

        return matches;
    }
}
