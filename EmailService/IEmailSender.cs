using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApp.EmailService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
}