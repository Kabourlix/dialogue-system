# Dialog System - Aurore

This plugin has been created using [xNode](https://github.com/Siccity/xNode) by Siccity.

It consists in a node based editor for a dialog system. This enables you to create a dialog tree and then use it in your game.
The UI connection is already made you just have to add a prefab (Check how to setup for further information).
Thus, the UI could be styled as you want with no further implementation.

## Dependencies

The package require the use of :
- [xNode](https://github.com/Siccity/xNode) : Should be set in the plugin ;
- TMP_Pro.

## How to use it

- Create a DialogGraph asset from the menu Assets/Create/Aurore/DialogGraph ;  
- Then, open the graph (double-click) and start creating your dialog tree ;
- Once you are satisfied, add a DialogComponent to any game object in your scene that need to trigger a dialogue ;
- You shall call the function `TriggerDialog(DialogGraph graph)` or `TriggerDialogue(DialogGraph graph, AudioSource source)` on the DialogComponent to start the dialog.
- Then add the prefab of your dialog UI to a canvas ;
- To perform the Link with UI, create a class that extends `DialogueManagerMaster` or use the default one `DialogueUiManager` (check the ExampleScene).

You can checkout the example scene to see how it works in the action.






## -- Licence --  

This package has been developed by Hugo Da Maïa.

*From the original plugin*

MIT License

Copyright (c) 2017 Thor Brigsted

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

