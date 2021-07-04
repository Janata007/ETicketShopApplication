using ETicket.Domain.DomainModels;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ETicket.Services.Interface
{
    public interface IEmailService
    {

        Task SendEmailAsync(List<EmailMessage> allMails);

    }
}
