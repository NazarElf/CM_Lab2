﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CM_Lab2_WPF
{
    class DeviceNode
    {
        //Device Name
        public string name;
        //Built in Random generator
        Random r = new Random();
        //Number of tasks that can be processing at the same time
        private uint TASKS_PER_PROC = 1;
        //Queue of tasks
        public uint queue = 0;
        //Time to compleate task on proc
        private double tau;
        //Probabilities of Transitions
        public Dictionary<DeviceNode, double> Transition = new Dictionary<DeviceNode, double>();
        //Tasks that exec on proc, if -1 => inactive
        private double[] onProc;
        //Have amount of time when processor was busy
        public double[] busyTime;


        public DeviceNode(double tau, string name, uint queue = 0, uint maxParalelTasks = 1)
        {
            if (maxParalelTasks == 0)
                throw new Exception("Device should have at least 1 thread of tasks");
            TASKS_PER_PROC = maxParalelTasks;
            onProc = new double[TASKS_PER_PROC];
            busyTime = new double[TASKS_PER_PROC];
            this.tau = tau;
            this.queue = queue;
            this.name = name;
            //set every core as inactive
            for (int i = 0; i < TASKS_PER_PROC; i++)
                onProc[i] = -1.0;
        }

        public void Refresh()
        {
            //set every core as inactive
            for (int i = 0; i < TASKS_PER_PROC; i++)
            {
                busyTime[i] = 0.0;
                onProc[i] = -1.0;
            }
        }

        public void EndTask()
        {
            if (Transition.Count == 0)
                throw new InvalidTransitionException("Task must have transitions!!!");
            for (int i = 0; i < TASKS_PER_PROC; i++)
            {
                if (onProc[i] == 0)
                {
                    //add busy time
                    busyTime[i] += onProc[i];
                    //set onProc disabled
                    onProc[i] = -1;
                    //start next task
                    StartTask();

                    double sum = 0.0;
                    foreach (var item in Transition.Values)
                    {
                        sum += item;
                    }
                    //generating random value between 0 and 1 
                    //and multiplex it on sum of probabilities to have sum of chances = 1.0
                    //that needs to sell task to another device
                    double temp = sum * r.NextDouble();
                    foreach(var item in Transition)
                    {
                        //if random value greatre skip that transition
                        //and let have chance to other transitions
                        if (temp > item.Value)
                            temp -= item.Value;
                        else
                        {
                            //add task to queue
                            item.Key.queue++;
                            //try to start task
                            item.Key.StartTask();
                            //after sending task to next node cycle needs to break
                            break;
                        }
                    }
                    //We need no anymore this cycle
                    break;
                }
            }
        }

        public void StartTask()
        {
            for (int i = 0; i < TASKS_PER_PROC; i++)
            {
                if(onProc[i] <= 0 && queue != 0)
                {
                    //remove task from queue
                    --queue;
                    //generate task length time and set it onProc
                    onProc[i] = GenPoissonValue();
                }
            }
        }

        public void SetTau(double Tau)
        {
            MyMessageBox.Show("TauChanged!      ",this.name,MyMessageBoxButton.Ok,MyMessageBoxImage.Nuclear);
            this.tau = Tau;
        }

        public double GetTau()
        {
            return tau;
        }

        /// <summary>
        /// Searching the lowest left time from active cores
        /// </summary>
        /// <returns>The lowest left time on this Device</returns>
        public double GetLowestTime()
        {
            double min = double.PositiveInfinity;
            for (int i = 0; i < TASKS_PER_PROC; i++)
                if (onProc[i] > 0 && onProc[i] < min)
                    min = onProc[i];
            return min;
        }

        /// <summary>
        /// Substract time from every active core
        /// </summary>
        /// <param name="time">Parameter that defines how much time to substract</param>
        public void SubstractTime(double time)
        {
            for (int i = 0; i < TASKS_PER_PROC; i++)
                if (onProc[i] > 0)
                {
                    onProc[i] -= time;
                    busyTime[i] += time;
                }
        }

        /// <summary>
        /// Adding transition with restriction to haven't summary probability more than 1.0
        /// </summary>
        /// <param name="trans">Node that will be next with specified probability</param>
        /// <param name="probability">Probability for transition to specified Node</param>
        public void AddTransition(DeviceNode trans, double probability)
        {
            Transition.Add(trans, probability);
        }

        /// <summary>
        /// This function generates random int by Poisson distribution
        /// </summary>
        /// <returns>Returns generated double</returns>
        private double GenPoissonValue()
        {
            return -tau * Math.Log(r.NextDouble());
        }
    }
}
