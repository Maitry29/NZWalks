using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers

{
    [Route("api/[controller]")] // or  [Route("api/[Regions]")]
    [ApiController]

    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        //GET ALL Regions
        //GET : http://localhost:portnumber/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //GET data from database - Domain models
            var regionsDomain = dbContext.Regions.ToList();

            //Map/convert  Domain Models to DTOs
            var regionDTO = new List<RegionDTO>();
            foreach (var regionDomain in regionsDomain)
            {
                regionDTO.Add(new RegionDTO()
                {
                    ID = regionDomain.ID,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl,

                });
            }

            //Return DTOs
            return Ok(regionsDomain);

        }

        //Get single Reagion (Get Region By ID)
        //GET : http://localhost:portnumber/api/regions/{ID}
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id) 
        {
            //GET data from database - Domain models
            //var region = dbContext.Regions.Find(id);
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.ID == id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            //Map/convert  Domain Models to DTOs
            var regionDTO = new RegionDTO
            {

                ID = regionDomain.ID,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };
            //Return DTOs bcak to clint
            return Ok(regionDomain);
        }

        [HttpPost]
        //HHTPPOST to create new Region 
        //Post: http://localhost:portnumber/api/regions

        public IActionResult Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //Map/convert  Domain Models to DTOs
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl,
            };
            //Ise Domain Model to create Region
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            //Map/convert   Domain Model to DTO back
            var regionDTO = new RegionDTO
            {

                ID = regionDomainModel.ID,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.ID }, regionDTO);
        }

        //UPDATE Region
        //PUT: http://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id , [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            //Check if Region exist
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.ID == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //convert/mat DTo to domain model
            regionDomainModel.Code = updateRegionRequestDTO.Code;
            regionDomainModel.Name = updateRegionRequestDTO.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDTO.RegionImageUrl;

            dbContext.SaveChanges();

            //Map/convert   Domain Model to DTO back
            var regionDTO = new RegionDTO
            {

                ID = regionDomainModel.ID,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return Ok(regionDTO);
        }


        //DelaTE Region
        //DELETE: http://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            //Check if Region exist
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.ID == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //DELETE region
            dbContext.Remove(regionDomainModel);
            dbContext.SaveChanges();

            //optional (return Deleted region back)
            //Map/convert DOmain to DTO
            var regionDTO = new RegionDTO
            {

                ID = regionDomainModel.ID,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return Ok(regionDTO);

        }
    }
}

