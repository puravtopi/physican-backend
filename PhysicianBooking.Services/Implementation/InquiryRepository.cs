using Dapper;
using PhysicianBooking.Domain.Modal;
using PhysicianBooking.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianBooking.Services.Implementation
{
    public class InquiryRepository : IInquiryRepository
    {
        private readonly IDbConnection _db;

        public InquiryRepository(IDbConnection db)
        {
            _db = db;
        }
        public async Task SaveInquiry(InquiryModal modal)
        {
            string sql = @"
    INSERT INTO tbl_inquiry (name, email, phone, note)
    SELECT @name, @email, @phone,@note";

            await _db.ExecuteAsync(sql, new
            {
                name = (string)modal.Name,
                email = (string)modal.Email,
                phone = (string)modal.Phone,
                note = (string)modal.Note
            });
        }
    }
}
