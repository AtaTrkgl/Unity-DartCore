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
        public static float screenEdgePadding = 10;

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
            float maxX = canvas.sizeDelta.x / 2 * canvas.lossyScale.x;
            float maxY = canvas.sizeDelta.y / 2 * canvas.lossyScale.y;

            // Input.MousePos returns cordinates where the bottom left 
            // side of the screen is 0,0 and the top right part is w,h
            var desiredPos = (Vector2) Input.mousePosition - new Vector2(maxX, maxY);

            desiredPos = new Vector2(
                Mathf.Clamp(desiredPos.x, -maxX + screenEdgePadding, maxX - bg.sizeDelta.x - screenEdgePadding),
                Mathf.Clamp(desiredPos.y, -maxY + screenEdgePadding, maxY - bg.sizeDelta.y - screenEdgePadding));
            transform.localPosition = desiredPos;
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