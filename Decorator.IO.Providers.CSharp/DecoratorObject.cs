﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Decorator.IO.Providers.CSharp
{
	public static class DecoratorObject
	{
		public static IEnumerable<MemberDeclarationSyntax> Create()
			=> $@"public interface {Config.DecoratorName}
{{
	object[] Serialize();
}}"
			.AsCompilationUnitSyntax()
			.AsMemberDeclarationSyntaxes();
	}
}