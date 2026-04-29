using RealEstate.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data.Context;

namespace RealEstate.BLL.Interfaces
{
    public interface IInquiryService
    {
        IEnumerable<Inquiry> GetAll();
        Task Create(Inquiry inquiry);
        IEnumerable<Inquiry> GetAllWithProperties();
        Task Reply(int id, string reply);

    }
}