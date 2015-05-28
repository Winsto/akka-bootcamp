/*
 * Created by SharpDevelop.
 * User: Neil.Winstanley
 * Date: 28/05/2015
 * Time: 11:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace ChartApp.Actors
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Windows.Forms.DataVisualization.Charting;
	
	using Akka.Actor;
	
	public class PerformanceCounterCoordinatorActor : ReceiveActor
	{
		#region Message Types
		public class Watch
		{
			public Watch(CounterType counter)
			{
				Counter = counter;
			}
			
			public CounterType Counter {get; private set;}
		}
		
		public class UnWatch
		{
			public UnWatch(CounterType counter)
			{
				Counter = counter;
			}
			
			public CounterType Counter {get; private set;}
		}
		#endregion
		
		private static readonly Dictionary<CounterType, Func<PerformanceCounter>> CounterGenerators =
			new Dictionary<CounterType, Func<PerformanceCounter>>()
		{
			{CounterType.Cpu, () => new PerformanceCounter("Processor", "% Processor Time", "_Total", true)},
			{CounterType.Memory, () => new PerformanceCounter("Memory", "% Committed Bytes In Use", true)},
			{CounterType.Disk, () => new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total", true)}
		};
		
		private static readonly Dictionary<CounterType, Func<Series>> CounterSeries =
			new Dictionary<CounterType, Func<Series>>()
		{
			{CounterType.Cpu, () => new Series(CounterType.Cpu.ToString()){ChartType = SeriesChartType.SplineArea, Color = Color.DarkGreen}},
			{CounterType.Memory, () => new Series(CounterType.Memory.ToString()){ChartType = SeriesChartType.FastLine, Color = Color.MediumBlue}},
			{CounterType.Disk, () => new Series(CounterType.Disk.ToString()){ChartType = SeriesChartType.SplineArea, Color = Color.DarkRed}}
		};
		
		private Dictionary<CounterType, IActorRef> counterActors;
		
		private IActorRef chartingActor;
		
		public PerformanceCounterCoordinatorActor(IActorRef chartingActor, Dictionary<CounterType, IActorRef> counterActors)
		{
			this.chartingActor = chartingActor;
			
			this.counterActors = counterActors;
			
			Receive<Watch>(watch => 
			               {
			               	if(!counterActors.ContainsKey(watch.Counter))
			               	{
			               		var counterActor = 
			               			Context.ActorOf(
			               				Props.Create(() => new PerformanceCounterActor(watch.Counter.ToString(), CounterGenerators[watch.Counter])));
			               		
			               		counterActors[watch.Counter] = counterActor;
			               		                                                
			               	}
			               	
			               	chartingActor.Tell(new ChartingActor.AddSeries(CounterSeries[watch.Counter]()));
			               	
			               	counterActors[watch.Counter].Tell(new SubscribeCounter(watch.Counter, chartingActor));
			               });
			
			Receive<UnWatch>(unwatch =>
			                 {
			                 	if(!counterActors.ContainsKey(unwatch.Counter))
			                 		return;
			                 	
			                 	counterActors[unwatch.Counter].Tell(new UnsubscribeCounter(unwatch.Counter, chartingActor));
			                 	
			                 	chartingActor.Tell(new ChartingActor.RemoveSeries(unwatch.Counter.ToString()));
			                 });
		}
	}
}
