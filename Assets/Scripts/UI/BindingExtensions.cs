using RuzikOdyssey.Common;
using System;

namespace RuzikOdyssey.UI
{
	public static class BindingExtensions
	{
		public static void WithFormat(this UiLabelBindingBuilderSyntax builderSyntax, string format)
		{
			builderSyntax.Format = format;
		}

		public static UiLabelBindingBuilderSyntax BindTo<TSource>(this UILabel label, Property<TSource> property)
		{
			var builder = new UiLabelBindingBuilderSyntax(label);
			builder.target.text = property.Value.ToString();
			property.PropertyChanged += 
				(sender, e) => builder.target.text = String.Format(builder.Format, e.PropertyValue.ToString());

			return builder;
		}
	}

	public class UiLabelBindingBuilderSyntax
	{
		private const string DefaultFormat = "{0}";

		public readonly UILabel target;
		public string Format { get; set; } 

		public UiLabelBindingBuilderSyntax(UILabel target)
		{
			this.target = target;
			this.Format = DefaultFormat;
		}
	}
}