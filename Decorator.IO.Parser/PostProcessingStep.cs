using Decorator.IO.Core;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Parser
{
	public class PostProcessingStep
	{
		private readonly DecoratorFile _file;

		private readonly Dictionary<string, Core.DecoratorClass> _classes = new Dictionary<string, Core.DecoratorClass>();

		public PostProcessingStep(DecoratorFile file)
		{
			_file = file;
		}

		public Core.DecoratorFile Process()
		{
			foreach (var @class in _file.Classes)
			{
				Process(@class);
			}

			return new Core.DecoratorFile
			{
				Namespace = _file.Namespace,
				Classes = _classes.Values.ToArray(),
			};
		}

		public Core.DecoratorClass Process(DecoratorClass current)
		{
			foreach (var field in current.Fields)
			{
				Process(field, _file.Namespace);
			}

			if (current.Inherits.Length == 0)
			{
				var record = new Core.DecoratorClass
				{
					Name = current.Name,
					RawName = current.RawName,
					Fields = current.Fields,
					Parents = new Core.DecoratorClass[0]
				};

				_classes[record.Name] = record;

				return record;
			}

			if (_classes.TryGetValue(current.Name, out var result)
				&& result != null)
			{
				return result;
			}

			if (BeingProcessed(current.Name))
			{
				throw new Exception("Recursion.");
			}

			// classify this class as being processed
			_classes[current.Name] = null;

			var processing = new List<string>(current.Inherits);

			// don't need to process the ones we already have
			processing.RemoveAll(x => _classes.ContainsKey(x)
				&& _classes.TryGetValue(x, out var value)
				&& value != null);

			while (processing.Count > 0)
			{
				var index = 0;

				for (index = 0; index < processing.Count; index++)
				{
					if (!BeingProcessed(processing[index]))
					{
						goto foundNonProcessedClass;
					}
				}

				throw new Exception("Recursion.");

			foundNonProcessedClass:

				// process this one

				var process = processing[index];

				_classes[process] = Process(_file.Classes.First(x => x.Name == process));

				processing.Remove(process);
			}

			// now we can compute this class
			_classes[current.Name] = new Core.DecoratorClass
			{
				Name = current.Name,
				RawName = current.RawName,
				Fields = current.Fields,
				Parents = current.Inherits.Select(x => _classes[x]).ToArray()
			};

			return _classes[current.Name];
		}

		public void Process(Core.DecoratorField current, string @namespace)
		{
			if (current.Type is DummyType dummyType)
			{
				dummyType.SetFullName = $"{@namespace}.{dummyType.FullName}";
				current.Type = dummyType;
			}
		}

		private bool BeingProcessed(string name)
			=> _classes.TryGetValue(name, out var value) && value == null;
	}

	public static class PostProcessingExtensions
	{
		public static Core.DecoratorFile Proess(this DecoratorFile file)
			=> new PostProcessingStep(file).Process();
	}
}