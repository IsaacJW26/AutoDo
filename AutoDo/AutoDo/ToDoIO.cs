using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using AutoDo.Model;

namespace AutoDo
{
    /* -- File Format --
     * each line contains a single letter corresponding to the type of data,
     * followed by a '>' then the lines text 
     * Letter and types -
     * c - (c)urrent task
     * q - (q)ueued/todo task
     * f - (f)inished/completed task
     * e - (e)stimated task time
     * p - task (p)riority (0-3)
     * d - (d)ue date yyyy/mm/dd (none means no duedate)
     * 
     * //future implementation
     * a - p(a)rent
     * g - cate(g)ory
     * r - (r)emaining time
     */
    /* -- Example --
     * c>Steam Hams
     * e>0.5
     * p>4
     * 
     * q>Grocery Shopping
     * e>1.0
     * p>1
     * d>2020/4/20
     * 
     * q>Destroy City
     * e>12
     * p>0
     *  
     * f>Build deathlaser
     * e>12.0
     * p>3
     * 
     * f>Write Report
     * e>2.0
     * p>2
     * d>6969/12/1
     */
    class ToDoIO
    {
        private readonly string fullFilePath;
        public const string fileName = "info.txt";

        public ToDoIO()
        {
            string filePath =
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData);

            fullFilePath = Path.Combine(filePath, fileName);

            //https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/files?tabs=vswin
        }

        //write to file
        public void WriteToFile(List<ToDoTask> completedList, List<ToDoTask> toDoList,
            ToDoTask currentTask)
        {
            string outText = "";
            //write current task to file
            outText = TaskToFileString(currentTask, 'c', outText);

            //write all completed tasks to file
            foreach (ToDoTask tempTask in completedList)
            {
                outText = TaskToFileString(tempTask, 'f', outText);
            }
            //all
            foreach (ToDoTask tempTask in toDoList)
            {
                outText = TaskToFileString(tempTask, 'q', outText);
            }

            //write everything to file
            File.WriteAllText(fullFilePath, outText);
        }

        //Converts task to string with valid file format
        private string TaskToFileString(ToDoTask toDoTask, char prefix, string originalString)
        {
            string outText = "";

            //remove newlines from name to prevent file corruption
            string taskName = toDoTask.Name.Replace('\n', ' ');
            //add task name to end of string
            outText = String.Format("{0}{1}>{2}\n", outText, prefix, taskName);

            //estimated time
            outText = String.Format("{0}e>{1}\n", outText, toDoTask.EstimatedTime);

            //task priority
            outText = String.Format("{0}p>{1}\n", outText, (int)toDoTask.TaskPriority);

            //add duedate line only if duedate exists
            if (toDoTask.HasDueDate)
            {
                //due date
                outText = String.Format("{0}d>{1}\n", outText, toDoTask.DueDate.ToString());
            }

            //return string appended to original
            return String.Format("{0}{1}", originalString, outText);
        }

        //read from file
        public void ReadFromFile(ref List<ToDoTask> completedList, ref List<ToDoTask> toDoList,
            ref ToDoTask currentTask)
        {
            //file io components
            string fileInfo = "";
            string[] fileParts;

            //debugging
            Console.WriteLine(fullFilePath + " exists "
                +File.Exists(fullFilePath));

            if (File.Exists(fullFilePath))
            {
                fileInfo = File.ReadAllText(fullFilePath);
                Console.WriteLine(fileInfo);

                fileParts = fileInfo.Split('\n');

                for (int ii = 0; ii < fileParts.Length; ii++)
                {
                    ProcessLine(fileParts[ii], ref completedList, ref toDoList, ref currentTask);
                }
            }
            //if file does not exist; do nothing

        }

        private void ProcessLine(string lineInfo, ref List<ToDoTask> completedList,
            ref List<ToDoTask> toDoList,
            ref ToDoTask currentTask)
        {
            //task components
            string tempName = "";
            double tempTime = 1f;
            Priority tempPriority = Priority.Medium;
            bool tempHasDDate = false;
            DateTime tempDDate = DateTime.Today.AddDays(3.0);
            ToDoTask tempTask = null;

            //current task, todolist or completed
            char destination = 'q';

            char lineType;
            char[] lineArr = lineInfo.ToCharArray();

            //format is correct
            if (lineArr[1] == '>')
            {
                //get first character in line
                lineType = lineInfo.ToCharArray()[0];
                switch (lineType)
                {
                    //current task
                    case 'c':
                        destination = 'c';
                        tempName = lineInfo.Substring(2);
                        break;

                    //queued / todo task
                    case 'q':
                        destination = 'q';
                        tempName = lineInfo.Substring(2);
                        break;

                    //finished / completed task
                    case 'f':
                        destination = 'f';
                        tempName = lineInfo.Substring(2);
                        break;

                    //estimated task time
                    case 'e':
                        tempTime = double.Parse(lineInfo.Substring(2));
                        break;

                    //task priority (0 - 3)
                    case 'p':
                        tempPriority = (Priority)(int.Parse(lineInfo.Substring(2)));
                        break;

                    //due date yyyy/mm/dd (none means no duedate)
                    case 'd':
                        tempDDate = DateTime.Parse(lineInfo.Substring(2));
                        tempHasDDate = true;
                        break;

                    //ignore
                    default:
                        break;
                }
            }

            tempTask = new ToDoTask(tempName, tempTime, tempPriority, tempHasDDate, tempDDate);
            //else ignore line
            switch (destination)
            {
                //current task
                case 'c':
                    currentTask = tempTask;
                    break;

                //queued / todo task
                case 'q':
                    toDoList.Add(tempTask);
                    break;

                //finished / completed task
                case 'f':
                    completedList.Add(tempTask);
                    break;
            }
        }
    }
}
