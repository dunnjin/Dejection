# Dejection
A simple convention-based MVVM framework for WPF

Its used to

## Table of contents

1. [Implemented functionality](https://github.com/osc-solutions/Dejection#implemented-functionality)
2. [Referencing](https://github.com/osc-solutions/Dejection#referencing)
3. [Usage](https://github.com/osc-solutions/Dejection#usage)
4. [Example](https://github.com/osc-solutions/Dejection#example)
5. [License](https://github.com/osc-solutions/Dejection#license)



## Implemented functionality
* Automatically generated dynamic datatemplates to bind ViewModel to Views via name convention
* Dialog service via ViewModel type to manage windows/dialogs without connection to UserControls
* Navigation service via ViewModel types to change ContentControl ViewModels inside views from any dialog from any action
* Fluent command service supporting undo/redo that implements the ICommand interface



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
   /// <summary>
   /// Required entry point
   /// Uses an extension on StartupEventArgs to start the app
   /// </summary>
   /// <param name="startupEventArgs"></param>
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
                // NOTE: Dejection dependencies are all already registered

                // Here you should register your dependencies which will automatically be resolved via creation of ViewModels
                container.RegisterType<IApplicationService, ApplicationService>();
            })
            // Starts a the main window with the given ViewModel
            .Start<ApplicationViewModel>();
    }
}
```



### Dialog Service
Uses ViewModel types to create window dialogs, decoupled from UserControl's or Views
```csharp

// Will create a new window and inject its View/ViewModel into its resources
// Behaves like the standard Windows ShowDialog / Show (async also supported)
IDialogService dialogService = new DialogService();
dialogService.ShowDialog<SampleViewModel>();

IDialogService dialogService = new DialogService();
dialogService.Show<SampleViewModel>();
```

Close a dialog based off of ViewModel
```csharp

// Will close the topmost dialog of the given ViewModel when called
IDialogService dialogService = new DialogService();
dialogService.Close<SampleViewModel>();
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
    .Execute(() =>
    {
        // Do something
    })
    // Returns ICommand for WPF
    .Relay();
```

Catch exceptions
```csharp
commandService
    .Execute(() =>
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
    .Execute(() =>
    {
        // Do something
    })
    // Will execute command without returning ICommand
    .Run();
```

Can Execute
```csharp
commandService
    .Execute(() =>
    {
        // Do something
    })
    .CanExecute(() =>
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

Validation
```csharp
    public class TestViewModel : ViewModelBase
    {
        private readonly ICommandService _commandService;

        private string _requiredField;

        [Required]
        public string RequiredField
        {
            get { return _requiredField; }
            set
            {
                if(_requiredField == value)
                    return;

                _requiredField = value;

                NotifyPropertyChanged();
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _commandService
                    .Execute(() =>
                    {
                        // Save RequiredField
                    })
                    .CanExecute(obj =>
                    {
                        // If Required attribute is satisfied then IsValidated will result to true and the SaveCommand will be allowed to execute, else the button will not be allowed to be pressed
                        return base.IsValidated;
                    })
                    .Relay();

            }
        }
    }
```

```xaml
<!-- Note all views should be usercontrols to support reuseability -->
<UserControl x:Class="Osc.Dejection.Sample.Views.SampleView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             mc:Ignorable="d"
             d:DesignHeight="300" d:DesignWidth="300" Background="{StaticResource MainBackgroundColorBrush}">
    <StackPanel>
        <TextBox Text="{Binding RequiredField, ValidatesOnDataErrors=True, UpdateSourceTrigger=PropertyChanged}"></TextBox>
        <Button Content="Save" Command="{Binding SaveCommand}"/>
    </StackPanel>
</UserControl>
```


## Example

A sample project can be found [here](https://github.com/osc-solutions/Dejection/tree/master/Osc.Dejection.Sample)
. Sample project provides several different examples of scenarios, however, there is still more functionality that exists not included. This also assumes you understand the MVVM pattern.

## License

Copyright (c) 2015 - 2016 Jonathan Dunn

Dejection is provided as-is under the MIT license. For more information see [LICENSE](https://opensource.org/licenses/MIT).
