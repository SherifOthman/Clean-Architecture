﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Orders.Queries.GetOrdersForMonth;
public class GetOrdersForMonthQuery : IRequest<PagedOrdersForMonthVm>
{
    public DateTime Date { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }

}
