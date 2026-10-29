using Tintelo.iOS.Models.Journal;

namespace Tintelo.iOS.Utils.Extensions;

public static class ActivityIconExtensions
{
	extension(ActivityIcon icon)
	{
		public string ToSymbol() =>
			icon switch
			{
				ActivityIcon.Home => "house.fill",
				ActivityIcon.Family => "person.3.fill",
				ActivityIcon.Work => "briefcase.fill",
				ActivityIcon.Health => "heart.fill",
				ActivityIcon.Sport => "figure.run",
				ActivityIcon.Friends => "person.2.fill",
				ActivityIcon.Hobbies => "gamecontroller.fill",
				ActivityIcon.Food => "fork.knife",
				ActivityIcon.Travel => "airplane",
				ActivityIcon.Creative => "paintpalette.fill",
				ActivityIcon.Learning => "book.fill",
				ActivityIcon.Music => "music.note",
				ActivityIcon.Nature => "leaf.fill",
				ActivityIcon.Pets => "pawprint.fill",
				ActivityIcon.Errands => "cart.fill",
				ActivityIcon.Rest => "bed.double.fill",
				_ => "x.circle"
			};
	}
}