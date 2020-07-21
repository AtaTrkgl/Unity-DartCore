using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DartCore.UI
{
    [HelpURL("https://github.com/AtaTrkgl/Unity-DartCore/wiki/DartCore.UI#8-draggable-window")]
    public class DraggableWindow : MonoBehaviour
    {
        public bool isCursorOn = false;

        private GraphicRaycaster raycaster;

        private void Awake()
        {
            raycaster = GameObject.FindObjectOfType<GraphicRaycaster>().GetComponent<GraphicRaycaster>();
        }

        private void Update()
        {
            CheckCursorPos();
        }

        private void CheckCursorPos()
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();

            pointerData.position = Input.mousePosition;
            raycaster.Raycast(pointerData, results);

            if (results.Count > 0)
            {
                if (results[0].gameObject == gameObject)
                {
                    isCursorOn = true;
                    return;
                }
            }

            isCursorOn = false;
        }
    }
}