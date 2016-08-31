# Dejection
A simple convention-based MVVM framework for WPF



## Table of contents

1. [Implemented functionality](https://github.com/osc-solutions/Dejection#implemented-functionality)
2. [Referencing](https://github.com/osc-solutions/Dejection#referencing)
3. [Usage](https://github.com/osc-solutions/Dejection#usage)
4. [Example](https://github.com/osc-solutions/Dejection#example)
5. [License](https://github.com/osc-solutions/Dejection#license)



## Implemented functionality
* Automatically generated dynamic datatemplates
* Dialog service via viewmodel type
* Navigation service via viewmodel types
* Fluent command service supporting undo/redo ICommand



## Referencing

Dejection is available as a nuget package from the package manager console:

```csharp
Install-Package Osc.Dejection
```



## Usage



### Startup
```csharp
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs startupEventArgs)
    {
    
        startupEventArgs
            .Register(assembly =>
            {
              // Which dlls should be looked at inside the app domain when registering views/viewModels
              // Views and view models should be in the format of {name}View and {name}ViewModel
              // Ex: ApplicationView (UserControl) ApplicationViewModel (ViewModel)
              return assembly.FullName.StartsWith("Osc")
            })
            .Container(container =>
            {
                // Register unity IoC dependencies
                container.RegisterType<IApplicationService, ApplicationService>();
            })
            // Starts a dialog with the given viewModel
            .Start<ApplicationViewModel>();
    }
}
```



### Dialog Service
Uses view model types to create window dialogs, decoupled from usercontrols or views
```csharp

// Will create a new window (from default or given settings) and inject its view/viewModel into its resources
// Behaves like the standard ShowDialog (async also supported)
IDialogService dialogService = new DialogService();
dialogService.ShowDialog<SampleViewModel>();

```
```csharp

// Will create a new window (from default or given settings) and inject its view/viewModel into its resources
// Behaves like the standard Show (includes dialog settings)
IDialogService dialogService = new DialogService();
dialogService.Show<SampleViewModel>(new DialogSettings()
    {
        AllowsTransparency = false,
        PreventClosing = false,
        ResizeMode = ResizeMode.CanResize,
        WindowStartupLocation = WindowStartupLocation.CenterOwner,
        WindowState = WindowState.Normal,
        WindowStyle = WindowStyle.SingleBorderWindow,
        SizeToContent = SizeToContent.WidthAndHeight,
    });
```



### Navigation Service
Uses view model types to determine which owner should display which target
```xaml
<!-- Note all views should be usercontrols to support reuseability -->
<UserControl x:Class="Osc.Dejection.Sample.Views.SampleView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             mc:Ignorable="d"
             d:DesignHeight="300" d:DesignWidth="300" Background="{StaticResource MainBackgroundColorBrush}">
    <Grid>
        <ContentControl Content="{Binding SelectedViewModel}"></ContentControl>
    </Grid>
</UserControl>
```

```csharp

// Will set the SelectedViewModel from the view to the specified viewModel
INavigationService navigationService = new NavigationService();
// Owner/Target
navigationService.Navigate<SampleViewModel, SampleViewModelChild>();
```

```csharp
// Can navigate to a different dialog viewModel
// The Owner will be selected starting from the topmost dialog going down
// Once Owner is found the Target will be set creating a new instance of SelectedViewModel
INavigationService navigationService = new NavigationService();
navigationService.Navigate<DialogSampleViewModel, SampleViewModelChild>();
```



### Command service
Uses a simple fluent language as a wrapper for a ICommand implementation

```csharp
ICommandService commandService = new CommandService();
commandService
    .Execute(obj =>
    {
        // Do something
    })
    // Returns ICommand for WPF
    .Relay();
```

Catch exceptions
```csharp
commandService
    .Execute(obj =>
    {
        // Do something
    })
    .OnException<Exception>(exception =>
    {
        // Catches exception type specified by generic
    });
```

Run without returning ICommand
```csharp
commandService
    .Execute(obj =>
    {
        // Do something
    })
    // Will execute command without returning ICommand
    .Run();
```

Can Execute
```csharp
commandService
    .Execute(obj =>
    {
        // Do something
    })
    .CanExecute(obj =>
    {
        // Dictates whether the command can be executed
        // Also tied to ICommand CanExecute integrated with WPF
        return true;
    })
    .Run();

```

Listeners
```csharp
public partial class App : Application
   {
       protected override void OnStartup(StartupEventArgs startupEventArgs)
       {
           startupEventArgs
               .Register(assembly => assembly.FullName.StartsWith("Osc"))
               .Container(container =>
               {
                   // Can be used for global logging throughout the application
                   // Setup listeners
                   ICommandService commandService = container.Resolve<ICommandService>();

                   // Captures all exceptions that occur
                   commandService.Listener.OnException = (exception, data) =>
                   {
                         Console.WriteLine(exception.ToString());
                   };

                   // Captures all actions executed
                   commandService.Listener.Execute = data =>
                   {
                         Console.WriteLine($"Class: {data.ClassName}");
                         Console.WriteLine($"Method: {data.MethodName}");
                         Console.WriteLine($"Line Number: {data.LineNumber}");
                   };
               })
               .Start<ApplicationViewModel>();
       }
   }
```

Undo/Redo
```csharp

// Undo the last command that was executed
int i = 0;

ICommandService commandService = new CommandService();
commandService.ExecuteToStack(() =>
    {
        // Do something
        i = 1;
    })
    .UnExecute(() =>
    {
        // Must provide inverse of what is executed
        i = 0;
    })
    .Run();

// i = 1

// Undo something
commandService.Undo();

// i = 0

commandService.Redo();

// i = 1
```



## Example

A sample project can be found [here](https://github.com/osc-solutions/Dejection/tree/master/Osc.Dejection.Sample)

## License

Copyright (c) 2015 - 2016 Jonathan Dunn

Dejection is provided as-is under the MIT license. For more information see [LICENSE](https://opensource.org/licenses/MIT).
