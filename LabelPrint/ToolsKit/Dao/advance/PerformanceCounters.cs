
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PrintX.Dev.Utils.ToolsKit
{
	internal sealed class PerformanceCounters
	{
		private static bool started;

		private static System.Collections.Generic.IList<Counter> counters = new System.Collections.Generic.List<Counter>();

		private static CounterCreationDataCollection countersData = new CounterCreationDataCollection();

		private PerformanceCounters()
		{
		}

		public static void Start(string categoryName, string categoryHelp)
		{
			if (!PerformanceCounters.started)
			{
				System.AppDomain.CurrentDomain.DomainUnload += new System.EventHandler(PerformanceCounters.UnloadEventHandler);
				System.AppDomain.CurrentDomain.ProcessExit += new System.EventHandler(PerformanceCounters.ExitEventHandler);
				System.AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler(PerformanceCounters.ExceptionEventHandler);
				PerformanceCounters.EnusreCategory(categoryName, categoryHelp);
				foreach (Counter current in PerformanceCounters.counters)
				{
					current.Start(categoryName);
				}
				PerformanceCounters.started = true;
			}
		}

		private static void EnusreCategory(string categoryName, string categoryHelp)
		{
			if (PerformanceCounterCategory.Exists(categoryName))
			{
				bool flag = true;
				foreach (CounterCreationData counterCreationData in PerformanceCounters.countersData)
				{
					if (!PerformanceCounterCategory.CounterExists(counterCreationData.CounterName, categoryName))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return;
				}
			
				PerformanceCounterCategory.Delete(categoryName);
			}
			
			PerformanceCounterCategory.Create(categoryName, categoryHelp, PerformanceCounterCategoryType.MultiInstance, PerformanceCounters.countersData);
		}

		private static string GetInstanceName()
		{
			return "_Total";
		}

		public static Counter Create(string counterName, string counterHelp)
		{
			Counter counter = new Counter(null, PerformanceCounters.GetInstanceName(), counterName);
			PerformanceCounters.counters.Add(counter);
			CounterCreationData value = new CounterCreationData(counterName, counterHelp, PerformanceCounterType.NumberOfItems32);
			PerformanceCounters.countersData.Add(value);
			return counter;
		}

		private static void UnloadEventHandler(object sender, System.EventArgs e)
		{
			PerformanceCounters.Dispose();
		}

		private static void ExitEventHandler(object sender, System.EventArgs e)
		{
			PerformanceCounters.Dispose();
		}

		private static void ExceptionEventHandler(object sender, System.UnhandledExceptionEventArgs e)
		{
			if (e != null && e.IsTerminating)
			{
				PerformanceCounters.Dispose();
			}
		}

		private static void Dispose()
		{
			foreach (Counter current in PerformanceCounters.counters)
			{
				current.Dispose();
			}
			PerformanceCounters.counters.Clear();
		}
	}
}
