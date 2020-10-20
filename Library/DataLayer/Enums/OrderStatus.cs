using System.ComponentModel.DataAnnotations;

namespace DataLayer.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Забронировано")]
        Booked,

        [Display(Name = "Отменено")]
        Cancelled,

        [Display(Name = "Возвращено")]
        Returned
    }
}
