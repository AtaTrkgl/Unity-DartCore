# DartCore

*DartCore* is a unity library for game development which includes essential features like localization, UI and much more. To use this package, you'll need to import the Assets/DartCore file into your project.
*DartCore* is split into multiple namespaces for ease of use, each of them are documented bellow.

## 1. DartCore.Localization


#### 1.1 Adding and Removing Language Files
To manage the languages, click "DartCore/Localization/Add or Remove Languages" from the toolbar

![image of the toolbar](https://imgur.com/2Ou4wWZ.png) 

After pressing it, the "*Language File Manager*" will pop up. From there, you can press on the language buttons of your choice to remove it. The section bellow the current languages allows you to add new languages to your project easly. There are 4 fields to fill to create a add a new language.
1. "Language to add" : Language to add
2. "File Name" : Name of the file where the data will be stored (don't specify the extention like .txt or .csv
3. "Language Name" : The name of the language in that language (Deutch for German, Türkçe for Turkish)
4. "Language Error Message" : The error message in that language in case something goes wrong

![image of the Language File Manager](https://imgur.com/KXi8VY1.png)


#### 1.2 Adding, Removing and Editing Keys
To add, remove or edit keys we need to access the *Key Editor* window. for that click "DartCore/Localization/Key Editor" from the toolbar

![image of the toolbar](https://imgur.com/2Ou4wWZ.png) 

When you first open the *Key Editor* you'll get a screen with a text area and a button, in that text area you can either write a new key and press the *"Add new Key"* button or search for an existing key and edit its values.

![image of the Key Editor](https://imgur.com/jVWKD0t.png)

To check which keys have been localized you can open the *"Key Browser"* via "DartCore/Localization/Key Browser". There you can search for keys and see which ones localized to which languages. Green means localized, while red means the opposite.

![image of the Key Browser](https://imgur.com/GYHeYpL.png)


#### 1.3 Localizator.GetCurrentLanguage()
This function returns the current language.

Example Usage:
``` C#
SystemLanguage currentLanguage = Localization.GetCurrentLanguage();
```


#### 1.4 Localizator.GetString()
This function is used for getting the localized value of a given key. It has 2 overloads:
- `GetString(string key, bool returnErrorString = true)`
- `GetString(string key, SystemLanguage language, bool returnErrorString = true)`

if the parameter *returnErrorString* is set to *true* the function will return the value of the *"lng_error"* key if the given key is invalid or not localized in the current language. If it is set to *false*, it will return and empty string(`""`) instead of an error message.

Example Usage for getting the value of the *"test_key"* key in the current language:
``` C#
string value = Localization.GetString(test_key);
```
getting the same value in French:
``` C#
string value = Localization.GetString(test_key, SystemLanguage.French);
```

#### 1.5 Localizator.UpdateLanguage()
This function is used for changing the current language. It takes one argument which is the desired language. If that language is available it will return *true* and change the language, however, if the language is not available, it will return *false*.

Example Usage for changing the language to Turkish:
``` C#
Localizator.UpdateLanguage(SystemLanguage.Turkish);
```

#### 1.6 Localizator.SetLanguageAccordingToSystem()
This function will set the current language to the systems language if it is available.

Example Usage:
``` C#
Localizator.SetLanguageAccordingToSystem();
```


## 2. DartCore.UI

You can create the following UI widgets by right clicking on the hierarchy UI/DartCore.
#### 2.1 ButtonPlus
ButtonPlus functions same as the standart button of the Unity Engine. It has 2 extra features, a tooltip and sound effects.
![image of the ButtonPlus](https://imgur.com/OMbHuK8.png)
Properties:
1. **Tooltip**:
1.1. **Tooltip Text**: The text of the tooltip. If left blank, tooltip will be disabled.
1.2. **Tooltip Text Color**: Text color of the tooltip.
1.3. **Tooltip BG Color**: Text bg of the tooltip.
2. **Audio**:
2.1. **Highlighted Clip**: *AudioClip* that will be played on cursor enter.
2.2. **Pressed Clip**: *AudioClip* that will be played on cursor click.
2.3. **Volume**: Volume of the sound effects between 0 and 1.
2.4. **Audio Mixer Group**: The mixergroup of the sound effects (not mandatory for SFXs).

You can access to all of these properties programmatically.

Example usage:
``` C#
string desiredToolTip = "This is the desired tooltip text";
GameObject.FindObjectOfType<ButtonPlus>().toolTip = desiredToolTip;
```

#### 2.2 TogglePlus
TogglePlus functions same as the standart toggle of the Unity Engine with a lot of new customization options.
![image of the TogglePlus](https://imgur.com/GAwbhZV.png)
Properties:
1. **Colors**:
1.1 **Normal Color**: Color of the background when not pressed or highligted.
1.2 **Transition Duration**: Transition time between background colors.
1.3 **Highligted Color**: Color of the background when the cursor is on the toggle.
1.4 **Disabled Color**: Color of the background when the *isOn* boolean is set to *false*.
2. **Filing**:
2.1. **Fill Transition Duration**: The time it takes to transition between the on and off states.
2.2. **Fill Color**: Color of the filler.
2.3. **Color Transition Duration**: The time it takes to transition between fill colors.
2.4. **Anim Type**: Animation of the filling.
2.5. **Fill Scale**: Scale of the fill between 0 and 1.
3. **Tooltip**:
3.1. **Tooltip Text**: The text of the tooltip. If left blank, tooltip will be disabled.
3.2. **Tooltip Text Color**: Text color of the tooltip.
3.3. **Tooltip BG Color**: Text bg of the tooltip.
4. **Audio**:
4.1. **Highlighted Clip**: *AudioClip* that will be played on cursor enter.
4.2. **Pressed Clip**: *AudioClip* that will be played on cursor click.
4.3. **Volume**: Volume of the sound effects between 0 and 1.
4.4. **Audio Mixer Group**: The mixergroup of the sound effects (not mandatory for SFXs).

You can access to all of these properties programmatically.

Example usage:
``` C#
float desiredTransitionDuration = .2f;
GameObject.FindObjectOfType<TogglePlus>().fillTransitionDuration = desiredTransitionDuration;
```
#### 2.3 Switch
Not yet documented.
#### 2.4 Linear Progress Bar
Not yet documented.
#### 2.5 Radial Progress Bar
Not yet documented.
#### 2.6 Tooltip
Not yet documented.
#### 2.7 Graph
Not yet documented.
#### 2.8 Draggable Window Container
Not yet documented.
## 3. DartCore.Utilities

#### 3.1 Utils.Average()
This function returns the arithmetic mean of a given array or list of *ints* or *floats* as a *float*.

Example usage with an array:
``` C#
int[] nums = new int[]{1,6,9,12,7};
float average = Utils.Average(nums);
Debug.Log(nums); // prints out 7
```

Example usage with a list:
``` C#
List<float> nums = new List<float>(){1f, 6f, 9f, 12f, 7f};
float average = Utils.Average(nums);
Debug.Log(nums); // prints out 7
```

#### 3.2 Utils.UnitCirclePositionRadians()
This function takes two arguments, first one is the angle in radians and the second one is the radius with the default value of 1. Returns the position as a *Vector2*.

Example Usage:
``` C#
Vector2 position = Utils.UnitCirclePositionRadians(Mathf.PI/3, 1f);
Debug.Log(position); // prints out (0.86602540378, 0.5)
```

#### 3.3 Utils.UnitCirclePositionDegrees()
This function functions same as the *Utils.UnitCirclePositionRadians()*, however, it uses degrees instead of radians

Example Usage:
``` C#
Vector2 position = Utils.UnitCirclePositionRadians(60f, 1f);
Debug.Log(position); // prints out (0.86602540378, 0.5)
```

#### 3.4 Utils.RandomChance() 
This function takes two arguments, a numerator and a denominator. It will return true with a chance of numerator/denominator. The denominators default value is 100. 

Example Usage 1:
``` C#
if (Utils.RandomChance(30f))
    Debug.Log("You'll get this output 30% of the time");
else
    Debug.Log("You'll get this output 70% of the time");
```
Example Usage 2:
``` C#
if (Utils.RandomChance(1f, 3f))
    Debug.Log("You'll get this output one thirds of the time");
else
    Debug.Log("You'll get this output two thirds of the time");
```

#### 3.5 Utils.RandomVector2()
This function will return a random normalized *Vector2*.

Example Usage:
``` C#
Vector2 randVec2 = Utils.RandomVector2();
```

#### 3.6 Utils.RandomVector3()
This function will return a random normalized *Vector3*.

Example Usage:
``` C#
Vector3 randVec3 = Utils.RandomVector3();
```

#### 3.7 Utils.RandomVector4()
This function will return a random normalized *Vector4*.

Example Usage:
``` C#
Vector4 randVec4 = Utils.RandomVector4();
```

#### 3.8 Utils.IsInRange()
This function will check if the given number is inside the provided bounds. It takes three arguments, the first one being the number and the other being the bounds. Order of the bounds does not matter, if they are equal to each other function will check if the number is equal to the bounds.

Example Usage:
``` C#
if(Utils.IsInRange(5,0,7))
    Debug.Log("5 is inside the given range");
else
    Debug.Log("5 is not inside the given range");
// Out put is "5 is inside the given range"
```