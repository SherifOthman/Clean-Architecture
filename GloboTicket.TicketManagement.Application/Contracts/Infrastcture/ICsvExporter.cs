
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventExport;

namespace GloboTicket.TicketManagement.Application.Contracts.Infrastcture;
public interface ICsvExporter 
{
    byte[] ExportEventsToCsv(List<EventExportDto> eventExportDtos);
}
