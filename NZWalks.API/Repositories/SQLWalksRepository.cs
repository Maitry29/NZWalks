using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalksRepository : IWalksRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalksRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walks> CreateAsync(Walks walks)
        {
            
            await dbContext.Walks.AddAsync(walks);
            await dbContext.SaveChangesAsync();
            return walks;
        }

        public async Task<Walks?> DeleteAsync(Guid id)
        {
            var existingWalk =  await dbContext.Walks.FirstOrDefaultAsync(x=>x.ID == id);
            if(existingWalk == null)
            {
                return null;
            }
            dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walks>> GetAllAsync()
        {
           return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walks?> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(X => X.ID == id);
        }

        public async Task<Walks?> UpdateAsync(Guid id, Walks walks)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(X => X.ID == id);
            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walks.Name;
            existingWalk.Description = walks.Description;
            existingWalk.LengthInKm = walks.LengthInKm;
            existingWalk.RegionID = walks.RegionID;
            existingWalk.DifficultyID = walks.DifficultyID;

            await dbContext.SaveChangesAsync();
            return existingWalk;


        }
    }
}
