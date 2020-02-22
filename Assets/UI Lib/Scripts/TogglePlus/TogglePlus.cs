using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UILib
{
    [ExecuteInEditMode]
    public class TogglePlus : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
#region Unity Editor
#if UNITY_EDITOR
        [MenuItem("GameObject/UI/UI Lib/TogglePlus")]
        public static void AddTogglePlus()
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("TogglePlus"));
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
        }
#endif
#endregion

        [Header("Configuration")]
        public bool isOn = false;
        public bool isInteractive = true;
        private bool wasInteractive = true;

        public float transitionDuration = .1f;
        public Color normalColor;
        public Color highlightedColor;
        public Color disabledColor;

        [Header("Filling")]
        public float fillTransitionDuration = .1f;
        public Color fillColor = Color.red;
        public Image.FillMethod animType;

        [Header("Tooltip")]
        [SerializeField] private string toolTip;
        [SerializeField] private Color tooltipTextColor = new Color(70, 70, 70);
        [SerializeField] private Color tooltipBgColor = new Color(240, 240, 240);

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
                isOn = !isOn;
        }

        private void Highlight()
        {
            if (isInteractive)
            { 
                GetComponent<Image>().DOColor(highlightedColor, transitionDuration);
                if (toolTip.Length > 0)
                    Tooltip.ShowToolTip_Static(toolTip, tooltipTextColor, tooltipBgColor);
            }
        }
        private void NormalState()
        {
            if (isInteractive)
            { 
                GetComponent<Image>().DOColor(normalColor, transitionDuration);
                if (toolTip.Length > 0)
                    Tooltip.HideToolTip_Static();
            }
        }
        private void DisabledState()
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