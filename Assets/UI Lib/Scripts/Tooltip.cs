using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UILib
{
    public class Tooltip : MonoBehaviour
    {
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
            FollowCursor();
        }

        private void ShowToolTip(string tooltipString, Color textColor, Color bgColor)
        {
            text.text = tooltipString;
            Vector2 bgSize = new Vector2(text.preferredWidth, text.preferredHeight);
            bg.sizeDelta = bgSize;
            bg.GetComponent<Image>().color = bgColor;

            text.GetComponent<RectTransform>().sizeDelta = bgSize;
            text.color = textColor;

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
        public static void HideToolTip_Static() => instance.HideToolTip();
    }
}