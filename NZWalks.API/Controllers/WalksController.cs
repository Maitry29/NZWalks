using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalksRepository walksRepository;

        public WalksController(IMapper mapper, IWalksRepository walksRepository)
        {
            this.mapper = mapper;
            this.walksRepository = walksRepository;
        }


        // CREATE Walk
        // POST: /api/walks
        [HttpPost]
        // [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDTO addWalksRequestDto)
        {
            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walks>(addWalksRequestDto);
            await walksRepository.CreateAsync(walkDomainModel);

            //Map Domain model to DTO
            return Ok(mapper.Map<WalksDTO>(walkDomainModel));
        }


        //GET Walks
        //GET: /api/walks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walkDomainModel = await walksRepository.GetAllAsync();

            //Map Domain Model to DTO
            return Ok(mapper.Map<List<WalksDTO>>(walkDomainModel));
        }


        //GET walk by ID
        //GET/api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var walkDomainModel = await walksRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }

            //Map Domain Mode to DTO
            return Ok(mapper.Map<WalksDTO>(walkDomainModel));
        }

        // Update Walk By Id
        // PUT: /api/Walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        // [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalksRequestDTO updateWalksRequestDto)
        {

            // Map DTO to Domain Model
            var walkDomainModel1 = mapper.Map<Walks>(updateWalksRequestDto);

            walkDomainModel1 = await walksRepository.UpdateAsync(id, walkDomainModel1);

            if (walkDomainModel1 == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<WalksDTO>(walkDomainModel1));
        }

        // Delete a Walk By Id
        // DELETE: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await walksRepository.DeleteAsync(id);

            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(mapper.Map<WalksDTO>(deletedWalkDomainModel));

        }
    }
}
