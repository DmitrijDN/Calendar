using System.Collections.Generic;
using Bs.Calendar.Mvc.ViewModels.Events;

namespace Bs.Calendar.Mvc.ViewModels.Home
{
    public class CalendarVm
    {
        public IEnumerable<BirthdayEventVm> BirthdayEvents { get; set; }
        public IEnumerable<CalendarCellEventVm> CalendarEvents { get; set; }
    }
}