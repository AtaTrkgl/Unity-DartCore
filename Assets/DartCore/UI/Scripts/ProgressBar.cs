using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DartCore.UI
{
    [ExecuteInEditMode, HelpURL("https://github.com/AtaTrkgl/Unity-DartCore/wiki/DartCore.UI#4-progress-bar")]
    public class ProgressBar : MonoBehaviour
    {
#region Unity Editor
#if UNITY_EDITOR
        [MenuItem("DartCore/UI/Linear Progress Bar"), MenuItem("GameObject/UI/DartCore/Linear Progress Bar")]
        public static void AddLinearProgressBar()
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Linear Progress Bar"));
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
            obj.name = "New Linear Progress Bar";
        }

        [MenuItem("DartCore/UI/Radial Progress Bar"), MenuItem("GameObject/UI/DartCore/Radial Progress Bar")]
        public static void AddRadialProgressBar()
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Radial Progress Bar"));
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
            obj.name = "New Radial Progress Bar";
        }
#endif
        #endregion

        [Tooltip("true: limits the current between min & max")]public bool hasBoundries = true;
        public float min;
        public float max = 1;
        public float current = 0;
        [Range(0,.5f)] public float fillTime = .1f;
        public Color bgColor = Color.white;
        public Color fillerColor = Color.red;
        public bool isRadial;
        [Range(0,1)] public float innerRadius;
        public Sprite innerCircleIcon;
        public Color innerCircleIconColor;
        public Color innerCircleColor;

        private Image mask;
        private Image filler;
        private Image innerCircle;
        private Image outerCircleMask;
        private Image bgImage;
        [Range(0f,10f)] public float outerCircleRadius = 5f;
        private float currentOuterCircleRadius;

        private void Awake()
        {
            if (isRadial)
            {
                outerCircleMask = transform.Find("Masking").GetComponent<Image>();
                bgImage = outerCircleMask.transform.Find("BG").GetComponent<Image>();
                mask = outerCircleMask.transform.Find("Mask").GetComponent<Image>();
                filler = mask.transform.Find("Fill").GetComponent<Image>();
                innerCircle = transform.Find("InnerCircle").GetComponent<Image>();
                currentOuterCircleRadius = outerCircleRadius;
            }
            else
            {
                bgImage = GetComponent<Image>();
                mask = transform.Find("Mask").GetComponent<Image>();
                filler = mask.transform.Find("Fill").GetComponent<Image>();
            }
        }

        private void Update()
        {
            #region UnityEditor
#if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                bgImage.color = bgColor;
                filler.color = fillerColor;
                mask.fillAmount = (current - min)/(max - min);
                currentOuterCircleRadius = outerCircleRadius;
            }
#endif
            #endregion

            if (hasBoundries)
                current = Mathf.Clamp(current, min, max);

            bgImage.DOColor(bgColor, fillTime);
            GetCurrentFill();
            if (isRadial)
            { 
                UpdateInnerCircle();
                DOTween.To(() => currentOuterCircleRadius, x => currentOuterCircleRadius = x, outerCircleRadius, .1f);
                outerCircleMask.pixelsPerUnitMultiplier = currentOuterCircleRadius;
                outerCircleMask.SetAllDirty();
            }
        }

        private void GetCurrentFill()
        {
            float currentOffset = current - min;
            float maxOffset = max - min;

            float desiredFillAmount = currentOffset / maxOffset;
            if (mask)
                mask.DOFillAmount(desiredFillAmount, fillTime);

            filler.DOColor(fillerColor,.4f);
        }

        private void UpdateInnerCircle()
        {
            innerCircle.GetComponent<RectTransform>().localScale = Vector2.one * innerRadius;
            innerCircle.transform.Find("Icon").GetComponent<Image>().sprite = innerCircleIcon;
            innerCircle.transform.Find("Icon").GetComponent<Image>().color = innerCircleIconColor;
            innerCircle.color = innerCircleColor;
        }

        public float GetPercentage(int digits = 2)
        {
            float percentage = current * 100 / (max - min);
            return (float) Math.Round((decimal) percentage, digits);
        }
    }
}
