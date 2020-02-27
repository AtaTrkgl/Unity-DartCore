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
        public Color fillColor = Color.red;
        public Image.FillMethod animType;

        [Header("Tooltip")]
        [SerializeField] protected string toolTip;
        [SerializeField] protected Color tooltipTextColor = new Color(.2f, .2f, .2f);
        [SerializeField] protected Color tooltipBgColor = new Color(.85f, .85f, .85f);

        [Header("Audio")]
        [SerializeField] protected AudioClip highlightedClip;
        [SerializeField] protected AudioClip pressedClip;
        [SerializeField] protected AudioMixerGroup mixerGroup;
        [SerializeField, Range(0, 1)] protected float volume = .2f;

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
            UpdateFill();
            if (!isInteractive)
                DisabledState();
            if (isInteractive && !wasInteractive)
                NormalState();

            DOTween.To(() => fillAmount, x => fillAmount = x, isOn ? 1 : 0, fillTransitionDuration);
            fill.DOColor(fillColor, .4f);

            wasInteractive = isInteractive;
        }

        private void UpdateFill()
        {
            mask.fillMethod = animType;
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
}