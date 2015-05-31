namespace ChartApp.Actors
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using Akka.Actor;
	
	public class PerformanceCounterActor : UntypedActor
	{
		private readonly string seriesName;
		private readonly Func<PerformanceCounter> performanceCounterGenerator;
		private PerformanceCounter counter;
		
		private readonly HashSet<IActorRef> subscriptions;
		private readonly ICancelable cancelPublishing;
		
		public PerformanceCounterActor(string seriesName, Func<PerformanceCounter> performanceCounterGenerator)
		{
			this.seriesName = seriesName;
			this.performanceCounterGenerator = performanceCounterGenerator;
			subscriptions = new HashSet<IActorRef>();
			cancelPublishing = new Cancelable(Context.System.Scheduler);
		}
		
		protected override void PreStart()
		{
			counter = performanceCounterGenerator();
			
			Context.System.Scheduler.ScheduleTellRepeatedly(
				initialDelay: TimeSpan.FromMilliseconds(250),
				interval: TimeSpan.FromMilliseconds(250), 
				receiver: Self, 
				message: new GatherMetrics(), 
				sender: Self, 
				cancelable: cancelPublishing);
		}
		
		protected override void PostStop()
		{
			try
			{
				cancelPublishing.Cancel(false);
				counter.Dispose();
			}
			catch{} // Ignore any other dispose exceptions
			finally
			{
				base.PostStop();
			}
		}
		
		protected override void OnReceive(object message)
		{
			if(message is GatherMetrics)
			{
				var metric = new Metric(seriesName, counter.NextValue());
				
				foreach(var subscription in subscriptions)
					subscription.Tell(metric);
			}
			else if(message is SubscribeCounter)
			{
				var sc = message as SubscribeCounter;
				subscriptions.Add(sc.Subscriber);
			}
			else if(message is UnsubscribeCounter)
			{
				var uc = message as UnsubscribeCounter;
				
				subscriptions.Remove(uc.Subscriber);
			}
		}
	}
}
