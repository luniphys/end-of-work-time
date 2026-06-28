# src/flextime-calculator

Application sourcecode directory.

## Contents

```
flextime-calculator/
├── App.xaml / App.xaml.cs                      # Application entry point
├── AppShell.xaml / AppShell.xaml.cs            # Defining the app's pages
├── MauiProgram.cs                              # App configuration
├── flextime-calculator.csproj                  # Project file
├── Models/
│   ├── MainModel.cs                            # Main model containing all logic
│   └── TimeCalculations.cs                     # Record holding computed results (Feierabend times, daily/cumulative deltas)
├── ViewModels/
│   ├── ViewModelBase.cs                        # INotifyPropertyChanged implementation
│   ├── MainViewModel.cs                        # ViewModel for the main page
│   └── SetupViewModel.cs                       # ViewModel for the first-time setup wizard
├── Views/
│   ├── MainPage.xaml / MainPage.xaml.cs        # Main page with week/day views and settings panel
│   └── FirstTimeSetupPage.xaml / .xaml.cs      # Setup wizard shown on first launch
├── Commands/
│   └── RelayCommand.cs                         # ICommand implementation
├── Constants/
│   └── PreferenceKeys.cs                       # Keys for MAUI `Preferences`
├── Platforms/                                  # Platform specific files
└── Resources/                                  # Assets
```

## Notes

- The settings and time states are stored via MAUI's `Preferences`
