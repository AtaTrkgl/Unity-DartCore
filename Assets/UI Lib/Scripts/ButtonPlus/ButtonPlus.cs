using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UILib
{
    public class ButtonPlus : Button, IPointerEnterHandler, IPointerExitHandler
    {
        #region Unity Editor
#if UNITY_EDITOR
        [MenuItem("GameObject/UI/UI Lib/ButtonPlus")]
        public static void AddButtonPlus()
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("ButtonPlus"));
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
        }
#endif
        #endregion

        [Header("Extras")]
        [SerializeField] private string toolTip;
        [SerializeField] private Color textColor = new Color(70, 70, 70);
        [SerializeField] private Color bgColor = new Color(240, 240, 240);

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (toolTip.Length > 0)
                Tooltip.ShowToolTip_Static(toolTip, textColor, bgColor);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (toolTip.Length > 0)
                Tooltip.HideToolTip_Static();
        }
    }
}