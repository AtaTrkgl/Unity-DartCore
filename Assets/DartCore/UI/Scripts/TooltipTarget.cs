﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace DartCore.UI
{
    public class TooltipTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        [Header("Tooltip")]
        public string toolTip;
        [Tooltip("toolTip will be used as a key if set to true")] public bool localizeToolTip = false;
        [SerializeField] private Color tooltipTextColor = new Color(.2f, .2f, .2f);
        [SerializeField] private Color tooltipBgColor = new Color(.85f, .85f, .85f);

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (toolTip.Length > 0)
                Tooltip.ShowTooltipStatic(toolTip, tooltipTextColor, tooltipBgColor, localizeToolTip);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (toolTip.Length > 0)
                Tooltip.HideTooltipStatic();
        }

    }
}
