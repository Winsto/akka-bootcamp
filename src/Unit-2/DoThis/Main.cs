using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Akka.Actor;
using Akka.Util.Internal;
using ChartApp.Actors;

namespace ChartApp
{
    public partial class Main : Form
    {
        private IActorRef _chartActor;
        private readonly AtomicCounter _seriesCounter = new AtomicCounter(1);
        private IActorRef coordinatorActor;
        private Dictionary<CounterType, IActorRef> toggleActors =
        	new Dictionary<CounterType, IActorRef>();

        public Main()
        {
            InitializeComponent();
        }

        #region Initialization


        private void Main_Load(object sender, EventArgs e)
        {
            _chartActor = Program.ChartActors.ActorOf(Props.Create(() => new ChartingActor(sysChart)), "charting");
            _chartActor.Tell(new ChartingActor.InitializeChart(null));
            
            coordinatorActor = Program.ChartActors.ActorOf(Props.Create(() =>
				new PerformanceCounterCoordinatorActor(_chartActor)), "counters");
            
            toggleActors[CounterType.Cpu] = Program.ChartActors.ActorOf(
            	Props.Create(() => new ButtonToggleActor(coordinatorActor, cpuSelector, CounterType.Cpu, false)).
            	WithDispatcher("akka.actor.synchronized-dispatcher"));
            
            toggleActors[CounterType.Memory] = Program.ChartActors.ActorOf(
            	Props.Create(() => new ButtonToggleActor(coordinatorActor, memorySelector, CounterType.Memory, false)).
            	WithDispatcher("akka.actor.synchronized-dispatcher"));
            
            toggleActors[CounterType.Disk] = Program.ChartActors.ActorOf(
            	Props.Create(() => new ButtonToggleActor(coordinatorActor, diskSelector, CounterType.Disk, false)).
            	WithDispatcher("akka.actor.synchronized-dispatcher"));
            
            toggleActors[CounterType.Cpu].Tell(new ButtonToggleActor.Toggle());
        }

        #endregion

        private void cpuSelector_Click(object sender, EventArgs e)
        {
        	toggleActors[CounterType.Cpu].Tell(new ButtonToggleActor.Toggle());
        }

        private void memorySelector_Click(object sender, EventArgs e)
        {
        	toggleActors[CounterType.Memory].Tell(new ButtonToggleActor.Toggle());
        }

        private void diskSelector_Click(object sender, EventArgs e)
        {
        	toggleActors[CounterType.Disk].Tell(new ButtonToggleActor.Toggle());
        }
    }
}
