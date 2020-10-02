using System.ComponentModel.DataAnnotations;

namespace DataLayer.Enums
{
    public enum BookStatus
    {
        [Display(Name = "Доступна")]
        Available,

        [Display(Name = "Забронирована")]
        Booked,

        [Display(Name = "Отдана")]
        Passed
    }
}
