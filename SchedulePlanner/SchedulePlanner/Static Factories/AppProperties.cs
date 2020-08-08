using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulePlanner.Static_Factories
{
    /// <summary>
    /// Handles all App.Current.Properties interactions.
    /// </summary>
    public static class AppProperties
    {
        /// <summary>
        /// Ensures that an initial value exists for all keys in App.Current.Properties
        /// </summary>
        public static void Initialize_App_Properties()
        {
            if (!App.Current.Properties.ContainsKey("PlanID"))
            {
                Write_To_PlanID("" + 0);
            }
            if (!App.Current.Properties.ContainsKey("All"))
            {
                Write_To_All(JsonConvert.SerializeObject(new PlanListCollection()));
            }
        }

        /// <summary>
        /// Gets the next plan ID to be assigned.
        /// </summary>
        /// <returns>The next available plan ID</returns>
        public static int Get_Plan_ID()
        {
            return int.Parse(App.Current.Properties["PlanID"] as string);
        }

        /// <summary>
        /// Gets the most updated list of plans.
        /// </summary>
        /// <returns>The collection of all plans the user has not completed</returns>
        public static PlanListCollection Get_PlanListCollection()
        {
            return JsonConvert.DeserializeObject<PlanListCollection>(App.Current.Properties["All"] as string);
        }

        /// <summary>
        /// Updates necessary App Properties. Called when adding plans to the collection.
        /// </summary>
        /// <param name="all">The updated collection of plans</param>
        /// <param name="planID">The next available plan ID</param>
        public static void Write_To_Properties(PlanListCollection all, int planID)
        {
            Write_To_All(JsonConvert.SerializeObject(all));
            Write_To_PlanID("" + planID);
            App.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// Updates necessary App Properties. Called when editing or removing plans from the collection
        /// </summary>
        /// <param name="all"></param>
        public static void Write_To_Properties(PlanListCollection all)
        {
            Write_To_All(JsonConvert.SerializeObject(all));
            App.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// Handles the update for the PlanListCollection.
        /// </summary>
        /// <param name="json">Serialized string of the updated PlanListCollection</param>
        public static void Write_To_All(string json)
        {
            if (App.Current.Properties.ContainsKey("All"))
            {
                App.Current.Properties["All"] = json;
            }
            else
            {
                App.Current.Properties.Add("All", json);
            }
        }

        /// <summary>
        /// Handles the update for the plan ID.
        /// </summary>
        /// <param name="id">The next available plan ID</param>
        public static void Write_To_PlanID(string id)
        {
            if (App.Current.Properties.ContainsKey("PlanID"))
            {
                App.Current.Properties["PlanID"] = id;
            }
            else
            {
                App.Current.Properties.Add("PlanID", id);
            }
        }
    }
}
