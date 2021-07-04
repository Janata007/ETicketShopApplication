using ETicket.Domain.DomainModels;
using ETicket.Repository.Interface;
using ETicket.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicket.Services.Implementation
{
    public class BackgroundEmailSender : IBackgroundEmailSender
    {

        private readonly IEmailService _emailService;
        private readonly IRepository<EmailMessage> _mailRepository;

        public BackgroundEmailSender(IEmailService emailService, IRepository<EmailMessage> mailRepository) 
        {
            this._emailService = emailService;
            this._mailRepository = mailRepository;

        }

        public async Task DoWork()
        {
            await _emailService.SendEmailAsync(_mailRepository.GetAll().Where(z => !z.Status).ToList());
        }
    }
}
