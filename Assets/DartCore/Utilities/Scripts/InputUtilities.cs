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

        public static ControllerType GetMainController()
        {
            if (Input.GetJoystickNames().Length <= 0)
                return ControllerType.None;

            switch (Input.GetJoystickNames()[0])
            {
                case "PlayStation 3 Controller":
                    return ControllerType.Dualshock3;
                case "Wireless Controller":
                    return ControllerType.Dualshock4;
                case "XBox 360 Controller":
                    return ControllerType.XBox360;
                case "XBox One Controller":
                    return ControllerType.XBoxOne;
                case "Logitech F310 Controller":
                    return ControllerType.LogitechF310;
                case "Logitech F510 Controller":
                    return ControllerType.LogitechF510;
                case "Logitech F710 Controller":
                    return ControllerType.LogitechF710;
                default:
                    return ControllerType.None;
            }
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
        None = 101,
    }

    public enum ControllerKey
    { 
        Trigger = 0,
        Bumper = 1,
        LeftStick = 2,
        LeftStickButton = 3,
        RightStick =  4,
        RightStickButton = 5,
        Dpad = 6,
        DpadTop = 7,
        DpadBottom = 8,
        DpadRight = 9,
        DpadLeft = 10,
        TopButton = 11,
        BottomButton = 12,
        LeftButton = 13,
        RightButton = 14
    }
}