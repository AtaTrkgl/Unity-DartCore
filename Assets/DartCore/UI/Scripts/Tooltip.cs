using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DartCore.UI
{
    public class Tooltip : MonoBehaviour
    {
        #region Unity Editor
#if UNITY_EDITOR
        [MenuItem("DartCore/UI/Tooltip"), MenuItem("GameObject/UI/DartCore/Tooltip")]
        public static void AddTogglePlus()
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Tooltip"));
            obj.transform.SetParent(Selection.activeGameObject.transform, false);
            obj.name = "Tooltip";
        }
#endif
        #endregion

        public static Tooltip instance;
        public static float screenEdgePadding = 10f;

        private TextMeshProUGUI text;
        private RectTransform bg;
        private RectTransform canvas;

        private void Awake()
        {
            instance = this;
            bg = transform.Find("bg").GetComponent<RectTransform>();
            text = transform.Find("text").GetComponent<TextMeshProUGUI>();
            canvas = GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>();

            HideToolTip();
        }

        private void Update()
        {
            Vector2 bgSize = new Vector2(text.preferredWidth, text.preferredHeight);
            bg.sizeDelta = bgSize;
            text.GetComponent<RectTransform>().sizeDelta = bgSize;

            FollowCursor();
        }

        private void ShowToolTip(string tooltipString, Color textColor, Color bgColor)
        {
            text.text = tooltipString;
            bg.GetComponent<Image>().color = bgColor;

            text.color = textColor;

            FollowCursor();
            gameObject.SetActive(true);
        }

        private void HideToolTip()
        {
            gameObject.SetActive(false);
        }

        private void FollowCursor()
        {
            float screenWidth = canvas.sizeDelta.x;
            float screenHeight = canvas.sizeDelta.y;

            // Input.MousePos returns cordinates where the bottom left 
            // side of the screen is (0,0) and the top right corner is (w * canvas scale,h * canvas scale)
            Vector2 realMosPos = (Vector2)Input.mousePosition / canvas.lossyScale;
            var desiredPos = realMosPos - new Vector2(screenWidth / 2, screenHeight / 2);

            desiredPos = new Vector2(
                Mathf.Clamp(desiredPos.x,
                -screenWidth / 2 + screenEdgePadding, // no bg size because of the pivot of it
                screenWidth / 2 - screenEdgePadding - bg.sizeDelta.x),
                Mathf.Clamp(desiredPos.y,
                -screenHeight / 2 + screenEdgePadding, // no bg size because of the pivot of it
                screenHeight / 2 - screenEdgePadding - bg.sizeDelta.y));
            GetComponent<RectTransform>().localPosition = desiredPos;
        }

        public static void ShowToolTip_Static(string tooltipString, Color textColor, Color bgColor)
            => instance.ShowToolTip(tooltipString, textColor, bgColor);
        public static void HideToolTip_Static()
        { 
            if (instance)
                 instance.HideToolTip();
        }
    }
}