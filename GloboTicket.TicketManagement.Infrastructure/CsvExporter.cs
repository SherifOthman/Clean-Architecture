using CsvHelper;
using GloboTicket.TicketManagement.Application.Contracts.Infrastcture;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventExport;
using System.Globalization;


namespace GloboTicket.TicketManagement.Infrastructure;
public class CsvExporter : ICsvExporter
{
    public byte[] ExportEventsToCsv(List<EventExportDto> eventExportDtos)
    {
        using var memoryStream = new MemoryStream();
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.CurrentCulture);
            csvWriter.WriteRecords(eventExportDtos);
        }
        return memoryStream.ToArray();
    }
}
