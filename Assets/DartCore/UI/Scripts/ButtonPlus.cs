using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DartCore.UI
{
    public class ButtonPlus : Button, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Unity Editor
#if UNITY_EDITOR
        [MenuItem("DartCore/UI/ButtonPlus"), MenuItem("GameObject/UI/DartCore/ButtonPlus")]
        public static void AddButtonPlus()
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("ButtonPlus"));
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
            obj.name = "New Button Plus";
        }
#endif
        #endregion

        [Header("Tooltip")]
        public string toolTip;
        public Color tooltipTextColor = new Color(.2f, .2f, .2f);
        public Color tooltipBgColor = new Color(.85f, .85f, .85f);
        [Tooltip("toolTip will be used as a key if set to true")] public bool localizeText = false;

        [Header("Audio")]
        public AudioClip highlightedClip;
        public AudioClip pressedClip;
        public AudioMixerGroup mixerGroup;
        [Range(0,1)] public float volume = .2f;

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
                    Tooltip.HideTooltipStatic();
            }
        }

        private void Highlight()
        {
            if (base.interactable)
            {
                if (toolTip.Length > 0)
                    Tooltip.ShowTooltipStatic(toolTip, tooltipTextColor, tooltipBgColor, localizeText);
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