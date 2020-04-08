using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.EmailSender;
using NetCoreApp.Core.Interfaces.Logging;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NetCoreApp.Test.Tests_Intégrations.EmailSender
{
    public class EmailSenderTest
    {
        private readonly IEmailSender _emailSender;
        private readonly EmailSettings _emailSettings;

        public EmailSenderTest()
        {
            _emailSettings = new EmailSettings
            {
                MailPort = 465,
                MailServer = "smtp.ccp.ci",
                Password = "CCP@2018",
                Sender = "support.dasp@ccp.ci",
                SenderName = "NetCore App"
            };
            var mockIHostingEnvironment = new Mock<IHostingEnvironment>();
            mockIHostingEnvironment.Setup(m => m.EnvironmentName)
                .Returns("Development");

            var mockIOptionsEmailSettings = new Mock<IOptions<EmailSettings>>();
            mockIOptionsEmailSettings.Setup(m => m.Value)
                .Returns(_emailSettings);

            var mockIAppLogger = new Mock<IAppLogger<NetCoreApp.Infrastructure.EmailSender.EmailSender>>();

            _emailSender = new NetCoreApp.Infrastructure.EmailSender.EmailSender(mockIOptionsEmailSettings.Object,
                mockIHostingEnvironment.Object, mockIAppLogger.Object);
        }

        [Fact]
        public async Task Test_SendEmailAsync_Ne_Retourne_Pas_D_Erreur()
        {
            var email = "akcedrik@yahoo.fr";
            var subject = "Test Email";
            var message = "Email de test pour s'assurer du bon fonctionnement de la méthode SendEmailAsync";

            await _emailSender.SendEmailAsync(email, subject, message);
            //await Assert.ThrowsAsync<InvalidOperationException>(() => );
        }
    }
}
