using UnityEngine;
using UnityEngine.EventSystems;

namespace UILib
{
    public class TooltipTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        [Header("Tooltip")]
        public string toolTip;
        [SerializeField] private Color tooltipTextColor = new Color(.2f, .2f, .2f);
        [SerializeField] private Color tooltipBgColor = new Color(.85f, .85f, .85f);

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (toolTip.Length > 0)
                Tooltip.ShowToolTip_Static(toolTip, tooltipTextColor, tooltipBgColor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (toolTip.Length > 0)
                Tooltip.HideToolTip_Static();
        }

    }
}
