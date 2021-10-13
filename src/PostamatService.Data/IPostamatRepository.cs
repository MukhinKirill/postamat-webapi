using System.Collections.Generic;
using System.Threading.Tasks;
using PostamatService.Data.Models;

namespace PostamatService.Data
{
    public interface IPostamatRepository
    {
        Task<IEnumerable<Postamat>> GetAll(bool trackChanges);
        Task<IEnumerable<Postamat>> GetActive(bool trackChanges);
        Task<Postamat> Get(string number, bool trackChanges);
        Task SaveAsync();
    }
}
