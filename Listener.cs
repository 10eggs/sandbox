using System;
using System.Collections.Generic;
using System.Text;

namespace DelegatesLambdasEvents
{
    public class Listener
    {
        public Listener(Subject sub) 
        {
            sub.TriggerListeners += Reaction;
        }

        private void Reaction()
        {
            Console.WriteLine("SO SURPRISED!!!");
        }
    }
}
