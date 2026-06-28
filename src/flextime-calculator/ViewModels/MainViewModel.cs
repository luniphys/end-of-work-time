using System.Windows.Input;
using flextime_calculator.Commands;
using flextime_calculator.Models;

namespace flextime_calculator.ViewModels;


public sealed class MainViewModel : ViewModelBase
{
    #region Private fields

    private string _feierabendWeek = string.Empty;
    private string _feierabendDay  = string.Empty;

    // Delta times
    private string _dayDeltaMonday    = string.Empty;
    private string _dayDeltaTuesday   = string.Empty;
    private string _dayDeltaWednesday = string.Empty;
    private string _dayDeltaThursday  = string.Empty;
    private string _cumDeltaMonday    = string.Empty;
    private string _cumDeltaTuesday   = string.Empty;
    private string _cumDeltaWednesday = string.Empty;
    private string _cumDeltaThursday  = string.Empty;

    // Page visibilities
    private bool _mainPageVisibility     = true;
    private bool _dayPageVisibility      = false;
    private bool _dimOverlayVisibility   = false;
    private bool _infoTextWeekVisibility = false;
    private bool _infoTextDayVisibility  = false;

    // Switch button
    private bool   _switchButtonEnabled = true;
    private double _switchButtonFontSize;
    private Color  _switchButtonBackgroundColor;

    // Texts
    private string _switchButtonText = "Tag";
    private string _bulletPoint1Text = "Man kann nicht vor 12:00 Uhr gehen.";
    private string _bulletPoint2Text = "Geht man vor 13:00 Uhr wird die Mittagspause nicht dazu gerechnet.";


    // Other
    private bool _settingsOpen      = false;
    private bool _weekMode          = true;
    private bool _isLoadingSettings = true;
    private bool _infoTextOpen      = false;

    private static readonly Color VSPurple = Color.FromRgb(80, 43, 212);

    public event Action<bool>? SettingsPanelToggled;

    private readonly MainModel _model;


    // Commands
    private readonly RelayCommand _questionMarkWeekCommand;
    private readonly RelayCommand _questionMarkDayCommand;
    private readonly RelayCommand _closeInfoCommand;

    #endregion



    public MainViewModel()
	{
        _switchButtonBackgroundColor = VSPurple;
        _switchButtonFontSize = GetNamedFontSize("Medium");

        _model = new MainModel();

        SettingsCommand = new RelayCommand(
            execute:    _ => Settings(),
            canExecute: _ => true);

        SwitchCommand = new RelayCommand(
            execute:    _ => Switch(),
            canExecute: _ => true);

        RestoreCommand = new RelayCommand(
            execute: _ => Restore(),
            canExecute: _ => true);

        _questionMarkWeekCommand = new RelayCommand(
            execute: _ => QuestionMarkWeek(),
            canExecute: _ => _weekMode);

        _questionMarkDayCommand = new RelayCommand(
            execute: _ => QuestionMarkDay(),
            canExecute: _ => !_weekMode);

        _closeInfoCommand = new RelayCommand(
            execute: _ => CloseInfo(),
            canExecute: _ => _infoTextOpen);
    }



    #region Public Properties

    // Commands
    public ICommand SettingsCommand { get; }
    public ICommand SwitchCommand { get; }
    public ICommand RestoreCommand { get; }
    public ICommand QuestionMarkWeekCommand => _questionMarkWeekCommand;
    public ICommand QuestionMarkDayCommand => _questionMarkDayCommand;
    public ICommand CloseInfoCommand => _closeInfoCommand;


