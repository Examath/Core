# Examath.Core
A class library containing of styles, controls and other code for .net WPF desktop applications.

# Modules
## Controls
- `BoolOutput`: a boolean indicator.
- `DirectoryPicker`: using Ookii.Dialogs.Wpf
- `FilePicker : with directory support
## Converters
- `EnumToItemsSource`
- `EqualityConverter`
- `MultiplyConverter`
- `NotConverter`
- `StringSplitConverter`
## Enviorment
An console-like user interface system allowing for accepting user input and logging application output.
## Model
`FileManipulationObject`, a derivative of CommunityToolkit.Mvvm ObservableObject that allows for saving data to a file.
`XMLFileObject` implements this using xml as the serialization format
## Parts (XAML Styles)
TBC...
## Plugin
A system for running user scripts with Scripter
## Utils
`TimeLogger` supports timing for Enviorment
