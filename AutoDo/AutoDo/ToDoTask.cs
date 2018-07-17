using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDo.Model
{
    //Container class
    public class ToDoTask : ITaskDecoration
    {
        //Properties------------
        public string Name { get; set; }
        public double EstimatedTime { get; set; }
        public Priority TaskPriority { get; set; }
        //
        public bool HasDueDate { get; set; }
        private DateTime dueDate;
        //
        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                if(value.CompareTo(DateTime.Today) < 0)
                {
                    throw new Exception();
                }
                dueDate = value;
            }
        }
        //Priority used to determine task order
        public new int DynamicPriority { get => GetDynamicPriority(); }
        //Reoccuring 


        //Can have a parent that is a task or category
        public ToDoTask ParentTask { get; private set; }
        //Can only have other tasks as children
        public List<ToDoTask> ChildTasks { get; private set; }

        //equivalent weight for tasks with no due date
        // 0.035 is equivalent to task due in approximately 2 weeks
        private const double NoDueDateWeight = 0.07f;


        //Constructor------------
        public ToDoTask(String inTaskName,
            double inEstimatedTime,
            Priority inPriority,
            bool inHasDueDate,
            DateTime inDueDate)
        {
            Name = inTaskName;
            EstimatedTime = inEstimatedTime;
            TaskPriority = inPriority;
            HasDueDate = inHasDueDate;
            DueDate = inDueDate;
            ChildTasks = new List<ToDoTask>();
        }

        // Calculates a priority based on start priority,
        // due date and estimated time.
        // A high priority value means task is more important.
        // Priority increases as due date approaches, higher start
        // priority estimated time is.
        private int CalcDynamicPriority(double daysLeftInverse)
        {
            double decPrio;
            int outPrio;

            //Priority as decimal, add 1 to avoid a 0 priority, 3 is now loswest possible prio.
            //Square root of estimated time because it should smaller factor in comparison to priority
            decPrio = ((double)TaskPriority + 1) * (Math.Sqrt(EstimatedTime)+1) * (daysLeftInverse+1);

            //Convert it to an integer, 5 digits is accurate enough
            outPrio = (int)(decPrio * 10000f);

            return outPrio;
        }

        private int GetDynamicPriority()
        {
            double daysLeftInverse;

            if (HasDueDate)
            {
                int daysLeft;

                //Calculates days between now
                daysLeft = Math.Abs(dueDate.Subtract(DateTime.Today).Days);
                //Sets inverse value to 2 if due date is today or earlier otherwise 1 divided by days remaining
                daysLeftInverse = (daysLeft > 0) ? 1 / ((double)daysLeft) : 2;
            }
            else
            {
                daysLeftInverse = NoDueDateWeight;
            }

            return CalcDynamicPriority(daysLeftInverse);
        }

        //Add child to from parent
        public void AddChild(ToDoTask inChild)
        {
            ChildTasks.Add(inChild);
            inChild.ParentTask = this;
        }
    }
}
