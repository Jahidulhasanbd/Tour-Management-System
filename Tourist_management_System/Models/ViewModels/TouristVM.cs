using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tourist_management_System.Models.ViewModels
{
    public class TouristVM
    {
        public TouristVM()
        {
            this.SpotList = new List<int>();
        }
        public int TouristId { get; set; }
        [Required, StringLength(50, ErrorMessage = "Tourist name is required!!"), Display(Name = "Tourist Name")]
        public string TouristName { get; set; } = default!;
        [Required, Column(TypeName = "date"), Display(Name = "Date of Birth"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        [Display(Name = "Image")]
        public IFormFile? PictureFile { get; set; }
        public string? Picture { get; set; }
        public bool MaritalStatus { get; set; }
        public List<int> SpotList { get; set; }
    }
}
