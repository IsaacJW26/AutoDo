using System;
using System.Collections.Generic;
using System.Text;
using AutoDo.Model;
using Xamarin.Forms;

namespace AutoDo.Controller
{
    public class TaskOrganiser
    {
        private static TaskOrganiser INSTANCE;
        private List<ToDoTask> toDoList;
        private List<ToDoTask> completedList;
        private ToDoTask currentTask;

        public int RemainingTime { get; private set; }

        //uses alternate constructor
        private TaskOrganiser() : this(new List<ToDoTask>(), new List<ToDoTask>()) { }

        private TaskOrganiser(List<ToDoTask> inToDoTasks,
            List<ToDoTask> inCompletedList)
        {
            ToDoIO readIO;
            readIO = new ToDoIO();

            //if lists do not exist
            toDoList = inToDoTasks;
            completedList = inCompletedList;

            //set default values
            currentTask = null;
            RemainingTime = 0;

            //read all info from file
            readIO.ReadFromFile(ref inCompletedList, ref inToDoTasks, ref currentTask);

            //Start update timer
            Device.StartTimer(TimeSpan.FromSeconds(1.0), UpdateTime);
        }

        //
        public static TaskOrganiser GetInstance()
        {
            //returns instance if it's not null, otherwise returns new task scheduler
            INSTANCE = INSTANCE ?? new TaskOrganiser();
            return INSTANCE;
        }

        //finds next highest
        public ToDoTask GetNextHighest(ToDoTask inTask)
        {
            int currPrio = 0;
            ToDoTask currTask = inTask;

            if(toDoList.Count <= 0)
            {
                throw new TaskOrganiserException("ToDo list is empty");
            }
            else if (currTask == null)
            {
                currTask = toDoList.ToArray()[0];
            }

            //finds highest task
            foreach (ToDoTask task in toDoList)
            {
                if(task.DynamicPriority > currPrio &&
                    task != inTask)
                {
                    currPrio = task.DynamicPriority;
                    currTask = task;
                }
            }

            return currTask;
        }

        //adds task to list
        //returns true if task already exists
        public bool AddTask(ToDoTask inTask)
        {
            bool outVal;
            Console.WriteLine("adding task " + inTask.Name +
                              " prio " + inTask.TaskPriority +
                              " time " + inTask.EstimatedTime +
                              " dynprio " + inTask.DynamicPriority);

            outVal = toDoList.Contains(inTask);
            //If toDoList does not have task, add it to 
            if (!outVal)
            {
                toDoList.Add(inTask);
                //update file
                ToDoIO toDoIO = new ToDoIO();
                toDoIO.WriteToFile(completedList, toDoList, currentTask);
            }

            return outVal;
        }

        //moves current task to completedList and replaces it with next highest priority task
        public void CompleteTask()
        {
            RemainingTime = 0;
            //add old task to completed
            if (currentTask != null)
            {
                completedList.Add(currentTask);
                currentTask = null;
            }

            if (toDoList.Count > 0)
            {
                //get next task from list
                currentTask = GetNextHighest(currentTask);

                //remove current from to do 
                toDoList.Remove(currentTask);

                //calculate remaining time
                //IMPLEMENT REMAINING TIME TO TODOTASK
                RemainingTime = (int)(currentTask.EstimatedTime * 60 * 60);

                //update file
                ToDoIO toDoIO = new ToDoIO();
                toDoIO.WriteToFile(completedList, toDoList, currentTask);
            }
            else
            {
                throw new TaskOrganiserException("ToDo list is empty, no task could be started");
            }
        }

        public void PostphoneTask()
        {
            if (toDoList.Count > 0)
            {
                ToDoTask nextTask;

                //get next task
                nextTask = GetNextHighest(currentTask);
                //remove from it from the to do list
                toDoList.Remove(nextTask);

                //after next task add old task back into list
                if (currentTask != null)
                {
                    //add old task back into todo
                    toDoList.Add(currentTask);
                }

                currentTask = nextTask;
                RemainingTime = (int)(currentTask.EstimatedTime * 60 * 60) + 1;
            }
            else
            {
                throw new TaskOrganiserException("ToDo list is empty, no task could be started");
            }
        }

        public ToDoTask GetCurrentTask()
        {
            if(currentTask == null)
            {
                currentTask = GetNextHighest(null);
                toDoList.Remove(currentTask);
            }

            return currentTask;
        }

        public string GetTaskName()
        {
            string outString = "";
            if (currentTask == null)
            {
                try
                {
                    currentTask = GetNextHighest(null);
                    toDoList.Remove(currentTask);
                    outString = currentTask.Name;
                }
                // do nothing
                catch (TaskOrganiserException) { }
            }
            else
            {
                outString = currentTask.Name;
            }

            return outString;
        }

        public bool UpdateTime()
        {
            //when time runs out postphone task and go to next
            if (RemainingTime <= 0)
            {
                RemainingTime = 0;
                if (toDoList.Count > 0)
                    PostphoneTask();
            }
            else
            {
                RemainingTime--;
            }
            return true;
        }
    }

    //Custom exception for task scheduler errors
    public class TaskOrganiserException : Exception
    {
        public TaskOrganiserException(string message) : base(message)
        { }

        public TaskOrganiserException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
