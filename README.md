ILNEditor
==========
[![Nuget](https://img.shields.io/nuget/v/ILNEditor?style=flat-square&logo=nuget&color=blue)](https://www.nuget.org/packages/ILNEditor)

Editor for ILNumerics (http://ilnumerics.net/) scene graphs and plot cubes.

## How to use

Attach the editor to your ILPanel instance:
```csharp
ILPanelEditor.AttachTo(ilPanel);
```
or (if you want to interact with the editor in code, e.g. serialization)
```csharp
var editor = ILPanelEditor.AttachTo(ilPanel);
```

Open the editor dialog by clicking on an object
in the graph/plot. For example, click on

- the Axes X/Y/Z for axis properties
- the SurfacePlot for ILSurface properties
- a LinePlot line for ILLinePlot properties
- etc.

Context menu (right click) provides additional options.

#### Public Methods

There are a few public methods on the panel editor:
- ```ShowEditor()``` open the panel editor (interactively change properties) from code
- ```ShowPlotBrowser()``` open the plot browser (list of known plot types) from code

#### De/Serialize scene state

Scene state refers to the properties of all objects in the scene graph. It does NOT (de)serialize the actual scene graph, i.e. it does NOT restore the graph on deserialization. The scene state captures colors, line styles, visibility, fonts and font sizes, etc., all the properties defining the visual appearance of the scene. The primary use case is to save and re-apply or transfer the _style_ to a new scene graph. To some degree the serialization is fuzzy, i.e. it still applies if objects are removed or added to the original scene (new objects obviously won't receive any styling).

Serialize settings from the current scene:
```csharp
var serializer = new XmlSerializer();
editor.Serialize(serializer);
//string xmlString = serializer.SaveToString();
serializer.SaveToFile(filePath);
```

Deserialize settings to the current scene:
```csharp
var deserializer = new XmlDeserializer();
//deserializer.LoadFromString(xmlString);
deserializer.LoadFromFile(filePath);
editor.Deserialize(deserializer);
```

#### PropertyChanged notifications

You can monitor for changes of the current scene's state by subscribing to the _PropertyChanged_ event of the panel editor:

```csharp
editor.PropertyChanged += myChangeHandler;
```


#### PlotCube menu

To customize the PlotCube menu obtain a reference to it by calling the _GetPlotCubeMenu()_ method of the panel editor.

```csharp
var menu = editor.GetPlotCubeMenu();

menu.Add("-"); // Separator
menu.Add("Click Me", null, (sender, args) => { /* do something*/ });
```

### Disclaimer
ILNEditor is in an early phase of development and should be considered experimental.

As of today (Jan 2023) the plot types in _ILNumerics.Toolboxes.Drawing2_ are not supported yet. However, you can inject support for custom types (and still unsupported types) by adding your own wrappers to the ```WrapperMap``` of panel editor. Of course, I hope to add support in the future.

### License
ILNEditor is licensed under the terms of the MIT license (<http://opensource.org/licenses/MIT>, see LICENSE.txt).
