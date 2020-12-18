ILNEditor
==========

Editor for ILNumerics (http://ilnumerics.net/) scene graphs
and plot cubes.

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

Context menu (right click) provides additional
options.

#### De/Serialize scene state

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

### License
ILNEditor is licensed under the terms of the MIT license (<http://opensource.org/licenses/MIT>, see LICENSE.txt).

### Disclaimer
ILNEditor is in an early phase of development and should be
considered experimental.
