using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DartCore.Utilities
{
    public class InputUtilities
    {
        public static Dictionary<ControllerType, int> GetControllers()
        {
            var dict = new Dictionary<ControllerType, int>();

            foreach (string controller in Input.GetJoystickNames())
            {
                switch (controller)
                {
                    case "PlayStation 3 Controller":
                        dict = TryIncrementValue(dict, ControllerType.Dualshock3);
                        break;
                    case "Wireless Controller":
                        dict = TryIncrementValue(dict, ControllerType.Dualshock4);
                        break;
                    case "XBox 360 Controller":
                        dict = TryIncrementValue(dict, ControllerType.XBox360);
                        break;
                    case "XBox One Controller":
                        dict = TryIncrementValue(dict, ControllerType.XBoxOne);
                        break;
                    case "Logitech F310 Controller":
                        dict = TryIncrementValue(dict, ControllerType.LogitechF310);
                        break;
                    case "Logitech F510 Controller":
                        dict = TryIncrementValue(dict, ControllerType.LogitechF510);
                        break;
                    case "Logitech F710 Controller":
                        dict = TryIncrementValue(dict, ControllerType.LogitechF710);
                        break;
                    default:
                        if (!string.IsNullOrWhiteSpace(controller))
                        {
                            dict = TryIncrementValue(dict, ControllerType.Other);
                        }
                        break;
                }
            }
            return dict;
        }

        private static Dictionary<ControllerType, int> TryIncrementValue(Dictionary<ControllerType, int> dict, ControllerType controller)
        {
            if (dict.ContainsKey(controller))
                dict[controller] += 1;
            else
                dict.Add(controller, 1);

            return dict;
        }

        public static bool IsUsingController()
        {
            return !string.IsNullOrWhiteSpace(Input.GetJoystickNames()[0]);
        }
    }

    public enum ControllerType
    { 
        Dualshock3 = 0,
        Dualshock4 = 1,
        XBox360 = 2,
        XBoxOne = 3,
        LogitechF310 = 4,
        LogitechF510 = 5,
        LogitechF710 = 6,
        Other = 100,
    }
}