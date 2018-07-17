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

        public MainPage()
		{
			InitializeComponent();
            taskOrganiser = TaskOrganiser.GetInstance();
            Device.StartTimer(TimeSpan.FromSeconds(1.0), UpdateTime);
		}

        private void ToolbarItem_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddTasks());
        }

        private bool UpdateTime()
        {
            
            int hours, minutes, seconds, remainingTime;
            remainingTime = taskOrganiser.RemainingTime;

            hours = (remainingTime / (60 * 60));
            minutes = (remainingTime / 60) % 60;
            seconds = remainingTime % 60;

            taskName.Text = taskOrganiser.GetTaskName();
            taskTime.Text = String.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            
            return true;
        }

        private void postphoneButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                TaskOrganiser.GetInstance().PostphoneTask();
            }
            catch(TaskOrganiserException ex)
            {
                DisplayAlert("", ex.Message, "yeah alright");
            }
        }

        private void completeButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                TaskOrganiser.GetInstance().CompleteTask();
            }
            catch (TaskOrganiserException ex)
            {
                DisplayAlert("", ex.Message, "yeah alright");
            }
        }
    }
}
