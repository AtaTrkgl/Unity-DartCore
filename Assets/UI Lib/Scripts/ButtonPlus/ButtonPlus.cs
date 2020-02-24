using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UILib
{
    public class ButtonPlus : Button, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Unity Editor
#if UNITY_EDITOR
        [MenuItem("GameObject/UI/UI Lib/ButtonPlus")]
        public static void AddButtonPlus()
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("ButtonPlus"));
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
            obj.name = "New Button Plus";
        }
#endif
        #endregion

        [Header("Tooltip")]
        [SerializeField] private string toolTip;
        [SerializeField] private Color tooltipTextColor = new Color(.2f, .2f, .2f);
        [SerializeField] private Color tooltipBgColor = new Color(.85f, .85f, .85f);

        [Header("Audio")]
        [SerializeField] private AudioClip highlightedClip;
        [SerializeField] private AudioClip pressedClip;
        [SerializeField] private AudioMixerGroup mixerGroup;
        [SerializeField, Range(0,1)] private float volume = .2f;

        private void Click()
        {
            if (base.interactable)
            { 
                UIAudioManager.PlayOneShotAudio(pressedClip, volume, mixerGroup);
            }
        }

        private void Exit()
        {
            if (base.interactable)
            {
                if (toolTip.Length > 0)
                    Tooltip.HideToolTip_Static();
            }
        }

        private void Highlight()
        {
            if (base.interactable)
            {
                if (toolTip.Length > 0)
                    Tooltip.ShowToolTip_Static(toolTip, tooltipTextColor, tooltipBgColor);
                UIAudioManager.PlayOneShotAudio(highlightedClip, volume, mixerGroup);
            }
        }

        #region Cursor Detection
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            Highlight();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            Exit();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            Click();
        }
        #endregion
    }
}