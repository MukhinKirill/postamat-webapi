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

        public async Task<IEnumerable<Postamat>> GetAll() =>
            await FindAll(false)
                .ToListAsync();

        public async Task<IEnumerable<Postamat>> GetActive() =>
            await FindByCondition(_=>_.IsActive,false)
                .OrderBy(_=>_.Number)
                .ToListAsync();

        public async Task<Postamat> Get(string number ) =>
            await FindByCondition(_ => _.Number == number, false)
                .SingleOrDefaultAsync();
    }
}
