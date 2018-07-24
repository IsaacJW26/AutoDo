using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDo.Model
{
    //in progress
    public class ReoccuringTask : ITaskDecoration
    {
        public ReoccuringTask()
        {
            
        }

        public ITaskDecoration Next { get; private set; }

        public ITaskDecoration Top => throw new NotImplementedException();


        public int CalcDynamicPriority(double priority)
        {
            throw new NotImplementedException();
        }

        public void CompleteTask(List<ITaskDecoration> completedList, List<ITaskDecoration> toDoList)
        {
            throw new NotImplementedException();
        }

        public void PostphoneTask(List<ITaskDecoration> completedList, List<ITaskDecoration> toDoList)
        {
            throw new NotImplementedException();
        }
    }
}
