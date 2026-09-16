using CommunityToolkit.Mvvm.ComponentModel;
using SkeleKit;
using Tintelo.iOS.Services;

namespace Tintelo.iOS.Models.Config;

public partial class AppThemeConfig : ObservableObject
{
	public AppThemeConfig(SimpleStorage storage) =>
		InitializeStoredProperties(storage);
	
	
	[ObservableProperty]
	[StoreAs("configuration.theme.appearance", Appearance.System)]
	public partial Appearance Appearance { get; set; }

	[ObservableProperty]
	[StoreAs("configuration.theme.accent", Accent.Default)]
	public partial Accent Accent { get; set; }

	[ObservableProperty]
	[StoreAs("configuration.theme.inlinetitles", true)]
	public partial bool InlineTitles { get; set; }

	[ObservableProperty]
	[StoreAs("configuration.theme.softscrolledge", false)]
	public partial bool SoftScrollEdge { get; set; }


	public void ApplyAppearance()
	{
		SkeleApplication.Current?.Theme.Appearance = Appearance;
	}
	
	public void ApplyAccent()
	{
		SkeleApplication.Current?.Theme.Tint = Accent switch
		{
			Accent.Default => Color.Dynamic(Color.FromHex(0x658631), Color.FromHex(0xb9cc7a)),
			Accent.Red => Color.Dynamic(Color.FromHex(0xe15b5b), Color.FromHex(0xff7b7b)),
			Accent.Orange => Color.Dynamic(Color.FromHex(0xf9833f), Color.FromHex(0xff8c2a)),
			Accent.Yellow => Color.Dynamic(Color.FromHex(0xe59b24), Color.FromHex(0xfde047)),
			Accent.Green => Color.Dynamic(Color.FromHex(0x48a968), Color.FromHex(0x6ee7b7)),
			Accent.Blue => Color.Dynamic(Color.FromHex(0x007ff5), Color.FromHex(0x1daeff)),
			Accent.Purple  => Color.Dynamic(Color.FromHex(0x9061f9), Color.FromHex(0xc084fc)),
			Accent.Pink => Color.Dynamic(Color.FromHex(0xed5e93), Color.FromHex(0xf472b6)),
			Accent.Gray => Color.Dynamic(Color.FromHex(0x64748b), Color.FromHex(0x94a3b8)),
			_ => throw new ArgumentOutOfRangeException(nameof(Accent))
		};
	}

	public void ApplyInlineTitles()
	{
		SkeleApplication.Current?.Theme.NavigationTitleStyle = InlineTitles
			? TitleStyle.Inline
			: TitleStyle.Large;
	}

	public void ApplySoftScrollEdge()
	{
		SkeleApplication.Current?.Theme.TopScrollEdgeStyle = SoftScrollEdge
			? ScrollEdgeStyle.Soft
			: ScrollEdgeStyle.Automatic;
	}


	partial void OnAppearanceChanged(Appearance value) =>
		ApplyAppearance();

	partial void OnAccentChanged(Accent value) =>
		ApplyAccent();

	partial void OnInlineTitlesChanged(bool value) =>
		ApplyInlineTitles();
	
	partial void OnSoftScrollEdgeChanged(bool value) =>
		ApplySoftScrollEdge();
}