using System.Globalization;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Utils;

namespace Tintelo.iOS.ViewModels.About;

public class AboutTermsViewModel
{
	static readonly DateTime EffectiveDate = new(2026, 9, 13);

	
	static Span Title(string content) =>
		new(content + '\n')
		{
			TextStyle = TextStyle.Title1,
			FontWeight = FontWeight.Semibold
		};
	static Span Body(string content) =>
		new(content+ '\n')
		{
			TextStyle = TextStyle.Body,
			TextAlignment = TextAlignment.Justified
		};
	
	static Span Footnote(string content) =>
		new(content+ '\n')
		{
			TextColor = Colors.SecondaryLabel,
			TextStyle = TextStyle.Footnote,
			TextAlignment = TextAlignment.Justified
		};
	
	static Span NewLine() =>
		new("\n")
		{
			TextStyle = TextStyle.Title1
		};
	static Span NewLineSmol() =>
		new("\n")
		{
			FontSize = 6
		};
	
	
	public Span[] Text { get; } =
	[
		Footnote(Texts.About_Terms_0_Intoduction.Format(EffectiveDate.ToString(Texts.DateTime_Format_yMMMMd, CultureInfo.InvariantCulture))),
		NewLine(),
		Title(Texts.About_Terms_1_Use),
		NewLineSmol(),
		Body(Texts.About_Terms_1_Use_Content),
		NewLine(),
		Title(Texts.About_Terms_2_Privacy),
		NewLineSmol(),
		Body(Texts.About_Terms_2_Privacy_Content),
		NewLine(),
		Title(Texts.About_Terms_3_Trends),
		NewLineSmol(),
		Body(Texts.About_Terms_3_Trends_Content),
		NewLine(),
		Title(Texts.About_Terms_4_Permissions),
		NewLineSmol(),
		Body(Texts.About_Terms_4_Permissions_Content),
		NewLine(),
		Title(Texts.About_Terms_5_Managing),
		NewLineSmol(),
		Body(Texts.About_Terms_5_Managing_Content),
		NewLine(),
		Title(Texts.About_Terms_6_Health),
		NewLineSmol(),
		Body(Texts.About_Terms_6_Health_Content),
		NewLine(),
		Title(Texts.About_Terms_7_Purchases),
		NewLineSmol(),
		Body(Texts.About_Terms_7_Purchases_Content),
		NewLine(),
		Title(Texts.About_Terms_8_Security),
		NewLineSmol(),
		Body(Texts.About_Terms_8_Security_Content),
		NewLine(),
		Title(Texts.About_Terms_9_Liability),
		NewLineSmol(),
		Body(Texts.About_Terms_9_Liability_Content),
		NewLine(),
		Title(Texts.About_Terms_10_Government),
		NewLineSmol(),
		Body(Texts.About_Terms_10_Government_Content),
		NewLine(),
		Footnote(Texts.About_Terms_11_End.Format(AboutViewModel.ContactEmail))
	];
}