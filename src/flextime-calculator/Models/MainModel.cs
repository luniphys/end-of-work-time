using flextime_calculator.Constants;

namespace flextime_calculator.Models;


public sealed class MainModel
{
    #region Public properties

    // Come & go times
    public TimeSpan ComeMonday    { get; set; }
    public TimeSpan ComeTuesday   { get; set; }
    public TimeSpan ComeWednesday { get; set; }
    public TimeSpan ComeThursday  { get; set; }
    public TimeSpan ComeFriday    { get; set; }
    public TimeSpan GoMonday      { get; set; }
    public TimeSpan GoTuesday     { get; set; }
    public TimeSpan GoWednesday   { get; set; }
    public TimeSpan GoThursday    { get; set; }

    // Other times
    public TimeSpan ComeDay       { get; set; }

    // Setting times
    public TimeSpan SettingsCome              { get; set; }
    public TimeSpan SettingsGo                { get; set; }
    public string   SettingsWeeklyHours       { get; set; } = string.Empty;
    public string   SettingsWeeklyMinutes     { get; set; } = string.Empty;
    public string   SettingsDailyHours        { get; set; } = string.Empty;
    public string   SettingsDailyMinutes      { get; set; } = string.Empty;
    public TimeSpan SettingsSmallBreakStart   { get; set; }
    public TimeSpan SettingsSmallBreakEnd     { get; set; }
    public TimeSpan SettingsMainBreakStart    { get; set; }
    public TimeSpan SettingsMainBreakEnd      { get; set; }
    public string   SettingsAdditionalHours   { get; set; } = string.Empty;
    public string   SettingsAdditionalMinutes { get; set; } = string.Empty;

    // Other
    public bool LateShift { get; set; }

    #endregion



    #region Public methods

    /// <summary>
    /// Loads come, go and setting times from storage and sets them as values.
    /// </summary>
    public void Load()
    {
        // Settings
        SettingsCome = ParseTime(Preferences.Get(PreferenceKeys.SettingsCome, "06:00"));
        SettingsGo = ParseTime(Preferences.Get(PreferenceKeys.SettingsGo, "14:15"));

        SettingsWeeklyHours = Preferences.Get(PreferenceKeys.SettingsWeeklyHours, "37");
        SettingsWeeklyMinutes = Preferences.Get(PreferenceKeys.SettingsWeeklyMinutes, "30");

        double weeklyTimeDouble = TimeToDouble(SettingsWeeklyHours, SettingsWeeklyMinutes);
        double dailyHoursDouble = Math.Floor((weeklyTimeDouble / 5));
        double dailyMinutesDouble = Math.Round(((weeklyTimeDouble / 5) % 1) * 60);
        SettingsDailyHours = Preferences.Get(PreferenceKeys.SettingsDailyHours, dailyHoursDouble.ToString());
        SettingsDailyMinutes = Preferences.Get(PreferenceKeys.SettingsDailyMinutes, dailyMinutesDouble.ToString());

        SettingsSmallBreakStart = ParseTime(Preferences.Get(PreferenceKeys.SettingsSmallBreakStart, "09:00"));
        SettingsSmallBreakEnd = ParseTime(Preferences.Get(PreferenceKeys.SettingsSmallBreakEnd, "09:15"));
        SettingsMainBreakStart = ParseTime(Preferences.Get(PreferenceKeys.SettingsMainBreakStart, "12:00"));
        SettingsMainBreakEnd = ParseTime(Preferences.Get(PreferenceKeys.SettingsMainBreakEnd, "12:30"));

        SettingsAdditionalHours = Preferences.Get(PreferenceKeys.SettingsAdditionalHours, "0");
        SettingsAdditionalMinutes = Preferences.Get(PreferenceKeys.SettingsAdditionalMinutes, "0");

        // Come and go times
        string settingsCome = SettingsCome.ToString();
        string settingsGo = SettingsGo.ToString();

        ComeMonday = ParseTime(Preferences.Get(PreferenceKeys.ComeMonday, settingsCome));
        ComeTuesday = ParseTime(Preferences.Get(PreferenceKeys.ComeTuesday, settingsCome));
        ComeWednesday = ParseTime(Preferences.Get(PreferenceKeys.ComeWednesday, settingsCome));
        ComeThursday = ParseTime(Preferences.Get(PreferenceKeys.ComeThursday, settingsCome));
        ComeFriday = ParseTime(Preferences.Get(PreferenceKeys.ComeFriday, settingsCome));
        GoMonday = ParseTime(Preferences.Get(PreferenceKeys.GoMonday, settingsGo));
        GoTuesday = ParseTime(Preferences.Get(PreferenceKeys.GoTuesday, settingsGo));
        GoWednesday = ParseTime(Preferences.Get(PreferenceKeys.GoWednesday, settingsGo));
        GoThursday = ParseTime(Preferences.Get(PreferenceKeys.GoThursday, settingsGo));

        ComeDay = ParseTime(Preferences.Get(PreferenceKeys.ComeDayTime, settingsCome));

        LateShift = Preferences.Get(PreferenceKeys.LateShift, false);
    }


