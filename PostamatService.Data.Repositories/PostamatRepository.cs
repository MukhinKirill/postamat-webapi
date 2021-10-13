using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PostamatService.Data.Models;

namespace PostamatService.Data.Repositories
{
    public sealed class PostamatRepository : RepositoryBase<Postamat>, IPostamatRepository
    {
        public PostamatRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<IEnumerable<Postamat>> GetAll(bool trackChanges) =>
            await FindAll(trackChanges)
                .ToListAsync();

        public async Task<IEnumerable<Postamat>> GetActive(bool trackChanges) =>
            await FindByCondition(_=>_.IsActive,trackChanges)
                .OrderBy(_=>_.Number)
                .ToListAsync();

        public async Task<Postamat> Get(string number, bool trackChanges) =>
            await FindByCondition(_ => _.Number == number, trackChanges)
                .SingleOrDefaultAsync();

        public async Task SaveAsync() => await RepositoryContext.SaveChangesAsync();
    }
}
