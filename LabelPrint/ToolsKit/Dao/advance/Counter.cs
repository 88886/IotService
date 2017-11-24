using System;
using System.Diagnostics;

namespace PrintX.Dev.Utils.ToolsKit
{
	internal sealed class Counter
	{
		private string categoryName;

		private string instanceName;

		private string counterName;

		private PerformanceCounter counter;

		public string CounterName
		{
			get
			{
				return this.counterName;
			}
		}

		public long Value
		{
			get
			{
				return (this.counter != null) ? this.counter.RawValue : 0L;
			}
			set
			{
				if (this.counter != null)
				{
					this.counter.RawValue = value;
				}
			}
		}

		internal Counter(string categoryName, string instanceName, string counterName)
		{
			this.categoryName = categoryName;
			this.instanceName = instanceName;
			this.counterName = counterName;
		}

		public void Start(string categoryName)
		{
			if (this.counter == null)
			{
				this.counter = new PerformanceCounter();
				if (categoryName != null)
				{
					this.categoryName = categoryName;
				}
				this.counter.CategoryName = this.categoryName;
				this.counter.CounterName = this.counterName;
				this.counter.InstanceName = this.instanceName;
				this.counter.InstanceLifetime = PerformanceCounterInstanceLifetime.Process;
				this.counter.ReadOnly = false;
				this.counter.RawValue = 0L;
			}
		}

		public void Increment()
		{
			if (this.counter != null)
			{
				this.counter.Increment();
			}
		}

		public void Decrement()
		{
			if (this.counter != null)
			{
				this.counter.Decrement();
			}
		}

		internal void Dispose()
		{
			PerformanceCounter performanceCounter = this.counter;
			this.counter = null;
			if (performanceCounter != null)
			{
				performanceCounter.RemoveInstance();
			}
		}
	}
}
