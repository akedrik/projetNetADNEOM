﻿using System.Threading.Tasks;

namespace NetCoreApp.Core.Interfaces.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
