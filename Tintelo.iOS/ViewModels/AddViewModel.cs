using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Services;

namespace Tintelo.iOS.ViewModels;

public partial class AddViewModel(
	INavigator navigator,
	CurrentDay currentDay) : ObservableObject
{
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Title))]
	public partial DateOnly? Date { get; set; }


	public string Title =>
		Date is DateOnly day
			? day.ToString(Texts.DateTime_Format_yMMMMd, CultureInfo.CurrentCulture)
			: Texts.AddEntry;


	[RelayCommand]
	Task ShowAsync()
	{
		Date = currentDay.Value;

		return navigator.PresentAsync<AddViewModel>(ModalStyle.FormSheet);
	}
}