    // Come & go times
    public TimeSpan ComeMonday
    {
        get => _model.ComeMonday;
        set 
        { 
            if (_model.ComeMonday == value) { return; }
            _model.ComeMonday = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public TimeSpan ComeTuesday
    {
        get => _model.ComeTuesday;
        set
        {
            if (_model.ComeTuesday == value) { return; }
            _model.ComeTuesday = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public TimeSpan ComeWednesday
    {
        get => _model.ComeWednesday;
        set
        {
            if (_model.ComeWednesday == value) { return; }
            _model.ComeWednesday = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public TimeSpan ComeThursday
    {
        get => _model.ComeThursday;
        set
        {
            if (_model.ComeThursday == value) { return; }
            _model.ComeThursday = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public TimeSpan ComeFriday
    {
        get => _model.ComeFriday;
        set
        {
            if (_model.ComeFriday == value) { return; }
            _model.ComeFriday = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public TimeSpan GoMonday
    {
        get => _model.GoMonday;
        set
        {
            if (_model.GoMonday == value) { return; }
            _model.GoMonday = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public TimeSpan GoTuesday
    {
        get => _model.GoTuesday;
        set
        {
            if (_model.GoTuesday == value) { return; }
            _model.GoTuesday = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public TimeSpan GoWednesday
    {
        get => _model.GoWednesday;
        set
        {
            if (_model.GoWednesday == value) { return; }
            _model.GoWednesday = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public TimeSpan GoThursday
    {
        get => _model.GoThursday;
        set
        {
            if (_model.GoThursday == value) { return; }
            _model.GoThursday = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }

    // Other times
    public TimeSpan ComeDay
    {
        get => _model.ComeDay;
        set 
        {
            if (_model.ComeDay == value) { return; }
            _model.ComeDay = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public string FeierabendWeek
    {
        get => _feierabendWeek;
        set => SetField(ref _feierabendWeek, value);
    }
    public string FeierabendDay
    {
        get => _feierabendDay;
        set => SetField(ref _feierabendDay, value);
    }

    // Setting times
    public TimeSpan SettingsCome
    {
        get => _model.SettingsCome;
        set
        {
            if (_model.SettingsCome == value) { return; }
            _model.SettingsCome = value;
            OnPropertyChanged();
            SettingsComeChanged();
        }
    }
    public TimeSpan SettingsGo
    {
        get => _model.SettingsGo;
        set
        {
            if (_model.SettingsGo == value) { return; }
            _model.SettingsGo = value;
            OnPropertyChanged();
            SettingsGoChanged();
        }
    }
    public string SettingsWeeklyHours
    {
        get => _model.SettingsWeeklyHours;
        set
        {
            if (_model.SettingsWeeklyHours == value) { return; }
            _model.SettingsWeeklyHours = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public string SettingsWeeklyMinutes
    {
        get => _model.SettingsWeeklyMinutes;
        set
        {
            if (_model.SettingsWeeklyMinutes == value) { return; }
            _model.SettingsWeeklyMinutes = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public string SettingsDailyHours
    {
        get => _model.SettingsDailyHours;
        set
        {
            if (_model.SettingsDailyHours == value) { return; }
            _model.SettingsDailyHours = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public string SettingsDailyMinutes
    {
        get => _model.SettingsDailyMinutes;
        set
        {
            if (_model.SettingsDailyMinutes == value) { return; }
            _model.SettingsDailyMinutes = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public TimeSpan SettingsSmallBreakStart
    {
        get => _model.SettingsSmallBreakStart;
        set
        {
            if (_model.SettingsSmallBreakStart == value) { return; }
            _model.SettingsSmallBreakStart = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public TimeSpan SettingsSmallBreakEnd
    {
        get => _model.SettingsSmallBreakEnd;
        set
        {
            if (_model.SettingsSmallBreakEnd == value) { return; }
            _model.SettingsSmallBreakEnd = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public TimeSpan SettingsMainBreakStart
    {
        get => _model.SettingsMainBreakStart;
        set
        {
            if (_model.SettingsMainBreakStart == value) { return; }
            _model.SettingsMainBreakStart = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }
    public TimeSpan SettingsMainBreakEnd
    {
        get => _model.SettingsMainBreakEnd;
        set
        {
            if (_model.SettingsMainBreakEnd == value) { return; }
            _model.SettingsMainBreakEnd = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }

    // Delta times
    public string DayDeltaMonday
    {
        get => _dayDeltaMonday;
        set => SetField(ref _dayDeltaMonday, value);
    }
    public string DayDeltaTuesday
    {
        get => _dayDeltaTuesday;
        set => SetField(ref _dayDeltaTuesday, value);
    }
    public string DayDeltaWednesday
    {
        get => _dayDeltaWednesday;
        set => SetField(ref _dayDeltaWednesday, value);
    }
    public string DayDeltaThursday
    {
        get => _dayDeltaThursday;
        set => SetField(ref _dayDeltaThursday, value);
    }
    public string CumDeltaMonday
    {
        get => _cumDeltaMonday;
        set => SetField(ref _cumDeltaMonday, value);
    }
    public string CumDeltaTuesday
    {
        get => _cumDeltaTuesday;
        set => SetField(ref _cumDeltaTuesday, value);
    }
    public string CumDeltaWednesday
    {
        get => _cumDeltaWednesday;
        set => SetField(ref _cumDeltaWednesday, value);
    }
    public string CumDeltaThursday
    {
        get => _cumDeltaThursday;
        set => SetField(ref _cumDeltaThursday, value);
    }

    // Page visibilities
    public bool MainPageVisibility
    {
        get => _mainPageVisibility;
        set => SetField(ref _mainPageVisibility, value);
    }
    public bool DayPageVisibility
    {
        get => _dayPageVisibility;
        set => SetField(ref _dayPageVisibility, value);
    }
    public bool DimOverlayVisibility
    {
        get => _dimOverlayVisibility;
        set => SetField(ref _dimOverlayVisibility, value);
    }
    public bool InfoTextWeekVisibility
    {
        get => _infoTextWeekVisibility;
        set => SetField(ref _infoTextWeekVisibility, value);
    }
    public bool InfoTextDayVisibility
    {
        get => _infoTextDayVisibility;
        set => SetField(ref _infoTextDayVisibility, value);
    }

    // Switch Button
    public bool SwitchButtonEnabled
    {
        get => _switchButtonEnabled;
        set => SetField(ref _switchButtonEnabled, value);
    }
    public double SwitchButtonFontSize
    {
        get => _switchButtonFontSize;
        set => SetField(ref _switchButtonFontSize, value);
    }
    public Color SwitchButtonBackgroundColor
    {
        get => _switchButtonBackgroundColor;
        set => SetField(ref _switchButtonBackgroundColor, value);
    }

    // Texts
    public string SwitchButtonText
    {
        get => _switchButtonText;
        set => SetField(ref _switchButtonText, value);
    }
    public string BulletPoint1Text
    {
        get => _bulletPoint1Text;
        set => SetField(ref _bulletPoint1Text, value);
    }
    public string BulletPoint2Text
    {
        get => _bulletPoint2Text;
        set => SetField(ref _bulletPoint2Text, value);
    }

    // Other flags
    public bool LateShift
    {
        get => _model.LateShift;
        set
        {
            if (_model.LateShift == value) { return; }
            _model.LateShift = value;
            OnPropertyChanged();
            SaveAndCalculate();
        }
    }

    #endregion



    #region Public methods

    /// <summary>
    /// Loading the time and settings states and calculates end-of-work time.
    /// </summary>
    public void LoadSettings()
    {
        _isLoadingSettings = true;
        _model.Load();
        _isLoadingSettings = false;

        SyncFromModel();
        ApplyResults(_model.Calculate());
    }

    #endregion



    #region Command methods

    /// <summary>
    /// The settings button toggles the settings panel with a slide animation.
    /// </summary>
    private void Settings()
    {
        if (_settingsOpen)
        {
            _settingsOpen        = false;
            SwitchButtonEnabled  = true;
            DimOverlayVisibility = false;
            if (_weekMode) { MainPageVisibility = true; }
            else { DayPageVisibility = true; }
        }
        else
        {
            _settingsOpen        = true;
            SwitchButtonEnabled  = false;
            DimOverlayVisibility = true;
            MainPageVisibility   = false;
            DayPageVisibility    = false;
            SwitchButtonBackgroundColor = VSPurple;
        }

        if (_infoTextOpen)
        {
            InfoTextWeekVisibility = false;
            InfoTextDayVisibility  = false;
            _infoTextOpen          = false;
        }

        SettingsPanelToggled?.Invoke(_settingsOpen);
    }


    /// <summary>
    /// The switch button is toggling between week and day modes. Adjusts button Text, FontSize.
    /// </summary>
    private void Switch()
    {
        if (_weekMode)
        {
            SwitchButtonText = "Woche";
            SwitchButtonFontSize = GetNamedFontSize("Caption");
            MainPageVisibility = false;
            DayPageVisibility = true;
            _weekMode = false;
        }
        else
        {
            SwitchButtonText = "Tag";
            SwitchButtonFontSize = GetNamedFontSize("Medium");
            MainPageVisibility = true;
            DayPageVisibility = false;
            _weekMode = true;
        }

        _questionMarkWeekCommand.NotifyCanExecuteChanged();
        _questionMarkDayCommand.NotifyCanExecuteChanged();

        if (_infoTextOpen)
        {
            InfoTextWeekVisibility = false;
            InfoTextDayVisibility = false;
            DimOverlayVisibility = false;

            _infoTextOpen = false;
            _closeInfoCommand.NotifyCanExecuteChanged();
        }
    }


    /// <summary>
    /// The questionmark button opens text bubble giving information about Feierabend rules for the week.
    /// </summary>
    private void QuestionMarkWeek()
    {
        if (!_infoTextOpen)
        {
            if (_model.LateShift)
            {
                BulletPoint1Text = "Man kann nicht vor 16:15 Uhr gehen.";
                BulletPoint2Text = "Kommt man nach 9:00 Uhr wird die kleine Pause nicht angerechnet.";
            }

            else
            {
                BulletPoint1Text = "Man kann nicht vor 12:00 Uhr gehen.";
                BulletPoint2Text = "Geht man vor 13:00 Uhr wird die Mittagspause nicht angerechnet.";
            }

            InfoTextWeekVisibility = true;
            DimOverlayVisibility = true;

            _infoTextOpen = true;
            _closeInfoCommand.NotifyCanExecuteChanged();
        }
    }


    /// <summary>
    /// The questionmark button opens text bubble giving information about Feierabend rules for the day.
    /// </summary>
    private void QuestionMarkDay()
    {
        if (!_infoTextOpen)
        {
            InfoTextDayVisibility = true;
            DimOverlayVisibility = true;

            _infoTextOpen = true;
            _closeInfoCommand.NotifyCanExecuteChanged();
        }
    }


    /// <summary>
    /// Closes info text bubble if open.
    /// </summary>
    private void CloseInfo()
    {
        if (_infoTextOpen)
        {
            InfoTextWeekVisibility = false;
            InfoTextDayVisibility = false;
            DimOverlayVisibility = false;

            _infoTextOpen = false;
            _closeInfoCommand.NotifyCanExecuteChanged();
        }
    }


    /// <summary>
    /// Restores come, go times from the settings.
    /// </summary>
    private void Restore()
    {
        SettingsComeChanged();
        SettingsGoChanged();
    }

    #endregion



    #region Private (helper) methods

    /// <summary>
    /// Sets all come times to settings come time once changed.
    /// </summary>
    private void SettingsComeChanged()
    {
        if (_isLoadingSettings) { return; }

        _isLoadingSettings = true;

        try
        {
            ComeMonday    = SettingsCome;
            ComeTuesday   = SettingsCome;
            ComeWednesday = SettingsCome;
            ComeThursday  = SettingsCome;
            ComeFriday    = SettingsCome;

            ComeDay       = SettingsCome;
        }
        finally
        {
            _isLoadingSettings = false;
        }

        _model.Save();
        ApplyResults(_model.Calculate());
    }


    /// <summary>
    /// Sets all go times to settings go time once changed.
    /// </summary>
    private void SettingsGoChanged()
    {
        if (_isLoadingSettings) { return; }

        _isLoadingSettings = true;

        try
        {
            GoMonday = SettingsGo;
            GoTuesday = SettingsGo;
            GoWednesday = SettingsGo;
            GoThursday = SettingsGo;
        }
        finally
        {
            _isLoadingSettings = false;
        }

        _model.Save();
        ApplyResults(_model.Calculate());
    }


    /// <summary>
    /// Saves time and setting states and calculates end-of-work time for the public property setters.
    /// </summary>
    private void SaveAndCalculate()
    {
        if (_isLoadingSettings) { return; }

        _model.Save();
        ApplyResults(_model.Calculate());
    }


    /// <summary>
    /// Sets time calculation results with correct string formatting.
    /// </summary>
    private void ApplyResults(TimeCalculations results)
    {
        FeierabendWeek    = $"{results.FeierabendWeek.Hours}:{results.FeierabendWeek.Minutes:D2}";
        FeierabendDay     = $"{results.FeierabendDay.Hours}:{results.FeierabendDay.Minutes:D2}";

        DayDeltaMonday    = FormatDelta(results.DayDeltas[0]);
        DayDeltaTuesday   = FormatDelta(results.DayDeltas[1]);
        DayDeltaWednesday = FormatDelta(results.DayDeltas[2]);
        DayDeltaThursday  = FormatDelta(results.DayDeltas[3]);

        CumDeltaMonday    = FormatDelta(results.CumDeltas[0]);
        CumDeltaTuesday   = FormatDelta(results.CumDeltas[1]);
        CumDeltaWednesday = FormatDelta(results.CumDeltas[2]);
        CumDeltaThursday  = FormatDelta(results.CumDeltas[3]);
    }


    /// <summary>
    /// Sync data from model to refresh UI.
    /// </summary>
    private void SyncFromModel()
    {
        OnPropertyChanged(nameof(ComeMonday));
        OnPropertyChanged(nameof(ComeTuesday));
        OnPropertyChanged(nameof(ComeWednesday));
        OnPropertyChanged(nameof(ComeThursday));
        OnPropertyChanged(nameof(ComeFriday));
        OnPropertyChanged(nameof(GoMonday));
        OnPropertyChanged(nameof(GoTuesday));
        OnPropertyChanged(nameof(GoWednesday));
        OnPropertyChanged(nameof(GoThursday));
        OnPropertyChanged(nameof(ComeDay));
        OnPropertyChanged(nameof(SettingsCome));
        OnPropertyChanged(nameof(SettingsGo));
        OnPropertyChanged(nameof(SettingsWeeklyHours));
        OnPropertyChanged(nameof(SettingsWeeklyMinutes));
        OnPropertyChanged(nameof(SettingsDailyHours));
        OnPropertyChanged(nameof(SettingsDailyMinutes));
        OnPropertyChanged(nameof(SettingsSmallBreakStart));
        OnPropertyChanged(nameof(SettingsSmallBreakEnd));
        OnPropertyChanged(nameof(SettingsMainBreakStart));
        OnPropertyChanged(nameof(SettingsMainBreakEnd));
        OnPropertyChanged(nameof(LateShift));
    }


    /// <summary>
    /// Takes TimeSpan and format into displayable string with sign indicators.
    /// </summary>
    /// <param name="time">The TimeSpan delta to format.</param>
    /// <returns>Formatted time as string.</returns>
    private static string FormatDelta(TimeSpan time)
    {
        string deltaText;
        double hours = time.Hours;
        double minutes = time.Minutes;

        if (Math.Abs(hours) > 0)
        {
            if (hours > 0)
            {
                deltaText = $"+{hours}h {Math.Abs(minutes)}min";
            }
            else
            {
                deltaText = $"{hours}h {Math.Abs(minutes)}min";
            }
        }
        else
        {
            if (minutes > 0)
            {
                deltaText = $"+{minutes}min";
            }
            else
            {
                deltaText = $"{minutes}min";
            }
        }

        return deltaText;
    }


    /// <summary>
    /// Gets double representation of named FontSize as string input.
    /// </summary>
    private static double GetNamedFontSize(string namedSize)
    {
        var converter = new FontSizeConverter();
        return (double)converter.ConvertFromInvariantString(namedSize)!;
    }

    #endregion
}
