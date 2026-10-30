using System.ComponentModel;
using SkeleKit;
using Tintelo.iOS.Localization;
using Tintelo.iOS.Models.Settings;
using Tintelo.iOS.ViewModels.Settings;
using Tintelo.iOS.Views.Settings.Cells;

namespace Tintelo.iOS.Views.Settings;

[Page(Singleton = true)]
public class SettingsView : ContentView<SettingsViewModel>
{
	public SettingsView(SettingsViewModel viewModel) : base(viewModel)
	{
		viewModel.ActivateCommand.PropertyChanged += OnActivateCommandPropertyChanged;
		
		Title = Texts.Settings;
		Background = Colors.GroupedBackground;
		
		Content = new CollectionView<SettingsEntry, SettingsSection>
		{
			Layout = CollectionLayout.List(true),
			SeparatorInsets = new(56, 0, 12, 0),
			RetainsHighlight = false,
			
			Header = new AboutHeader()
			{
				Margin = new(0, 0, 0, 20),
				
				TapCommand = viewModel.ShowAboutCommand,
				IsEnabled = Bind(vm => vm.ActivateCommand)
					.Path(command => command.IsRunning)
					.ConvertTo(isRunning => !isRunning)
			},
			
			SectionHeaderTemplate = static () => new SettingsSectionHeaderCell(),
			ItemTemplateSelector = new ItemTemplateSelector<SettingsEntry>()
				.Add(static () => new SettingsActionEntryCell())
				.Add(static () => new SettingsDisplayEntryCell())
				.Add(static () => new SettingsNavigationEntryCell())
				.Add(static () => new SettingsToggleEntryCell())
				.Add(static () => new SettingsPickerEntryCell())
				.Add(static () => new SettingsTimeEntryCell()),
			
			GroupedItemsSource = Bind(vm => vm.Sections),
			ItemCommand = viewModel.ActivateCommand
		};
	}

	
	void OnActivateCommandPropertyChanged(object? _, PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
			case nameof(SettingsViewModel.ActivateCommand.IsRunning):
				ConfirmLeave = ViewModel.ActivateCommand.IsRunning ? () => Task.FromResult(false) : null;
				break;
		}
	}
}
