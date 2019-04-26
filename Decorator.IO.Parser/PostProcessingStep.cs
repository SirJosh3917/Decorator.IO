using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			var classes = new List<Core.DecoratorClass>();

			foreach(var @class in _file.Classes)
			{
				classes.Add(Process(@class));
			}

			return new Core.DecoratorFile
			{
				Namespace = _file.Namespace,
				Classes = classes.ToArray(),
			};
		}

		public Core.DecoratorClass Process(DecoratorClass current)
		{
			if (current.Inherits.Length == 0)
			{
				return new Core.DecoratorClass
				{
					Name = current.Name,
					Fields = current.Fields,
					Parents = new Core.DecoratorClass[0]
				};
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

			while(processing.Count > 0)
			{
				var index = 0;

				for(index = 0; index < processing.Count; index++)
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
				Fields = current.Fields,
				Parents = current.Inherits.Select(x => _classes[x]).ToArray()
			};

			return _classes[current.Name];
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
