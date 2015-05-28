/*
 * Created by SharpDevelop.
 * User: Neil.Winstanley
 * Date: 28/05/2015
 * Time: 12:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace ChartApp.Actors
{
	using System.Windows.Forms;
	
	using Akka.Actor;
	
	public class ButtonToggleActor:UntypedActor
	{
		#region Message Types
		public class Toggle {}
		#endregion
		
		private readonly CounterType myCounterType;
		private bool isToggledOn;
		private readonly Button myButton;
		private readonly IActorRef coordinatorActor;
		
		public ButtonToggleActor(IActorRef coordinatorActor, Button myButton, CounterType myCounterType, bool isToggledOn = false)
		{
			this.coordinatorActor = coordinatorActor;
			this.myButton = myButton;
			this.isToggledOn = isToggledOn;
			this.myCounterType = myCounterType;
		}
		
		protected override void OnReceive(object message)
		{
			if(message is Toggle && isToggledOn)
			{
				coordinatorActor.Tell( new PerformanceCounterCoordinatorActor.UnWatch(myCounterType));
				
				FlipToggle();
			}
			else if(message is Toggle && !isToggledOn)
			{
				coordinatorActor.Tell(new PerformanceCounterCoordinatorActor.Watch(myCounterType));
				
				FlipToggle();
			}
			else
			{
				Unhandled(message);
			}
		}
		
		private void FlipToggle()
		{
			isToggledOn = ! isToggledOn;
			
			myButton.Text = String.Format("{0} ({1})", myCounterType.ToString().ToUpperInvariant(),
			                              isToggledOn ? "ON" : "OFF");
		}
	}
}
