using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DartCore.Utilities;

namespace DartCore.UI
{
    [ExecuteInEditMode]
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
            string path = "Input Icons/Controller Icons/" + currentController.ToString() + "/" + key.ToString() + "_" + style;
            if (Resources.Load(path))
            {
                var image = Resources.Load<Sprite>(path);
                GetComponent<Image>().sprite = image;
            }
        }

        private void AutoPickController()
        {
            currentController = InputUtilities.GetMainController();
        }
    }
}