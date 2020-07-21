using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DartCore.Localization;

namespace DartCore.UI
{
    [HelpURL("https://github.com/AtaTrkgl/Unity-DartCore/wiki/DartCore.UI#5-tooltip")]
    public class Tooltip : MonoBehaviour
    {
        public static Tooltip instance;
        public static float screenEdgePadding = 10f;
        public static int tooltipCanvasSortOrder = 100;

        private static Color textColor;
        private static Color bgColor;
        private static bool localizeText;
        private static string tooltipString;

        private TextMeshProUGUI text;
        private RectTransform bg;
        private RectTransform canvas;

        private RectTransform rect;
        
        private void Awake()
        {
            instance = this;
            rect = GetComponent<RectTransform>();
            bg = transform.Find("bg").GetComponent<RectTransform>();
            text = transform.Find("text").GetComponent<TextMeshProUGUI>();

            Localizator.OnLanguageChange += UpdateTooltip;
        }

        private void Start()
        {
            canvas = transform.parent.GetComponent<RectTransform>();
        }

        private void Update()
        {
            UpdateTextSize();
            FollowCursor();
        }

        private void UpdateTextSize()
        {
            Vector2 bgSize = new Vector2(text.preferredWidth, text.preferredHeight);
            bg.sizeDelta = bgSize;
            text.GetComponent<RectTransform>().sizeDelta = bgSize;
        }

        private void ShowTooltip(string tooltipString, Color textColor, Color bgColor, bool localizeText = false)
        {
            Tooltip.textColor = textColor;
            Tooltip.bgColor = bgColor;
            Tooltip.localizeText = localizeText;
            Tooltip.tooltipString = tooltipString;

            instance.UpdateTooltip();
            UpdateTextSize();
            FollowCursor();
            gameObject.SetActive(true);
        }

        private void UpdateTooltip()
        {
            bg.GetComponent<Image>().color = bgColor;
            text.color = textColor;
            text.text = localizeText ? Localizator.GetString(tooltipString) : tooltipString;
        }

        private void HideTooltip()
        {
            gameObject.SetActive(false);
        }

        private void FollowCursor()
        {
            if (!canvas)
                canvas = transform.parent.GetComponent<RectTransform>();

            var screenWidth = canvas.sizeDelta.x;
            var screenHeight = canvas.sizeDelta.y;

            // Input.MousePos returns cordinates where the bottom left 
            // side of the screen is (0,0) and the top right corner is (w * canvas scale,h * canvas scale)
            Vector2 realMosPos = (Vector2)Input.mousePosition / canvas.lossyScale;
            var desiredPos = realMosPos - new Vector2(screenWidth * .5f, screenHeight * .5f);

            desiredPos = new Vector2(
                Mathf.Clamp(desiredPos.x,
                -screenWidth / 2 + screenEdgePadding, // no bg size because of the pivot of it
                screenWidth / 2 - screenEdgePadding - bg.sizeDelta.x),
                Mathf.Clamp(desiredPos.y,
                -screenHeight / 2 + screenEdgePadding, // no bg size because of the pivot of it
                screenHeight / 2 - screenEdgePadding - bg.sizeDelta.y));
            rect.localPosition = desiredPos;
        }

        public static void ShowTooltipStatic(string tooltipString, Color textColor, Color bgColor, bool localizeText = false)
        {
            CheckInstance();

            instance.ShowTooltip(tooltipString, textColor, bgColor, localizeText);
        }
        public static void HideTooltipStatic()
        {
            CheckInstance();

            instance.HideTooltip();
        }
        public static void UpdateTooltipStatic()
        {
            CheckInstance();

            instance.UpdateTooltip();
        }

        private static void CheckInstance()
        {
            if (!instance)
            {
                var toolTipCanvas = GameObject.FindGameObjectWithTag("Tooltip Canvas");
                if (toolTipCanvas)
                {
                    var tooltipChild = toolTipCanvas.transform.GetChild(0).GetComponent<Tooltip>();
                    if (tooltipChild)
                    { 
                        instance = tooltipChild;
                        instance.gameObject.SetActive(true);
                        return;
                    }
                    else
                        Debug.LogError("Tooltip Canvas has an unknown child");
                }

                var obj = Instantiate(Resources.Load<GameObject>("Tooltip")) as GameObject;
                obj.name = "Tooltip";
                instance = obj.GetComponent<Tooltip>();

                var canvas = Instantiate(Resources.Load<GameObject>("TooltipCanvas")) as GameObject;
                canvas.name = "Tooltip Canvas";
                instance.transform.SetParent(canvas.transform, false);
                canvas.GetComponent<Canvas>().sortingOrder = tooltipCanvasSortOrder;
            }
        }

        public void ChangeFont(TMP_FontAsset desiredFont)
        {
            CheckInstance();

            instance.transform.Find("text").GetComponent<TextMeshProUGUI>().font = desiredFont;
        }
    }
}