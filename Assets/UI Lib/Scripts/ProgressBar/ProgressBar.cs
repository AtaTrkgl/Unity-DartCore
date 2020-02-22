using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UILib
{

    [ExecuteInEditMode]
    public class ProgressBar : MonoBehaviour
    {
#region Unity Editor
#if UNITY_EDITOR
        [MenuItem("GameObject/UI/UI Lib/Linear Progress Bar")]
        public static void AddLinearProgressBar()
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Linear Progress Bar"));
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
        }

        [MenuItem("GameObject/UI/UI Lib/Radial Progress Bar")]
        public static void AddRadialProgressBar()
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Radial Progress Bar"));
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
        }
#endif
#endregion

        public float min;
        public float max = 1;
        public float current = 0;
        public Color fillerColor = Color.red;
        public bool isRadial;
        [Range(0,1)] public float innerRadius;
        public Sprite innerCircleIcon;
        public Color innerCircleIconColor;
        public Color innerCircleColor;

        [SerializeField] private Image mask;
        [SerializeField] private Image filler;
        [SerializeField] private Image innerCircle;

        private void Update()
        {
            GetCurrentFill();
            if (isRadial)
                UpdateInnerCircle();
        }

        private void GetCurrentFill()
        {
            float currentOffset = current - min;
            float maxOffset = max - min;

            float fillAmount = currentOffset / maxOffset;
            if (mask)
                mask.fillAmount = fillAmount;

            filler.DOColor(fillerColor,.4f);
        }

        private void UpdateInnerCircle()
        {
            innerCircle.GetComponent<RectTransform>().localScale = Vector2.one * innerRadius;
            innerCircle.transform.Find("Icon").GetComponent<Image>().sprite = innerCircleIcon;
            innerCircle.transform.Find("Icon").GetComponent<Image>().color = innerCircleIconColor;
            innerCircle.color = innerCircleColor;
        }
    }
}
