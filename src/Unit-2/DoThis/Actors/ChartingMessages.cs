namespace ChartApp.Actors
{
    using Akka.Actor;

    	public class GatherMetrics { }

    	public class Metric
    	{
    		public Metric(string seriesName, float counterValue)
    		{
    			SeriesName = seriesName;

    			CounterValue = counterValue;
    		}

    		public string SeriesName{get; private set;}

    		public float CounterValue { get; private set;  }
    	}

    	public enum CounterType
    	{
    		Cpu,
    		Memory,
    		Disk
    	}

    	public class SubscribeCounter
    	{
    		public SubscribeCounter(CounterType counter, IActorRef subscriber)
    		{
    			Counter = counter;

    			Subscriber = subscriber;
    		}

    		public CounterType Counter
    		{ get; private set; }

    		public IActorRef Subscriber
    		{ get; private set; }
    	}

    	public class UnsubscribeCounter
    	{
    		public UnsubscribeCounter(CounterType counter, IActorRef subscriber)
    		{
    			Subscriber = subscriber;
    			Counter = counter;
    		}

    		public CounterType Counter
    		{ get; private set; }

    		public IActorRef Subscriber
    		{ get; private set; }
    	}
}
