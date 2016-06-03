A simple way to handle input shortcuts in Unity3D

# InputAttributes
InputAttributes is a helper to handle input shortcuts in Unity. Though it's optimized and can be used on games it's intended to be used on non-game code(menus, ingame editors, etc.) or games that don't heavily rely on input such as turn-based games.

## Usage

 1. Copy the code to some part of your project. 
 2. Create a ```GameObject``` in your scene and add a ```InputHandler``` to it. 
 3. Add a ```InputSequence``` to any method:
    ```
    [InputSequence(Key.LeftControl, Key.Mouse1_Down)]
    void doSomething()
    {
      ...
    }
    ```

Enjoy :)

## Remarks
Each ```KeyCode``` enum is rewritten in a custom enum ```Prism.Key``` like so:

* ```GetKey(KeyCode.A)``` becomes ```Key.A```
* ```GetKeyDown(KeyCode.A)``` becomes ```Key.A_Down```
* ```GetKeyUp(KeyCode.A)``` becomes ```Key.A_Up```

Because of this you can't use custom inputs mapped in any other way.

Also it currently doesn't handle ```GameObjects``` added at runtime which have any ```Component``` with methods with the ```InputSequenceAttribute```. The way ```InputHandler``` works is by getting all GameObjects in the scene at `Start` and saving them on a List.