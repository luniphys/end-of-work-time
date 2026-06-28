using flextime_calculator.ViewModels;

namespace flextime_calculator.Views;

public partial class FirstTimeSetupPage : ContentPage
{
    public FirstTimeSetupPage()
	{
		InitializeComponent();

        SetupViewModel vm = new SetupViewModel();

        vm.SetupCompleted += async () => await Navigation.PopModalAsync();

        BindingContext = vm;
    }


    /// <summary>
    /// Clicking Entry selects all text.
    /// </summary>
    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry)
        {
            Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(50), () =>
            {
                entry.CursorPosition = 0;
                entry.SelectionLength = entry.Text?.Length ?? 0;
            });
        }
    }
}