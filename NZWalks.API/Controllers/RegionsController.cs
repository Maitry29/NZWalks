using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers

{
    [Route("api/[controller]")] // or  [Route("api/[Regions]")]
    [ApiController]

    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContextc, IRegionRepository regionRepository , IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        //GET ALL Regions
        //GET : http://localhost:portnumber/api/regions
        [HttpGet]
        public async Task< IActionResult> GetAll()
        {
            //GET data from database - Domain models
            // var regionsDomain = await regionRepository.GetAllAsync();

            //Map/convert  Domain Models to DTOs
            /*var regionDTO = new List<RegionDTO>();
            foreach (var regionDomain in regionsDomain)
            {
                regionDTO.Add(new RegionDTO()
                {
                    ID = regionDomain.ID,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl,

                });
            }*/

            //GET data from database - Domain models
            var regionsDomain = await regionRepository.GetAllAsync();

            //Map/convert  Domain Models to DTOs
            var regionDtos = mapper.Map<List<RegionDTO>>(regionsDomain);
            //Return DTOs
            return Ok(regionsDomain);

        }

        //Get single Reagion (Get Region By ID)
        //GET : http://localhost:portnumber/api/regions/{ID}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task< IActionResult> GetById([FromRoute] Guid id) 
        {
            //GET data from database - Domain models
            //var region = dbContext.Regions.Find(id);
            var regionDomain = await regionRepository.GetByIDAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            //Map/convert  Domain Models to DTOs
            /*var regionDTO = new RegionDTO
            {

                ID = regionDomain.ID,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };*/

            //Return DTOs bcak to clint
            return Ok(mapper.Map<RegionDTO>(regionDomain));
        }

        [HttpPost]
        //HHTPPOST to create new Region 
        //Post: http://localhost:portnumber/api/regions

        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //Map/convert  DTOs to Domain Models
            /* var regionDomainModel = new Region
             {
                 Code = addRegionRequestDTO.Code,
                 Name = addRegionRequestDTO.Name,
                 RegionImageUrl = addRegionRequestDTO.RegionImageUrl,
             };*/


            //Map/convert  DTOs to Domain Models
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDTO);


            //Ise Domain Model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map/convert   Domain Model to DTO back
            /*var regionDTO = new RegionDTO
            {

                ID = regionDomainModel.ID,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };*/
            //Map/convert   Domain Model to DTO back
            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.ID }, regionDTO);
        }

        //UPDATE Region
        //PUT: http://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id , [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            //Map dto tp domain
            /* var regionDomainModel = new Region
             {
                 Code = updateRegionRequestDTO.Code,
                 Name = updateRegionRequestDTO.Name,
                 RegionImageUrl = updateRegionRequestDTO.RegionImageUrl,
             };*/

            //Map dto tp domain
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDTO);
            //Check if Region exist
            regionDomainModel =  await regionRepository.UpdateAsync(id, regionDomainModel);
            
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Map/convert   Domain Model to DTO back
            /*     var regionDTO = new RegionDTO
                 {

                     ID = regionDomainModel.ID,
                     Code = regionDomainModel.Code,
                     Name = regionDomainModel.Name,
                     RegionImageUrl = regionDomainModel.RegionImageUrl,
                 };*/

            //Map/convert   Domain Model to DTO back
            var regionDTO = mapper.Map<RegionDTO> (regionDomainModel);
            return Ok(regionDTO);
        }


        //DelaTE Region
        //DELETE: http://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //Check if Region exist
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            

           

            return Ok(mapper.Map<RegionDTO>(regionDomainModel));

        }
    }
}

