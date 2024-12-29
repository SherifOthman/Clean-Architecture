using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesWithEvents;
public class CategoryEventListVm
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public ICollection<CategoryEventDto>? Events { get; set; }
}
