using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianBooking.Services.Interface
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string name, string toEmail, string message);
    }
}