    /// <summary>
    /// Saves the current come, go and setting times to application preferences.
    /// </summary>
    public void Save()
    {
        Preferences.Set(PreferenceKeys.ComeMonday, ComeMonday.ToString());
        Preferences.Set(PreferenceKeys.ComeTuesday, ComeTuesday.ToString());
        Preferences.Set(PreferenceKeys.ComeWednesday, ComeWednesday.ToString());
        Preferences.Set(PreferenceKeys.ComeThursday, ComeThursday.ToString());
        Preferences.Set(PreferenceKeys.ComeFriday, ComeFriday.ToString());
        Preferences.Set(PreferenceKeys.GoMonday, GoMonday.ToString());
        Preferences.Set(PreferenceKeys.GoTuesday, GoTuesday.ToString());
        Preferences.Set(PreferenceKeys.GoWednesday, GoWednesday.ToString());
        Preferences.Set(PreferenceKeys.GoThursday, GoThursday.ToString());

        Preferences.Set(PreferenceKeys.ComeDayTime, ComeDay.ToString());

        Preferences.Set(PreferenceKeys.SettingsCome, SettingsCome.ToString());
        Preferences.Set(PreferenceKeys.SettingsGo, SettingsGo.ToString());
        Preferences.Set(PreferenceKeys.SettingsWeeklyHours, SettingsWeeklyHours);
        Preferences.Set(PreferenceKeys.SettingsWeeklyMinutes, SettingsWeeklyMinutes);
        Preferences.Set(PreferenceKeys.SettingsDailyHours, SettingsDailyHours);
        Preferences.Set(PreferenceKeys.SettingsDailyMinutes, SettingsDailyMinutes);
        Preferences.Set(PreferenceKeys.SettingsSmallBreakStart, SettingsSmallBreakStart.ToString());
        Preferences.Set(PreferenceKeys.SettingsSmallBreakEnd, SettingsSmallBreakEnd.ToString());
        Preferences.Set(PreferenceKeys.SettingsMainBreakStart, SettingsMainBreakStart.ToString());
        Preferences.Set(PreferenceKeys.SettingsMainBreakEnd, SettingsMainBreakEnd.ToString());
        Preferences.Set(PreferenceKeys.SettingsAdditionalHours, SettingsAdditionalHours);
        Preferences.Set(PreferenceKeys.SettingsAdditionalMinutes, SettingsAdditionalMinutes);

        Preferences.Set(PreferenceKeys.LateShift, LateShift);
    }


