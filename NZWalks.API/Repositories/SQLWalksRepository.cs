using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Linq;

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

        public async Task<List<Walks>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
             string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false) 
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(X => X.Name.Contains(filterQuery));
                }
            }

            //sorting
            if (string.IsNullOrWhiteSpace(sortBy)== false)
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(X => X.Name) : walks.OrderByDescending(X => X.Name);
                }
                else if(sortBy.Equals("Length",StringComparison.OrdinalIgnoreCase))
                { 
                    walks = isAscending ? walks.OrderBy(X => X.LengthInKm) : walks.OrderByDescending(X => X.LengthInKm);
                }

            }
            //pagination
            var skipResult = (pageNumber - 1) * pageSize;
            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
          // return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
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
