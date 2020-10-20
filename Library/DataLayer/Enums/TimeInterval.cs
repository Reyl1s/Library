using System.ComponentModel.DataAnnotations;

namespace DataLayer.Enums
{
    public enum TimeInterval
    {
        [Display(Name = "За день")]
        Day,

        [Display(Name = "За неделю")]
        Week,

        [Display(Name = "За месяц")]
        Mounth,

        [Display(Name = "За все время")]
        AllTime
    }
}
