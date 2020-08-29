using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Windows.Input;
using SchedulePlanner.Static_Factories;

namespace SchedulePlanner
{
    /// <summary>
    /// Handles all events triggered by the MainPage and updates MainPage accordingly.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        /// <summary>The date of the event being added/edited.</summary>
        private int date;
        /// <summary>Today's date.</summary>
        private readonly DateTime today;
        /// <summary>The description of the event being added/edited.</summary>
        private string newEvent;
        /// <summary>The ID of the next plan to be added.</summary>
        private int planID;
        /// <summary>The event being edited, if such an event exists.</summary>
        private Label beingEdited;
        /// <summary>True when handling an event, false otherwise.</summary>
        /// <remarks>Another event won't be accepted until current event is completed.</remarks>
        private bool busy = false;
        public PlanListCollection All { private set; get; }
        public MainPage()
        {
            InitializeComponent();
            AppProperties.Initialize_App_Properties();
            today = DP.Date;
            date = 0;
            planID = AppProperties.Get_Plan_ID();
            newEvent = "";
            All = AppProperties.Get_PlanListCollection();
            All.Update_Dates();
            GroupedView.ItemsSource = All;
            beingEdited = new Label();
        }

        /// <summary>
        /// Fired when "+" button is clicked. Brings up Add Event window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Clicked(object sender, EventArgs e)
        {
            if(busy) return;
            busy = true;
            Initialize_Add_Event();
            AddEventPopUp.IsVisible = true;
        }

        /// <summary>
        /// Sets default values for Add Event window.
        /// </summary>
        private void Initialize_Add_Event()
        {
            EntryField.Text = "";
            newEvent = EntryField.Placeholder;
            ReminderCheckBox.IsChecked = false;
            DateTime dt = today;
            DP.Date = today;
            date = dt.Day + dt.Month * 100 + dt.Year * 10000;
        }

        /// <summary>
        /// Fires when "Done" is selected from Add Event window.
        /// <para>Adds new event to collection and closes Add Event window.</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Finish_Add_Event(object sender, EventArgs e)
        {
            newEvent = newEvent.Equals("") ? "Unnamed Event" : newEvent;
            All.Add_By_Date(newEvent, date, ++planID);
            AddEventPopUp.IsVisible = false;
            busy = false;
        }

        /// <summary>
        /// Fires when "Cancel" is selected from Add Event window.
        /// <para>Closes Add Event window.</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Add_Event(object sender, EventArgs e)
        {
            AddEventPopUp.IsVisible = false;
            busy = false;
        }

        /// <summary>
        /// Fires when existing event is tapped on home page. Brings up Edit Event window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_Event(object sender, EventArgs e)
        {
            if(busy) return;
            busy = true;
            Label label = (Label)sender;
            DateTime dt = Conversion.Date_To_Date_Time(Conversion.String_To_Date(label.ClassId));
            Initialize_Edit_Event(label, dt);
        }

        /// <summary>
        /// Sets default values for Edit Event window.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="dt"></param>
        private void Initialize_Edit_Event(Label label, DateTime dt)
        {
            EntryFieldEdit.Text = !label.Text.Equals("Unnamed Event") ? label.Text : "";
            newEvent = label.Text;
            DPEdit.Date = dt;
            date = dt.Day + dt.Month * 100 + dt.Year * 10000;
            ReminderCheckBoxEdit.IsChecked = true;
            beingEdited = label;
            EditEventPopUp.IsVisible = true;
        }

        /// <summary>
        /// Fires when "Done" is selected from Edit Event window.
        /// <para>Updates event in collection and closes Edit Event window.</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Finish_Edit_Event(object sender, EventArgs e)
        {
            int oldDate = Conversion.String_To_Date(beingEdited.ClassId);
            newEvent = newEvent.Equals("") ? "Unnamed Event" : newEvent;
            Grid grid = (Grid)beingEdited.Parent;
            int id = int.Parse(grid.Children[0].ClassId);
            if (date != oldDate)
            {
                All.Delete_Selected(id);
                All.Add_By_Date(newEvent, date, ++planID);
            }
            else
            {
                beingEdited.Text = newEvent;
                All.Edit_Selected(id, newEvent);
            }
            EditEventPopUp.IsVisible = false;
            busy = false;
        }

        /// <summary>
        /// Fires when "Cancel" is selected from Edit Event window.
        /// <para>Closes Edit Event window.</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Edit_Event(object sender, EventArgs e)
        {
            EditEventPopUp.IsVisible = false;
            busy = false;
        }

        /// <summary>
        /// Fires when a checkbox is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CheckBox_Checked(object sender, CheckedChangedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (busy)
            {
                checkBox.IsChecked = false;
                return;
            }
            busy = true;
            var classID = checkBox.ClassId;
            Grid grid = (Grid)checkBox.Parent;
            Label label = (Label)grid.Children[1];
            label.FadeTo(0, length: 1000, easing: Easing.SinIn);
            await checkBox.FadeTo(0, length: 1000, easing: Easing.SinIn);
            All.Delete_Selected(int.Parse(classID));
            busy = false;
        }

        /// <summary>
        /// Fires when an entry field is completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void On_Entry_Completed(object sender, EventArgs e)
        {
            newEvent = ((Entry)sender).Text;
        }

        /// <summary>
        /// Fires when a date is selected from the DatePicker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Date_Selected(object sender, DateChangedEventArgs e)
        {
            DateTime dateTime = ((DatePicker)sender).Date;
            date = dateTime.Day + dateTime.Month * 100 + dateTime.Year * 10000;
        }
    }
}