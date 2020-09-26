using System;
using System.Collections.Generic;
using System.Text;

namespace DelegatesLambdasEvents
{
    public class Subject
    {
        public string Name { get; set; }
        private Action triggerListeners;

        public event EventHandler TriggerListenersByEventHandler;
        public event EventHandler<CustomizedEventArgs> EventHandlerWithEventArgs;

       
        public event Action TriggerListeners
        {
            
            add
            {
                triggerListeners += value;
                Console.WriteLine("Aciton was added");
            }
            remove
            {
                triggerListeners -= value;
                Console.WriteLine("Action was removed");

            }
        }
        
        public void CalEventHandlerWithEventArgs()
        {
            Volume vol = new Random().Next() % 2 == 0 ? Volume.Loud : Volume.Louder;
            if (EventHandlerWithEventArgs != null)
            {
                EventHandlerWithEventArgs(this, new CustomizedEventArgs(vol));
            }
        }
        public void BeTippedOver()
        {
            if (TriggerListenersByEventHandler != null) 
                TriggerListenersByEventHandler(this, EventArgs.Empty);
        }
        public void InvokeAction()
        {
            if (triggerListeners != null)
                triggerListeners();
         }
    }
}
