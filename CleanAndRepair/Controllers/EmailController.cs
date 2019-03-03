using ActionMailer.Net.Mvc;
using CleanAndRepair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CleanAndRepair.Controllers
{
    public class EmailController : MailerBase
    {
        // GET: Email
        public EmailResult SendEmail(EmailMessage model)
        {
            To.Add(model.To);

            From = model.From;

            Subject = model.Subject;

            return Email("SendEmail", model);
        }
    }
}