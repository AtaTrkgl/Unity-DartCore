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
    [ExecuteInEditMode]
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

        [Header("Colors")]
        public Color normalColor;
        [Range(0f,1f)] public float transitionDuration = .1f;
        public Color highlightedColor;
        public Color disabledColor;

        [Header("Filling")]
        public float fillTransitionDuration = .1f;
        private float fillTransDur;
        public Color fillColor = Color.red;
        public float colorTransitionDuration = .1f;
        public ToggleFillAnimation animType;
        [Range(0, 1)] public float fillScale = .8f;

        [Header("Tooltip")]
        public string toolTip;
        public Color tooltipTextColor = new Color(.2f, .2f, .2f);
        public Color tooltipBgColor = new Color(.85f, .85f, .85f);

        [Header("Audio")]
        public AudioClip highlightedClip;
        public AudioClip pressedClip;
        public AudioMixerGroup mixerGroup;
        [Range(0, 1)] public float volume = .2f;

        private Image mask;
        private Image fill;
        private float fillAmount = 0;

        private void Awake()
        {
            mask = transform.Find("Mask").GetComponent<Image>();
            fill = mask.transform.Find("Fill").GetComponent<Image>();
            fillAmount = isOn ? 1 : 0;
            NormalState();
        }

        private void Update()
        {
            #region UnityEditor
#if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                fill.color = fillColor;
                fillAmount = isOn ? 1 : 0;
            }
#endif
            #endregion

            fillTransDur = fillTransitionDuration;

            UpdateFill();
            if (!isInteractive)
                DisabledState();
            if (isInteractive && !wasInteractive)
                NormalState();

            DOTween.To(() => fillAmount, x => fillAmount = x, isOn ? 1 : 0, fillTransDur);
            if (isOn)
                fill.DOColor(fillColor, animType == ToggleFillAnimation.Fade ? fillTransDur : colorTransitionDuration);

            wasInteractive = isInteractive;
        }

        private void UpdateFill()
        {
            fillScale = Mathf.Clamp(fillScale, 0f, 1f);

            mask.GetComponent<RectTransform>().localScale = Vector2.one * fillScale;
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
                    if (!isOn)
                        fill.DOColor(Color.clear, fillTransDur);
                    break;
                case ToggleFillAnimation.None:
                    fillTransDur = 0f;
                    break;
                default:
                    break;
            }
            if (animType != ToggleFillAnimation.Fade)
                mask.fillAmount = fillAmount;
        }

        private void Click()
        {
            if (isInteractive)
            { 
                isOn = !isOn;
                OnToggle.Invoke();
                UIAudioManager.PlayOneShotAudio(pressedClip, volume, mixerGroup);
            }
        }

        private void Highlight()
        {
            if (isInteractive)
            { 
                GetComponent<Image>().DOColor(highlightedColor, transitionDuration);
                if (toolTip.Length > 0)
                    Tooltip.ShowToolTip_Static(toolTip, tooltipTextColor, tooltipBgColor);
                UIAudioManager.PlayOneShotAudio(highlightedClip, volume, mixerGroup);
            }
        }
        protected void NormalState()
        {
            if (isInteractive)
            { 
                GetComponent<Image>().DOColor(normalColor, transitionDuration);
                if (toolTip.Length > 0)
                    Tooltip.HideToolTip_Static();
            }
        }
        protected void DisabledState()
        {
            if (!isInteractive)
                GetComponent<Image>().DOColor(disabledColor, transitionDuration);
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