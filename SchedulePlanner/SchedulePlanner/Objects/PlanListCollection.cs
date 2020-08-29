using SchedulePlanner.Static_Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SchedulePlanner
{
    /// <summary>
    /// Class holding all scheduled plans of the user.
    /// </summary>
    public class PlanListCollection : ObservableCollection<PlanList>
    {
        public PlanListCollection()
        {
            // constructor required for deserialization at startup
        }

        /// <summary>
        /// Reassigns proper date values to each PlanList in the collection.
        /// </summary>
        /// <remarks>
        /// Date values in PlanList take on default values upon deserialization at startup.
        /// </remarks>
        public void Update_Dates()
        {
            foreach(PlanList pl in this)
            {
                pl.Update_Dates();
            }
        }

        /// <summary>
        /// Adds new event to collection. Events are ordered in chronological order.
        /// </summary>
        /// <param name="newEvent">Description of event</param>
        /// <param name="newEventDate">Date of event</param>
        /// <param name="planID">ID assigned to the event</param>
        public void Add_By_Date(string newEvent, int newEventDate, int planID)
        {
            int end = Count;
            int index = 0;
            while(index < end)
            {
                if (this[index].Date > newEventDate)
                {
                    Add_To_New_List(index, newEventDate, newEvent, planID);
                    break;
                }
                else if (this[index].Date == newEventDate)
                {
                    Add_To_Existing(index, newEventDate, newEvent, planID);
                    break;
                }
                index++;
            }
            if (index == end)
            {
                Add_To_New_List(end, newEventDate, newEvent, planID);
            }

        }

        /// <summary>
        /// Adds new PlanList to collection.
        /// </summary>
        /// <param name="index">Index to insert PlanList</param>
        /// <param name="newEventDate">Date of event</param>
        /// <param name="newEvent">Description of event</param>
        /// <param name="planID">ID assigned to the event</param>
        public void Add_To_New_List(int index, int newEventDate, string newEvent, int planID)
        {
            Insert(index, new PlanList(newEventDate)
            {
                new Plan(newEvent, newEventDate, planID)
            });
            AppProperties.Write_To_Properties(this, planID);
        }

        /// <summary>
        /// Adds event to existing PlanList.
        /// </summary>
        /// <param name="index">Index to insert plan within PlanList</param>
        /// <param name="newEventDate">Date of event</param>
        /// <param name="newEvent">Description of event</param>
        /// <param name="planID">ID assigned to the event</param>
        public void Add_To_Existing(int index, int newEventDate, string newEvent, int planID)
        {
            this[index].Add(new Plan(newEvent, newEventDate, planID));
            AppProperties.Write_To_Properties(this, planID);
        }

        /// <summary>
        /// Deletes plan with corresponding ID from collection.
        /// </summary>
        /// <param name="id">ID of plan to be removed</param>
        public void Delete_Selected(int id)
        {
            bool deleted = false;
            foreach (PlanList pl in this)
            {
                foreach (Plan p in pl)
                {
                    if (p.PlanID == id)
                    {
                        pl.Remove(p);
                        deleted = true;
                        break;
                    }
                }
                if (deleted)
                {
                    if (pl.Count == 0)
                    {
                        Remove(pl);
                    }
                    break;
                }
            }
            AppProperties.Write_To_Properties(this);

        }

        /// <summary>
        /// Updates event description of plan with corresponding ID.
        /// </summary>
        /// <param name="id">ID of plan to be changed.</param>
        /// <param name="new_text">New description of event</param>
        public void Edit_Selected(int id, string new_text)
        {
            bool found = false;
            foreach (PlanList pl in this)
            {
                foreach (Plan p in pl)
                {
                    if (p.PlanID == id)
                    {
                        p.PlanName = new_text;
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    break;
                }
            }
            AppProperties.Write_To_Properties(this);
        }
    }
}
