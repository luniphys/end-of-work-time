using flextime_calculator.Commands;
using flextime_calculator.Constants;
using System.Windows.Input;

namespace flextime_calculator.ViewModels;


public sealed class SetupViewModel : ViewModelBase
{
    #region Private fields

    // Backing fields
    private string _nextButtonText    = "Nächste";
    private bool _backButtonEnabled   = false;
    private double _backButtonOpacity = 0.5;
    private Color _nextButtonColor;

    private TimeSpan _setupCome            = new TimeSpan(6, 0, 0);
    private TimeSpan _setupGo              = new TimeSpan(14, 15, 0);
    private string _setupWeeklyHours       = "37";
    private string _setupWeeklyMinutes     = "30";
    private TimeSpan _setupSmallBreakStart = new TimeSpan(9, 0, 0);
    private TimeSpan _setupSmallBreakEnd   = new TimeSpan(9, 15, 0);
    private TimeSpan _setupMainBreakStart  = new TimeSpan(12, 0, 0);
    private TimeSpan _setupMainBreakEnd    = new TimeSpan(12, 30, 0);

    private bool _comeGridVisibility       = true;
    private bool _goGridVisibility         = false;
    private bool _weeklyGridVisibility     = false;
    private bool _smallBreakGridVisibility = false;
    private bool _mainBreakGridVisibility  = false;

    // Other
    private int _pageIndex  = 0;
    private Color _vsPurple = Color.FromRgb(80, 43, 212);

    private const int GridCount = 5;

    #endregion



    public SetupViewModel()
	{
        _nextButtonColor = _vsPurple;

        NextCommand = new RelayCommand(
            execute:    _ => Next(),
            canExecute: _ => true);

        BackCommand = new RelayCommand(
            execute: _ => Back(),
            canExecute: _ => _backButtonEnabled);
    }



    #region Public Properties

    public event Action? SetupCompleted;

    // Commands
    public ICommand NextCommand { get; }
    public ICommand BackCommand { get; }


    // Other
    public string NextButtonText
    {
        get => _nextButtonText;
        set => SetField(ref _nextButtonText, value);
    }
    public Color NextButtonColor
    {
        get => _nextButtonColor;
        set => SetField(ref _nextButtonColor, value);
    }
    public double BackButtonOpacity
    {
        get => _backButtonOpacity;
        set => SetField(ref _backButtonOpacity, value);
    }

    public TimeSpan SetupCome
    {
        get => _setupCome;
        set => SetField(ref _setupCome, value);
    }
    public TimeSpan SetupGo
    {
        get => _setupGo;
        set => SetField(ref _setupGo, value);
    }
    public string SetupWeeklyHours
    {
        get => _setupWeeklyHours;
        set => SetField(ref _setupWeeklyHours, value);
    }
    public string SetupWeeklyMinutes
    {
        get => _setupWeeklyMinutes;
        set => SetField(ref _setupWeeklyMinutes, value);
    }
    public TimeSpan SetupSmallBreakStart
    {
        get => _setupSmallBreakStart;
        set => SetField(ref _setupSmallBreakStart, value);
    }
    public TimeSpan SetupSmallBreakEnd
    {
        get => _setupSmallBreakEnd;
        set => SetField(ref _setupSmallBreakEnd, value);
    }
    public TimeSpan SetupMainBreakStart
    {
        get => _setupMainBreakStart;
        set => SetField(ref _setupMainBreakStart, value);
    }
    public TimeSpan SetupMainBreakEnd
    {
        get => _setupMainBreakEnd;
        set => SetField(ref _setupMainBreakEnd, value);
    }

    public bool ComeGridVisibility
    {
        get => _comeGridVisibility;
        set => SetField(ref _comeGridVisibility, value);
    }
    public bool GoGridVisibility
    {
        get => _goGridVisibility;
        set => SetField(ref _goGridVisibility, value);
    }
    public bool WeeklyGridVisibility
    {
        get => _weeklyGridVisibility;
        set => SetField(ref _weeklyGridVisibility, value);
    }
    public bool SmallBreakGridVisibility
    {
        get => _smallBreakGridVisibility;
        set => SetField(ref _smallBreakGridVisibility, value);
    }
    public bool MainBreakGridVisibility
    {
        get => _mainBreakGridVisibility;
        set => SetField(ref _mainBreakGridVisibility, value);
    }

    #endregion



    #region Private (helper) methods

    private void Next()
    {
        if (_pageIndex < GridCount)
        {
            _pageIndex++;
            UpdatePage();
        }
        
    }

    private void Back()
    {
        if (_pageIndex > 0)
        {
            _pageIndex--;
            UpdatePage();
        }
    }


    /// <summary>
    /// Updates the enabled and visibility state of UI grids based on the current page index.
    /// </summary>
    private void UpdatePage()
    {
        ComeGridVisibility       = _pageIndex == 0;
        GoGridVisibility         = _pageIndex == 1;
        WeeklyGridVisibility     = _pageIndex == 2;
        SmallBreakGridVisibility = _pageIndex == 3;
        MainBreakGridVisibility  = _pageIndex == 4;

        _backButtonEnabled       = _pageIndex > 0;
        BackButtonOpacity        = _pageIndex == 0 ? 0.5 : 1;

        ((RelayCommand)BackCommand).NotifyCanExecuteChanged();
        
        NextButtonColor = _pageIndex == GridCount - 1 ? Colors.Green : _vsPurple;
        NextButtonText  = _pageIndex == GridCount - 1 ? "Fertig" : "Nächste";

        if (_pageIndex == GridCount)
        {
            SaveSettings();
        }
    }


    /// <summary>
    /// Saves setup preferences and closes the setup modal.
    /// </summary>
    private void SaveSettings()
    {
        Preferences.Set(PreferenceKeys.SettingsCome, SetupCome.ToString());
        Preferences.Set(PreferenceKeys.SettingsGo, SetupGo.ToString());
        Preferences.Set(PreferenceKeys.SettingsWeeklyHours, SetupWeeklyHours);
        Preferences.Set(PreferenceKeys.SettingsWeeklyMinutes, SetupWeeklyMinutes);
        Preferences.Set(PreferenceKeys.SettingsSmallBreakStart, SetupSmallBreakStart.ToString());
        Preferences.Set(PreferenceKeys.SettingsSmallBreakEnd, SetupSmallBreakEnd.ToString());
        Preferences.Set(PreferenceKeys.SettingsMainBreakStart, SetupMainBreakStart.ToString());
        Preferences.Set(PreferenceKeys.SettingsMainBreakEnd, SetupMainBreakEnd.ToString());

        Preferences.Set(PreferenceKeys.SetupComplete, true);

        SetupCompleted?.Invoke();
    }

    #endregion
}
