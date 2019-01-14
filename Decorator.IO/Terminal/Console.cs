using System;
using System.Diagnostics;
using SConsole = System.Console;

namespace Decorator.IO.Terminal
{
	public static class Console
	{
		public static void WriteLine(string text, ConsoleColor foregroundColor) => InForeground(() => SConsole.WriteLine(text), foregroundColor);
		public static void Write(string text, ConsoleColor foregroundColor) => InForeground(() => SConsole.Write(text), foregroundColor);

		private static void InForeground(Action thing, ConsoleColor fg)
		{
			using (var tfg = new TempForegroundColor(fg))
			{
				thing?.Invoke();
			}
		}
	}

	public class TimedAction : TempAction
	{
		private readonly Stopwatch _stopwatch;

		public TimedAction(string desc, ConsoleColor consoleColor) : base(null, null)
		{
			_stopwatch = new Stopwatch();

			_start = () =>
			{
				Console.Write(desc, consoleColor);
				_stopwatch.Start();
			};

			_end = () =>
			{
				_stopwatch.Stop();
				Console.WriteLine($" {_stopwatch.ElapsedMilliseconds}ms, {_stopwatch.ElapsedTicks} ticks.", ConsoleColor.Magenta);
			};

			Start();
		}
	}

	public class TempForegroundColor : TempAction
	{
		private readonly ConsoleColor _beforeChange;
		private readonly ConsoleColor _consoleColor;

		public TempForegroundColor(ConsoleColor consoleColor) : base(null, null)
		{
			_beforeChange = SConsole.ForegroundColor;
			_consoleColor = consoleColor;

			_start = () => SConsole.ForegroundColor = consoleColor;
			_end = () => SConsole.ForegroundColor = _beforeChange;
		}
	}

	public class TempAction : IDisposable
	{
		protected Action _start;
		protected Action _end;

		public TempAction(Action start, Action end)
		{
			_start = start;
			_end = end;

			Start();
		}

		protected void Start() => _start?.Invoke();

		public void Dispose()
		{
			_end?.Invoke();
		}
	}
}
