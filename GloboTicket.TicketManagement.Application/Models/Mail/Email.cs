﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Models.Mail;
public class Email
{
    public required string To { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
}