    /// <summary>
    /// Calculates the end-of-work time (german: 'Feierabend') for Friday (week mode) and a single day (day mode)
    /// based on weekly and daily hours, break durations and hours worked Monday through Thursday.
    /// Also evaluates the time differences (deltas) between needed and actual working time.
    /// </summary>
    /// <returns>TimeCalculations object containing Feierabend of week/day plus delta times as separate arrays.</returns>
    public TimeCalculations Calculate()
    {
        TimeSpan smallBreakDuration = SettingsSmallBreakEnd - SettingsSmallBreakStart;
        TimeSpan mainBreakDuration = SettingsMainBreakEnd - SettingsMainBreakStart;

        if (smallBreakDuration < TimeSpan.Zero || mainBreakDuration < TimeSpan.Zero)
        {
            return new TimeCalculations(TimeSpan.Zero, TimeSpan.Zero, new TimeSpan[4], new TimeSpan[4]);
        }

        TimeSpan totalBreakDuration = smallBreakDuration + mainBreakDuration;

        double weeklyTotal = TimeToDouble(SettingsWeeklyHours, SettingsWeeklyMinutes);
        TimeSpan totalWeeklyHours = TimeSpan.FromHours(weeklyTotal);

        double dailyTotal = TimeToDouble(SettingsDailyHours, SettingsDailyMinutes);
        TimeSpan totalDailyHours = TimeSpan.FromHours(dailyTotal);

        double additionalBreak = TimeToDouble(SettingsAdditionalHours, SettingsAdditionalMinutes);
        TimeSpan additionalBreakDay = TimeSpan.FromHours(additionalBreak);
        TimeSpan additionalBreakWeek = additionalBreakDay * 5;


        // Come, go & duration lists
        TimeSpan[] comeTimes = [ComeMonday, ComeTuesday, ComeWednesday, ComeThursday, ComeFriday, ComeDay];
        TimeSpan[] goTimes = [GoMonday, GoTuesday, GoWednesday, GoThursday];

        TimeSpan[] durations = new TimeSpan[4];


        // Check if small break needs to be subtracted
        for (int i = 0; i < comeTimes.Length; i++)
        {
            comeTimes[i] = (comeTimes[i] > SettingsSmallBreakStart && comeTimes[i] <= SettingsSmallBreakEnd) ? SettingsSmallBreakEnd : comeTimes[i];
        }

        for (int i = 0; i < durations.Length; i++)
        {
            durations[i] = (comeTimes[i] >= SettingsSmallBreakEnd) ? (goTimes[i] - comeTimes[i] - mainBreakDuration) : (goTimes[i] - comeTimes[i] - totalBreakDuration);
        }


        // Updating delta times
        TimeSpan[] dayDeltas = new TimeSpan[4];
        TimeSpan[] cumDeltas = new TimeSpan[4];

        TimeSpan deltaTime;
        TimeSpan cumDeltaTime = TimeSpan.Zero;

        for (int i = 0; i < dayDeltas.Length; i++)
        {
            deltaTime = durations[i] - totalDailyHours - additionalBreakDay;
            dayDeltas[i] = deltaTime;

            cumDeltaTime += deltaTime;
            cumDeltas[i] = cumDeltaTime;
        }


        // Calculating Feierabend Week
        TimeSpan fourDayDuration = TimeSpan.Zero;
        foreach (TimeSpan duration in durations)
        {
            fourDayDuration += duration;
        }
        TimeSpan fridayHours = totalWeeklyHours - fourDayDuration;
        TimeSpan comeFriday = comeTimes[4];
        TimeSpan feierAbendWeek = (comeFriday >= SettingsSmallBreakEnd) ? comeFriday + fridayHours : comeFriday + fridayHours + smallBreakDuration;

        feierAbendWeek += additionalBreakWeek;

        TimeSpan oneOClock = new TimeSpan(13, 0, 0); // Leaving before 13:00 won't add the main break to working times
        feierAbendWeek = (feierAbendWeek < oneOClock) ? feierAbendWeek : feierAbendWeek + mainBreakDuration;

        TimeSpan twelveOClock = new TimeSpan(12, 0, 0); // Can't leave before 12:00
        if (!LateShift)
        {
            feierAbendWeek = (feierAbendWeek < twelveOClock) ? twelveOClock : feierAbendWeek;
        }

        TimeSpan fourFifteen = new TimeSpan(16, 15, 0); // Can't leave before 16:15 at late shift
        if (LateShift)
        {
            feierAbendWeek = (feierAbendWeek < fourFifteen) ? fourFifteen : feierAbendWeek;
        }

        // Calculating Feierabend day
        TimeSpan comeDay = comeTimes[5];
        TimeSpan feierAbendDay = (comeDay >= SettingsSmallBreakEnd) ? comeDay + totalDailyHours + mainBreakDuration : comeDay + totalDailyHours + totalBreakDuration;

        feierAbendDay += additionalBreakDay;


        return new TimeCalculations(feierAbendWeek, feierAbendDay, dayDeltas, cumDeltas);
    }

    #endregion



    #region Private (helper) methods

    /// <summary>
    /// Converts string hours and minutes to a decimal hour representation.
    /// </summary>
    /// <returns>Returns 0.0 if the input is invalid.</returns>
    /// <remarks>Minutes are represented as a fraction of an hour.</remarks>
    private static double TimeToDouble(string hoursString, string minutesString)
    {
        double hoursDouble = 0.0;
        double minutesDouble = 0.0;

        if (double.TryParse(hoursString, out double parsedHours))
        {
            hoursDouble = parsedHours;
        }

        if (double.TryParse(minutesString, out double parsedMinutes))
        {
            minutesDouble = parsedMinutes;
        }

        if (hoursDouble < 0 || minutesDouble < 0 || minutesDouble > 60)
        {
            return 0.0;
        }

        return hoursDouble + minutesDouble / 60;
    }


    /// <summary>
    /// Attempts to parse TimeSpan as string to TimeSpan object.
    /// </summary>
    /// <returns>TimeSpan.Zero if parsing fails.</returns>
    private static TimeSpan ParseTime(string timeString)
    {
        if (TimeSpan.TryParse(timeString, out var time))
        {
            return time;
        }
        return TimeSpan.Zero;
    }

    #endregion
}
