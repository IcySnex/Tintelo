using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using Tintelo.iOS.Localization;

namespace Tintelo.iOS.ViewModels.Calendar;

public partial class CalendarDetailsViewModel : ObservableObject
{
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Title))]
	public partial DateOnly? Date { get; set; }


	public string Title => Date?.ToString(Texts.DateTime_Format_yMMMMd, CultureInfo.CurrentCulture) ?? Texts.Calendar_Details;
}
