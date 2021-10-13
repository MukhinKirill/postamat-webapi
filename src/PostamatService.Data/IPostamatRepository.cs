using System.Collections.Generic;

using PostamatService.Data.Models;

namespace PostamatService.Data
{
    public interface IPostamatRepository
    {
        IEnumerable<Postamat> GetAll();
        IEnumerable<Postamat> GetActive();
        IEnumerable<Postamat> Get(string number);
    }
}
