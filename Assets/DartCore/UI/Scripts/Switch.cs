using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace DartCore.UI
{
    [HelpURL("https://github.com/AtaTrkgl/Unity-DartCore/wiki/DartCore.UI#3-switch")]
    public class Switch : DartCore.UI.TogglePlus
    {
        #region Unity Editor

#if UNITY_EDITOR
        [MenuItem("DartCore/UI/Switch"), MenuItem("GameObject/UI/DartCore/Switch")]
        public static void AddSwitch()
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Switch"));
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
            obj.name = "New Switch";
        }
#endif

        #endregion

        public Color bgColorOn = Color.green;
        public Color bgColorOff = Color.white;

        private RectTransform circle;
        private RectTransform rectTrans;
        private Image bgFill;
        private Image bg;
        private float circleX;
        private readonly float padding = 1.5f;

        private void Awake()
        {
            base.NormalState();

            rectTrans = GetComponent<RectTransform>();
            circle = transform.Find("Circle").GetComponent<RectTransform>();
            bgFill = transform.Find("BG Fill Mask").Find("BG Fill").GetComponent<Image>();
            bg = transform.Find("BG").GetComponent<Image>();
        }

        private void Update()
        {
            #region UnityEditor

#if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                bgFill.fillAmount = isOn ? 1 : 0;
                bg.color = bgColorOn;
                bgFill.color = bgColorOff;
            }
#endif

            #endregion

            if (!isInteractive)
                DisabledState();
            if (isInteractive && !wasInteractive)
                NormalState();

            circleX = rectTrans.sizeDelta.x * .5f - circle.sizeDelta.x * .5f - padding;
            circle.DOLocalMoveX(isOn ? circleX : -circleX, .1f).SetUpdate(true);
            bgFill.DOFillAmount(isOn ? 1 : 0, .2f).SetUpdate(true);

            bg.DOColor(bgColorOn, transitionDuration).SetUpdate(true);
            bgFill.DOColor(bgColorOff, transitionDuration).SetUpdate(true);

            wasInteractive = isInteractive;
        }
    }
}