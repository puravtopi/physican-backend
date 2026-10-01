using PhysicianBooking.Domain.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianBooking.Services.Interface
{
    public interface IInquiryRepository
    {
        Task SaveInquiry(InquiryModal modal);
    }
}
