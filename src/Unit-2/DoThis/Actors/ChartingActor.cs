using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using Akka.Actor;

namespace ChartApp.Actors
{
    public class ChartingActor : ReceiveActor
    {
    	public const int MaxPoints = 250;
    	
    	private int xPosCounter = 0;
    	
        #region Messages
        
        public class TogglePause{}

        public class RemoveSeries
        {
        	public RemoveSeries(string seriesName)
        	{
        		SeriesName = seriesName;
        	}
        	
        	public string SeriesName {get; private set;}
        }
        
        public class InitializeChart
        {
            public InitializeChart(Dictionary<string, Series> initialSeries)
            {
                InitialSeries = initialSeries;
            }

            public Dictionary<string, Series> InitialSeries { get; private set; }
        }

        public class AddSeries
        {
            public AddSeries(Series series)
            {
                Series = series;
            }

            public Series Series { get; private set; }
        }

        #endregion

        private readonly Chart _chart;
        private Dictionary<string, Series> _seriesIndex;
        private readonly Button pauseButton;

        public ChartingActor(Chart chart, Button pauseButton) : this(chart, new Dictionary<string, Series>(), pauseButton)
        {
        }

        public ChartingActor(Chart chart, Dictionary<string, Series> seriesIndex, Button pauseButton)
        {
            _chart = chart;
            _seriesIndex = seriesIndex;
            this.pauseButton = pauseButton;

            Charting();
        }
        
        private void Charting()
        {
            Receive<InitializeChart>(message => HandleInitialize(message));
            Receive<AddSeries>(message => HandleAddSeries(message));
        	Receive<RemoveSeries>(removeSeriesMessage => HandleRemoveSeries(removeSeriesMessage));
        	Receive<Metric>(metricMessage => HandleMetrics(metricMessage));
        	
        	Receive<TogglePause>(message => 
        	                     {
        	                     	SetPauseButtonText(true);
        	                     	BecomeStacked(Paused);
        	                     });
        }
        
        private void Paused()
        {
        	Receive<Metric>(message => HandleMetricsPaused(message));
        	Receive<TogglePause>(message =>
        	                     {
        	                     	SetPauseButtonText(false);
        	                     	UnbecomeStacked();
        	                     });
        }

        #region Individual Message Type Handlers
        
        private void HandleMetricsPaused(Metric metric)
        {
        	if(!string.IsNullOrEmpty(metric.SeriesName) && _seriesIndex.ContainsKey(metric.SeriesName))
        	{
        		var series = _seriesIndex[metric.SeriesName];
        		series.Points.AddXY(xPosCounter++, 0.0d); // Set Y value to zero when paused
        		while(series.Points.Count > MaxPoints)
        			series.Points.RemoveAt(0);
        		
        		SetChartBoundaries();
        	}
        }
		
        private void HandleInitialize(InitializeChart ic)
        {
        	if(ic.InitialSeries != null)
        	{
        		_seriesIndex = ic.InitialSeries;
        	}
        	
        	_chart.Series.Clear();
        	
        	var area = _chart.ChartAreas[0];
        	area.AxisX.IntervalType = DateTimeIntervalType.Number;
        	area.AxisY.IntervalType = DateTimeIntervalType.Number;
        	
        	SetChartBoundaries();
        	
        	if(_seriesIndex.Any())
        	{
        		foreach(var series in _seriesIndex)
        		{
        			series.Value.Name = series.Key;
        			_chart.Series.Add(series.Value);
        		}
        	}
        	
        	SetChartBoundaries();
        }
        
        private void HandleAddSeries(AddSeries series)
        {
        	if(!string.IsNullOrEmpty(series.Series.Name) && !_seriesIndex.ContainsKey(series.Series.Name))
        	{
        		_seriesIndex.Add(series.Series.Name, series.Series);
        		_chart.Series.Add(series.Series);
        		SetChartBoundaries();
        	}
        }
        
        private void HandleRemoveSeries(RemoveSeries series)
        {
        	if(!string.IsNullOrEmpty(series.SeriesName) && _seriesIndex.ContainsKey(series.SeriesName))
        	{
        		var seriesToRemove = _seriesIndex[series.SeriesName];
        		_seriesIndex.Remove(series.SeriesName);
        		_chart.Series.Remove(seriesToRemove);
        		SetChartBoundaries();
        	}
        }
        
        private void HandleMetrics(Metric metric)
        {
        	if(!string.IsNullOrEmpty(metric.SeriesName) && _seriesIndex.ContainsKey(metric.SeriesName))
        	{
        		var series = _seriesIndex[metric.SeriesName];
        		series.Points.AddXY(xPosCounter++, metric.CounterValue);
        		while(series.Points.Count > MaxPoints) series.Points.RemoveAt(0);
        		SetChartBoundaries();
        	}
        }

        #endregion
        
        private void SetChartBoundaries()
        {
		    double maxAxisX, maxAxisY, minAxisX, minAxisY = 0.0d;
		    var allPoints = _seriesIndex.Values.Aggregate(new HashSet<DataPoint>(),
		            (set, series) => new HashSet<DataPoint>(set.Concat(series.Points)));
		    var yValues = allPoints.Aggregate(new List<double>(), (list, point) => list.Concat(point.YValues).ToList());
		    maxAxisX = xPosCounter;
		    minAxisX = xPosCounter - MaxPoints;
		    maxAxisY = yValues.Count > 0 ? Math.Ceiling(yValues.Max()) : 1.0d;
		    minAxisY = yValues.Count > 0 ? Math.Floor(yValues.Min()) : 0.0d;
		    if (allPoints.Count > 2)
		    {
		        var area = _chart.ChartAreas[0];
		        area.AxisX.Minimum = minAxisX;
		        area.AxisX.Maximum = maxAxisX;
		        area.AxisY.Minimum = minAxisY;
		        area.AxisY.Maximum = maxAxisY;
        	}
    	}
        
        private void SetPauseButtonText(bool paused)
        {
        	pauseButton.Text = string.Format("{0}", !paused ? "PAUSE ||" : "RESUME ->");
        }
    }
}


