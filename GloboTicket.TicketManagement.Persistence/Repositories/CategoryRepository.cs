using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Persistence.Repositories;
public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(GloboTicketDbContext dbContext) : base(dbContext)
    {

    }
    public Task<List<Category>> GetCategoriesWithEvents(bool includeHistory)
    {
        var query = _dbContext.Categories.AsQueryable();

        if (includeHistory)
        {
            // Include all events
            query = query.Include(c => c.Events);
        }
        else
        {
            // Include only events that are in the future or today
            query = query.Include(c => c.Events!.Where(e => e.Date >= DateTime.Today));
        }

        // Execute the query and return the results
        return query.ToListAsync();

    }
}
