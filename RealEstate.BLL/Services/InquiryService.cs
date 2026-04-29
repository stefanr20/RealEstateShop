using Microsoft.EntityFrameworkCore;
using RealEstate.BLL.Interfaces;
using RealEstate.Data.Context;
using RealEstate.Data.UnitOfWork;
using RealEstate.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.BLL.Services
{
    public class InquiryService : IInquiryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RealEstateDbContext _context;

        public InquiryService(IUnitOfWork unitOfWork, RealEstateDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public IEnumerable<Inquiry> GetAll()
        {
            return _unitOfWork.Inquiries.GetAll();
        }

        public async Task Create(Inquiry inquiry)
        {
            _unitOfWork.Inquiries.Insert(inquiry);
            await _unitOfWork.CompleteAsync();
        }

        public IEnumerable<Inquiry> GetAllWithProperties()
        {
            return _context.Inquiries
                .Include(i => i.Property)
                .ToList();
        }
        public async Task Reply(int id, string reply)
        {
            var inquiry = _context.Inquiries.Find(id);
            if (inquiry == null) return;
            inquiry.AdminReply = reply;
            inquiry.RepliedAt = DateTime.Now;
            _context.Inquiries.Update(inquiry);
            await _context.SaveChangesAsync();
        }
    }
}