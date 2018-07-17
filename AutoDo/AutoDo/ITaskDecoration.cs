using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDo.Model
{
    public abstract class ITaskDecoration
    {
        //properties---------
        //
        protected ITaskDecoration Next { get; private set; }
        //First Decoration in list
        protected ITaskDecoration Top { get; }
        //modifies last priority
        public int DynamicPriority { get; }

        //methods------------
        //add new decoration to decorator
        public virtual void AddDecoration(ITaskDecoration taskDecoration)
        {
            if (Next != null)
            {
                Next.AddDecoration(taskDecoration);
            }
            else
            {
                Next = taskDecoration;
            }
        }

        //after this task is completed
        public abstract void CompleteTask(List<ITaskDecoration> completedList, List<ITaskDecoration> toDoList);

        //
        public abstract void PostphoneTask(List<ITaskDecoration> completedList, List<ITaskDecoration> toDoList);


    }
}
