using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SchedulePlanner
{
    /// <summary>
    /// Class that holds all plans for a given date.
    /// </summary>
    public class PlanList : ObservableCollection<Plan>
    {
        public string DateRepresentation { get; set; }
        public int Date { get; set; }
        public PlanList(int date)
        {
            Date = date;
            DateRepresentation = "" + (date / 100) % 100 + "/" + date % 100 + "/" + (date / 10000);
        }

        public PlanList()
        {
            Date = 0;
            DateRepresentation = "Unassigned Date";
        }

        /// <summary>
        /// Reassigns proper date values to a given PlanList.
        /// </summary>
        public void Update_Dates()
        {
            Date = this[0].Date;
            DateRepresentation = this[0].DateRepresentation;
        }
    }
}
