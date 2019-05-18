using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Decorator.IO.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Decorator.IO.Providers.CSharp
{
	public class MessageDeserializationSystem
	{
		private readonly NameGenerator _nameGenerator;

		public MessageDeserializationSystem(NameGenerator nameGenerator)
		{
			_nameGenerator = nameGenerator;
		}

		public IEnumerable<MemberDeclarationSyntax> Create(DecoratorClass[] classes)
		{
			return $@"public delegate void {Config.MessageDeserializationEventHandler}({Config.MessageDeserializationServer} {Config.ServerName}, {Config.InterfaceDecoratorObject} {Config.ObjectName});

public class {Config.MessageDeserializationServer}
{{
	{classes.Select(x => $"public event {Config.MessageDeserializationEventHandler} {x.Name};").NewlineAggregate()}

	public void Trigger(string {Config.MessageBaseName}, object[] {Config.ArrayName})
	{{
		switch({Config.MessageBaseName})
		{{
			{classes.Select(x => { var outName = _nameGenerator.Name();
				return $@"
case ""{x.RawName}"":
{{
	var {outName} = {Config.DecoratorFactory}.{Config.DeserializeAsName(x.Name)}({Config.ArrayName});

	if (this.{x.Name} != null)
	{{
		this.{x.Name}.Invoke(this, {outName});
	}}
}}
break;
"; }).NewlineAggregate()}
		}}

		throw new System.Exception(""Expected to be able to deserialize the object array"");
	}}

	public bool TryTrigger(string {Config.MessageBaseName}, object[] {Config.ArrayName})
	{{
		switch({Config.MessageBaseName})
		{{
			{classes.Select(x => {
				var outName = _nameGenerator.Name();
										   return
									   $@"case ""{x.RawName}"":
{{
	if ({Config.DecoratorFactory}.{Config.TryDeserializeAsName(x.Name)}({Config.ArrayName}, out {Config.InterfaceName(x.Name)} {outName}))
	{{
		if (this.{x.Name} != null)
		{{
			this.{x.Name}.Invoke(this, {outName});
		}}

		return true;
	}}

	return false;
}}
";
									   }).NewlineAggregate()}
		}}

		return false;
	}}
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();
		}
	}
}
