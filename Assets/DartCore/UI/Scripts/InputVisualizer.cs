using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DartCore.Utilities;

namespace DartCore.UI
{
    [ExecuteInEditMode, RequireComponent(typeof(Image))]
    public class InputVisualizer : MonoBehaviour
    {
        public bool autoPickController = false;
        public ControllerType currentController = ControllerType.XBoxOne;
        [Range(0,3)]public int style = 0;
        public ControllerKey key;

        void Start()
        {
            if (autoPickController)
                AutoPickController();

            UpdateImage();
        }

        private void Update()
        {
            UpdateImage();
        }

        public void UpdateController(ControllerType desiredController)
        {
            currentController = desiredController;
            UpdateImage();
        }

        private void UpdateImage()
        {
            string folderName = "XBox"; // Default Folder
            if (currentController == ControllerType.Dualshock3 || currentController == ControllerType.Dualshock4)
                folderName = "PS";
            else if (currentController == ControllerType.XBoxOne || currentController == ControllerType.XBox360)
                folderName = "XBox";

            string path = "Input Icons/Controller Icons/" + folderName + "/" + key.ToString() + "_" + style;
            if (Resources.Load(path))
            {
                var image = Resources.Load<Sprite>(path);
                GetComponent<Image>().sprite = image;
                return;
            }
        }

        private void AutoPickController()
        {
            currentController = InputUtilities.GetMainController();
        }
    }
}