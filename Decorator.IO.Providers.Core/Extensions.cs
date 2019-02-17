using System.Collections.Generic;
using System.Text;

namespace Decorator.IO.Providers.Core
{
	public static class Extensions
	{
		public static IEnumerable<StringBuilder> Process
		(
			this IStringProcess stringProcess,
			IGenerator generator
		)
			=> stringProcess
			.Process
			(
				generator
				.Generate()
			);

		public static IEnumerable<StringBuilder> Process
		(
			this IStringProcess stringProcess,
			IEnumerable<GeneratorItem> generatorItems
		)
			=> stringProcess
			.Process
			(
				generatorItems
				.GenerateStringBuilders()
			);

		public static IEnumerable<StringBuilder> GenerateStringBuilders
		(
			this IGenerator generator
		)
			=> generator
				.Generate()
				.GenerateStringBuilders();

		public static IEnumerable<StringBuilder> GenerateStringBuilders
		(
			this IEnumerable<GeneratorItem> generatorItems
		)
		{
			foreach (var item in generatorItems)
			{
				yield return item;
			}
		}
	}
}