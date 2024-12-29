using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Orders.Queries.GetOrdersForMonth;
public class GetOrdersForMonthQueryHandler : IRequestHandler<GetOrdersForMonthQuery, PagedOrdersForMonthVm>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;

    public GetOrdersForMonthQueryHandler(IMapper mapper, IOrderRepository orderRepository)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
    }
    public async Task<PagedOrdersForMonthVm> Handle(GetOrdersForMonthQuery request, CancellationToken cancellationToken)
    {
        var list = await _orderRepository.GetPagedOrdersForMonth(request.Date,
            request.Page, request.Size);

        var orders = _mapper.Map<List<OrdersForMonthDto>>(list);

        var count = await _orderRepository.GetTotalCountOfOrdersForMonth(request.Date);

        return new PagedOrdersForMonthVm()
        {
            Count = count,
            Page = request.Page,
            Size = request.Size,
            OrdersForMonth = orders
        };
    }
}
