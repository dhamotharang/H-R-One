using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HROne.Lib.Entities;
using HROne.DataAccess;

namespace HROne.TaskService
{

    //public class TaskParameter
    //{
    //    public DatabaseConnection dbConn;
    //    public EUser user;
    //    public TaskFactory processFactory;
    //}

    public abstract class TaskFactory
    {
        protected internal System.Globalization.CultureInfo executeCultureInfo;
        public TaskFactory()
        {
            executeCultureInfo = System.Threading.Thread.CurrentThread.CurrentUICulture;
        }
        public abstract bool Execute();
    }



    public class TaskQueueService
    {
        //protected System.Threading.Thread ServiceThread = null;
        protected List<System.Threading.Thread> threadList = new List<System.Threading.Thread>();
        protected System.Collections.Generic.Queue<TaskFactory> taskQueue = new System.Collections.Generic.Queue<TaskFactory>();
        protected int MaxThread = 1;

        public TaskQueueService(int MaxThread)
        {
            if (MaxThread > 0)
                this.MaxThread = MaxThread;
        }

        public TaskQueueService()
        {
            this.MaxThread = 1;
        }
        public System.Threading.Thread AddTask(TaskFactory taskFactory)
        {
            taskQueue.Enqueue(taskFactory);
            if (threadList.Count < MaxThread)
            {
                System.Threading.Thread ServiceThread = new System.Threading.Thread(new System.Threading.ThreadStart(Start));
                ServiceThread.IsBackground = false;
                ServiceThread.Start();
                threadList.Add(ServiceThread);
                return ServiceThread;
            }
            if (threadList.Count == 1)
                return threadList[0];
            else
                return null;
        }
        public virtual void Start()
        {
            while (taskQueue.Count > 0)
            {
                TaskFactory currentTaskFactory = taskQueue.Dequeue();
                if (currentTaskFactory.executeCultureInfo != null)
                    System.Threading.Thread.CurrentThread.CurrentUICulture = currentTaskFactory.executeCultureInfo;
                else
                    System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InstalledUICulture;

                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");

                if (!currentTaskFactory.Execute())
                    System.Threading.Thread.Sleep(10000);
                //taskQueue.Dequeue();
            }
            threadList.Remove(System.Threading.Thread.CurrentThread);
            //ServiceThread = null;
        }

    }
}
