using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DartCore
{
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
        private Image bgFill;
        private Image bg;
        private float circleX;
        private float padding = 1.5f;

        private void Awake()
        {
            base.NormalState();
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

            circleX = GetComponent<RectTransform>().sizeDelta.x / 2 - circle.sizeDelta.x/2 - padding;
            circle.DOLocalMoveX(isOn ? circleX : -circleX, .1f);
            bgFill.DOFillAmount(isOn ? 1 : 0, .2f);

            bg.DOColor(bgColorOn, transitionDuration);
            bgFill.DOColor(bgColorOff, transitionDuration);

            wasInteractive = isInteractive;
        }
    }
}