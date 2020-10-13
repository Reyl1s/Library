using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models.UserDTO
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Год рождения")]
        public int Year { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Номер телефона")]
        public string Phone { get; set; }
    }
}
