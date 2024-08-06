using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDTO
    {
        [Required]
        [MinLength(3,ErrorMessage ="code Has to be minimun 3 char")]
        [MaxLength(3, ErrorMessage = "code Has to be miximum 3 char")]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
