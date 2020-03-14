# DartCore

*DartCore* is a unity library for game development which includes essential features like localization, UI and much more. To use this package, you'll need to import the Assets/DartCore file into your project.
*DartCore* is split into multiple namespaces for ease of use, each of them are documented bellow.

## DartCore.Localization


#### Adding and Removing Language Files
To manage the languages, click "DartCore/Localization/Add or Remove Languages" from the toolbar

![image of the toolbar](https://imgur.com/2Ou4wWZ.png) 

After pressing it, the "*Language File Manager*" will pop up. From there, you can press on the language buttons of your choice to remove it. The section bellow the current languages allows you to add new languages to your project easly. There are 4 fields to fill to create a add a new language.
1. "Language to add" : Language to add
2. "File Name" : Name of the file where the data will be stored (don't specify the extention like .txt or .csv
3. "Language Name" : The name of the language in that language (Deutch for German, Türkçe for Turkish)
4. "Language Error Message" : The error message in that language in case something goes wrong

![image of the Language File Manager](https://imgur.com/KXi8VY1.png)


#### Adding, Removing and Editing Keys
To add, remove or edit keys we need to access the *Key Editor* window. for that click "DartCore/Localization/Key Editor" from the toolbar

![image of the toolbar](https://imgur.com/2Ou4wWZ.png) 

When you first open the *Key Editor* you'll get a screen with a text area and a button, in that text area you can either write a new key and press the *"Add new Key"* button or search for an existing key and edit its values.

![image of the Key Editor](https://imgur.com/jVWKD0t.png)

To check which keys have been localized you can open the *"Key Browser"* via "DartCore/Localization/Key Browser". There you can search for keys and see which ones localized to which languages. Green means localized, while red means the opposite.

![image of the Key Browser](https://imgur.com/GYHeYpL.png)


#### Localizator.GetCurrentLanguage()
This function returns the current language.

Example Usage:
``` C#
SystemLanguage currentLanguage = Localization.GetCurrentLanguage();
```


#### Localizator.GetString()
This function is used for getting the localized string. It has 2 overloads:
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

#### Localizator.UpdateLanguage()
This function is used for changing the current language. It takes one argument which is the desired language. If that language is available it will return *true* and change the language, however, if the language is not available, it will return *false*.

Example Usage for changing the language to Turkish:
``` C#
Localizator.UpdateLanguage(SystemLanguage.Turkish);
```

#### Localizator.SetLanguageAccordingToSystem()
This function will set the current language to the systems language if it is available.

Example Usage:
``` C#
Localizator.SetLanguageAccordingToSystem();
```
