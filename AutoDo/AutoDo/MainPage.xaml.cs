using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using AutoDo.Controller;

namespace AutoDo
{
	public partial class MainPage : ContentPage
	{
        TaskOrganiser taskOrganiser;

        //Main page 
        public MainPage()
		{
			InitializeComponent();
            taskOrganiser = TaskOrganiser.GetInstance();
            //Update timer every second
            Device.StartTimer(TimeSpan.FromSeconds(1.0), UpdateTime);
		}

        //When the toolbar button is pressed, open Add Tasks page
        private void ToolbarItem_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddTasks());
        }

        //Update timer time every second
        private bool UpdateTime()
        {
            int hours, minutes, seconds, remainingTime;
            remainingTime = taskOrganiser.RemainingTime;

            //convert total seconds to hours
            hours = (remainingTime / (60 * 60));
            //convert total seconds to minutes
            minutes = (remainingTime / 60) % 60;
            //convert total seconds to seconds out of 60
            seconds = remainingTime % 60;

            //Set task name to current tasks name
            taskName.Text = taskOrganiser.GetTaskName();
            //set task time with HH:MM:SS format
            taskTime.Text = String.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

            return true;
        }

        //When the "postphone" button is pressed in the main task page
        private void postphoneButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                TaskOrganiser.GetInstance().PostphoneTask();
            }
            //display error with error message
            catch (TaskOrganiserException ex)
            {
                DisplayAlert("", ex.Message, "OK");
            }
        }

        //When the "complete" button is pressed in the main task page
        private void completeButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                TaskOrganiser.GetInstance().CompleteTask();
            }
            //display error with error message
            catch (TaskOrganiserException ex)
            {
                DisplayAlert("", ex.Message, "OK");
            }
        }
    }
}
