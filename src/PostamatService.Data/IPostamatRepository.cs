using System.Collections.Generic;
using System.Threading.Tasks;
using PostamatService.Data.Models;

namespace PostamatService.Data
{
    public interface IPostamatRepository
    {
        Task<IEnumerable<Postamat>> GetAll();
        Task<IEnumerable<Postamat>> GetActive();
        Task<Postamat> Get(string number);
    }
}
