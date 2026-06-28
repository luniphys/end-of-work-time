using flextime_calculator.Constants;
using flextime_calculator.ViewModels;

namespace flextime_calculator.Views;


public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;


    public MainPage()
	{
		InitializeComponent();
        _vm = new MainViewModel();
        BindingContext = _vm;

        _vm.SettingsPanelToggled += OnSettingsPanelToggled;
	}

    
    /// <summary>
    /// Called when the mainpage appears. Displays the first-time setup page if setup has not been completed yet.
    /// Otherwise loads settings.
    /// </summary>
    protected override async void OnAppearing() // ContentPage Lifecycle: Constructor -> OnAppearing -> OnDisappearing -> Destructor
    {
        base.OnAppearing();

        //Preferences.Clear(); // Uncomment for FirstTimeSetupPage access

        if (!Preferences.ContainsKey(PreferenceKeys.SetupComplete))
        {
            await Navigation.PushModalAsync(new FirstTimeSetupPage(), animated: true);
            return;
        }

        _vm.LoadSettings();
    }


    /// <summary>
    /// Handles slide in/out animation of the settingspanel.
    /// </summary>
    /// <param name="isOpening">state of the settingspanel: open/closed</param>
    private void OnSettingsPanelToggled(bool isOpening)
    {
        double panelWidthRatio = 0.85;
        uint animationDuration = 100;

        if (isOpening)
        {
            SettingsPanel.IsVisible = true;
            SettingsPanel.Animate("open",
                v => SettingsPanel.WidthRequest = v,
                start: 0,
                end: this.Width * panelWidthRatio,
                length: animationDuration);
        }
        else
        {
            SettingsPanel.Animate("close",
                v => SettingsPanel.WidthRequest = v,
                start: this.Width * panelWidthRatio,
                end: 0,
                length: animationDuration,
                finished: (v, c) => SettingsPanel.IsVisible = false);
        }
    }


    /// <summary>
    /// Clicking an Entry selects all text.
    /// </summary>
    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry)
        {
            Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(50), selectAll);

            void selectAll()
            {
                entry.CursorPosition = 0;
                entry.SelectionLength = entry.Text?.Length ?? 0;
            }
        }
    }
}
