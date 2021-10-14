using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PostamatService.Data;
using PostamatService.Web.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PostamatService.Web.Controllers
{
    [Route("api/postamat")]
    [ApiController]
    public class PostamatController : ControllerBase
    {
        private readonly IPostamatRepository _postamatRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PostamatController> _logger;

        public PostamatController(IPostamatRepository postamatRepository, IMapper mapper, ILogger<PostamatController> logger)
        {
            _postamatRepository = postamatRepository;
            _mapper = mapper;
            _logger = logger;
        }
        // GET: api/<PostamatController>/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var postamats = await _postamatRepository.GetActive(false);
            var postamatsDto = _mapper.Map<IEnumerable<PostamatDto>>(postamats);
            return Ok(postamatsDto);
        }

        // GET api/<PostamatController>/0000-000
        [HttpGet("{number}")]
        public async Task<IActionResult> Get(string number)
        {
            var postamat = await _postamatRepository.Get(number, false);
            if (postamat == null)
            {
                _logger.LogInformation($"Postamat with number: {number} doesn't exist.");
                return NotFound();
            }
            var postamatDto = _mapper.Map<PostamatDto>(postamat);
            return Ok(postamatDto);
        }
    }
}
