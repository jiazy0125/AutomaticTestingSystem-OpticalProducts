using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutomaticTestingSystem.Framework.CommandBehaviors
{
    /// <summary>
    /// Defines the command behavior binding
    /// </summary>
    public class CommandBehaviorBinding : IDisposable
    {
        #region Properties
        /// <summary>
        /// Get the owner of the CommandBinding ex: a Button
        /// This property can only be set from the BindEvent Method
        /// </summary>
        public DependencyObject Owner { get; private set; }
        /// <summary>
        /// The command to execute when the specified event is raised
        /// </summary>
        public ICommand Command { get; set; }
        /// <summary>
        /// Gets or sets a CommandParameter
        /// </summary>
        public object CommandParameter { get; set; }
        /// <summary>
        /// The event name to hook up to
        /// This property can only be set from the BindEvent Method
        /// </summary>
        public string EventName { get; private set; }
        /// <summary>
        /// The event info of the event
        /// </summary>
        public EventInfo Event { get; private set; }
        /// <summary>
        /// Gets the EventHandler for the binding with the event
        /// </summary>
        public Delegate EventHandler { get; private set; }
        /// <summary>
        /// Pass event arguments the binding event
        /// </summary>
        public bool PassEventArgs { get; set; }

        #endregion

        //Creates an EventHandler on runtime and registers that handler to the Event specified
        public void BindEvent(DependencyObject owner, string eventName)
        {
            EventName = eventName;
            Owner = owner;
            Event = Owner.GetType().GetEvent(EventName, BindingFlags.Public | BindingFlags.Instance);
            if (Event == null)
                throw new InvalidOperationException(string.Format("Could not resolve event name {0}", EventName));

            //Create an event handler for the event that will call the ExecuteCommand method

            EventHandler = Delegate.CreateDelegate(Event.EventHandlerType, this, 
                typeof(CommandBehaviorBinding).GetMethod("ExecuteCommand", BindingFlags.Public | BindingFlags.Instance));
            //Register the handler to the Event
            Event.AddEventHandler(Owner, EventHandler);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        public void ExecuteCommand(object sender, EventArgs e)
        {
            //object sender, EventArgs e
            var parameter = PassEventArgs ? e : CommandParameter;

            if (Command.CanExecute(parameter))
                Command.Execute(parameter);
        }

        #region IDisposable Members
        bool disposed = false;
        /// <summary>
        /// Unregisters the EventHandler from the Event
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                Event.RemoveEventHandler(Owner, EventHandler);
                disposed = true;
            }
        }

        #endregion
    }
}
