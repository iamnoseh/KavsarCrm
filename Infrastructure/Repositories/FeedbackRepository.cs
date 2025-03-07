using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Filters;

namespace Infrastructure.Repositories
{
    public class FeedbackRepository(DataContext context) : IFeedbackRepository
    {

        public async Task<List<Feedback>> GetAll(BaseFilter filter)
        {
            var res = await context.Feedbacks.Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize).ToListAsync();
            return res;
        }
        public async Task<Feedback> GetById(int id)
        {
            return await context.Feedbacks.FindAsync(id);
        }
        
        public async Task<int> Create(Feedback feedback)
        {
            context.Feedbacks.Add(feedback);
            return await context.SaveChangesAsync();
        }
        public async Task<int> Update(Feedback feedback)
        {
            context.Feedbacks.Update(feedback);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(Feedback feedback)
        {
            context.Feedbacks.Remove(feedback);
            return await context.SaveChangesAsync();
        }
    }
}