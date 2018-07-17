using AutoDo.Controller;
using AutoDo.Model;
using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoDo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddTasks : ContentPage
	{
		public AddTasks()
		{
            InitializeComponent();

            //make sure due date is after current date
            dueDate.MinimumDate = DateTime.Today;
            ResetToDefault();
        }

        //Resets values to default
        private void ResetToDefault()
        {
            //default task name
            taskName.Text = "New Task";

            //set default completionTime
            completionTime.Text = "1";

            //set default due date
            hasDueDate.IsToggled = false;
            dueDate.Date = DateTime.Today;

            //sets default priority
            priorityPicker.SelectedIndex = 1;
        }

        //When button add task button is pressed
        private void Button_Pressed(object sender, EventArgs e)
        {
            String outTaskName;
            double outEstTime;
            Priority outPriority;
            bool outHasDueDate;
            DateTime outDueDate;
            ToDoTask outTask;

            //Create tasks
            outTaskName = taskName.Text;
            outEstTime = double.Parse(completionTime.Text);
            //Converts to int to Priority
            outPriority = (Priority)priorityPicker.SelectedIndex;
            outHasDueDate = hasDueDate.IsToggled;
            outDueDate = dueDate.Date;

            outTask = new ToDoTask(
                outTaskName,
                outEstTime,
                outPriority,
                outHasDueDate,
                outDueDate);

            //Add task to list
            TaskOrganiser.GetInstance().AddTask(outTask);

            //Alert user when task has been added
            DisplayAlert("", outTaskName + " has been added", "yeet");

            //Reset everything 
            ResetToDefault();
        }
    }
}