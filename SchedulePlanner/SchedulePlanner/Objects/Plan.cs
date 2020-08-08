using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SchedulePlanner
{
    /// <summary>
    /// Class that holds data for a given plan.
    /// </summary>
    public class Plan
    {
        public string PlanName { get; set; }
        public int Date { get; set; }
        public int PlanID { get; set; }
        public string DateRepresentation { get; set; }
        public Plan(string plan, int date, int planID)
        {
            PlanName = plan;
            Date = date;
            PlanID = planID;
            DateRepresentation = "" + (date / 100) % 100 + "/" + date % 100 + "/" + (date / 10000);
        }
    }
}
