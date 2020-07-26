using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace DartCore.UI
{
    [ExecuteInEditMode, HelpURL("https://github.com/AtaTrkgl/Unity-DartCore/wiki/DartCore.UI#2-toggleplus")]
    public class TogglePlus : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region Unity Editor

#if UNITY_EDITOR
        [MenuItem("DartCore/UI/Toggle Plus"), MenuItem("GameObject/UI/DartCore/Toggle Plus")]
        public static void AddTogglePlus()
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("TogglePlus"));
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
            obj.name = "New Toggle Plus";
        }
#endif

        #endregion

        public bool isOn = false;
        public bool isInteractive = true;
        protected bool wasInteractive = true;

        [SerializeField] private UnityEvent OnToggle;

        [Header("Colors")] public Color normalColor;
        public Color highlightedColor;
        public Color disabledColor;
        [Range(0f, 1f)] public float transitionDuration = .1f;

        [Header("Filling")] public float fillTransitionDuration = .1f;
        private float fillTransDur;
        public Color fillColor = Color.red;
        public float colorTransitionDuration = .1f;
        public ToggleFillAnimation animType;
        [Range(0, 1)] public float fillScale = .8f;

        [Header("Tooltip")] public string toolTip;

        [Tooltip("toolTip will be used as a key if set to true")]
        public bool localizeTooltip = false;

        public Color tooltipTextColor = new Color(.2f, .2f, .2f);
        public Color tooltipBgColor = new Color(.85f, .85f, .85f);

        [Header("Audio")] public AudioClip highlightedClip;
        public AudioClip pressedClip;
        public AudioMixerGroup mixerGroup;
        [Range(0, 1)] public float volume = .2f;

        private Image mask;
        private Image fill;

        private RectTransform maskRect;
        private Image image;

        private void Awake()
        {
            mask = transform.Find("Mask").GetComponent<Image>();
            maskRect = mask.GetComponent<RectTransform>();
            image = GetComponent<Image>();

            fill = mask.transform.Find("Fill").GetComponent<Image>();
            NormalState();
        }

        private void Update()
        {
            #region UnityEditor

#if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                fill.color = fillColor;
            }
#endif

            #endregion

            UpdateFill();
            if (!isInteractive)
                DisabledState();
            if (isInteractive && !wasInteractive)
                NormalState();

            if (isOn)
                fill.DOColor(fillColor, animType == ToggleFillAnimation.Fade ? fillTransDur : colorTransitionDuration).SetUpdate(true);

            wasInteractive = isInteractive;
        }

        private void UpdateFill()
        {
            fillScale = Mathf.Clamp(fillScale, 0f, 1f);

            if (maskRect) maskRect.localScale = Vector2.one * fillScale;
            switch (animType)
            {
                case ToggleFillAnimation.Horizontal:
                    fillTransDur = fillTransitionDuration;
                    mask.fillMethod = Image.FillMethod.Horizontal;
                    break;
                case ToggleFillAnimation.Vertical:
                    fillTransDur = fillTransitionDuration;
                    mask.fillMethod = Image.FillMethod.Vertical;
                    break;
                case ToggleFillAnimation.Radial90:
                    fillTransDur = fillTransitionDuration;
                    mask.fillMethod = Image.FillMethod.Radial90;
                    break;
                case ToggleFillAnimation.Radial180:
                    fillTransDur = fillTransitionDuration;
                    mask.fillMethod = Image.FillMethod.Radial180;
                    break;
                case ToggleFillAnimation.Radial360:
                    fillTransDur = fillTransitionDuration;
                    mask.fillMethod = Image.FillMethod.Radial360;
                    break;
                case ToggleFillAnimation.Fade:
                    fillTransDur = fillTransitionDuration;
                    mask.fillAmount = 1;
                    if (!isOn)
                        fill.DOColor(Color.clear, fillTransDur).SetUpdate(true);
                    break;
                case ToggleFillAnimation.None:
                    fillTransDur = 0f;
                    break;
                default:
                    break;
            }

            if (animType != ToggleFillAnimation.Fade)
                mask.DOFillAmount(isOn ? 1 : 0, fillTransDur).SetUpdate(true);
        }

        private void Click()
        {
            if (!isInteractive) return;
            
            isOn = !isOn;
            OnToggle.Invoke();
            UIAudioManager.PlayOneShotAudio(pressedClip, volume, mixerGroup);
        }

        private void Highlight()
        {
            if (!isInteractive) return;
            
            GetComponent<Image>().DOColor(highlightedColor, transitionDuration).SetUpdate(true);
            if (toolTip.Length > 0)
                Tooltip.ShowTooltipStatic(toolTip, tooltipTextColor, tooltipBgColor, localizeTooltip);
            UIAudioManager.PlayOneShotAudio(highlightedClip, volume, mixerGroup);
        }

        protected void NormalState()
        {
            if (!isInteractive) return;
            
            image.DOColor(normalColor, transitionDuration).SetUpdate(true);
            if (toolTip.Length > 0)
                Tooltip.HideTooltipStatic();
        }

        protected void DisabledState()
        {
            if (!isInteractive)
                image.DOColor(disabledColor, transitionDuration).SetUpdate(true);
        }

        #region Cursor Detection

        public void OnPointerClick(PointerEventData eventData) => Click();
        public void OnPointerEnter(PointerEventData eventData) => Highlight();
        public void OnPointerExit(PointerEventData eventData) => NormalState();

        #endregion
    }

    public enum ToggleFillAnimation : byte
    {
        Horizontal = 0,
        Vertical = 1,
        Radial90 = 2,
        Radial180 = 3,
        Radial360 = 4,
        Fade = 5,
        None = 6,
    }
}