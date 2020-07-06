using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DartCore.UI
{
    [RequireComponent(typeof(TMP_Text)), HelpURL("https://github.com/AtaTrkgl/Unity-DartCore/wiki/DartCore.UI#9-hyperlink")]
    public class HyperLink : MonoBehaviour, IPointerClickHandler
    {
        [Header("Link Customization")]
        public Color linkColor = Color.green;
        public bool calculateAlpha = false;
        public bool addUnderLines = true;
        public bool makeBold = false;

        [Space]

        [SerializeField] public Link[] links;

        private TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            UpdateLinkColors();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, eventData.position, null);

            if (linkIndex != -1)
            {
                var linkInfo = text.textInfo.linkInfo[linkIndex];
                foreach (Link link in links)
                {
                    if (link.name == linkInfo.GetLinkID())
                    {
                        link.onClick.Invoke();
                        return;
                    }
                }
            }
        }

        public void UpdateLinkColors()
        {
            int[] indexes = new int[links.Length];
            bool[] foundLink = new bool[links.Length];
            for (int i = 0; i < links.Length; i++)
            {
                foundLink[i] = true;
                indexes[i] = 0;
            }

            string start;
            if (!calculateAlpha)
                start = $"<color=#{ColorUtility.ToHtmlStringRGB(linkColor)}ff>";
            else
                start = $"<color=#{ColorUtility.ToHtmlStringRGBA(linkColor)}>";

            string end = "</color>";

            if (makeBold)
            {
                start = start.Insert(0, "<b>");
                end = end.Insert(0, "</b>");
            }
            if (addUnderLines)
            {
                // adding the underline inside colors so that the
                // underlines are not set white.
                start = start.Insert(start.Length, "<u>");
                end = end.Insert(end.Length, "</u>");
            }

            if (!text)
                text = GetComponent<TextMeshProUGUI>();

            while (ContainsTrue(foundLink))
            {
                for (int i = 0; i < links.Length; i++)
                {
                    if (!foundLink[i])
                        continue;

                    // Locating the link
                    string value = text.text;

                    string desiredStart = $"<link={links[i].name}>";
                    string desiredEnd = "</link>";

                    int startIndex = value.IndexOf(desiredStart, indexes[i]);
                    if (startIndex == -1)
                    {
                        foundLink[i] = false;
                        continue;
                    }
                    if (startIndex - start.Length >= 0)
                    {
                        if (value.Substring(startIndex - start.Length, start.Length) == start)
                        {
                            foundLink[i] = false;
                            continue;
                        }
                    }

                    int endIndex = value.IndexOf(desiredEnd, startIndex + desiredStart.Length);
                    if (endIndex == -1)
                    {
                        foundLink[i] = false;
                        continue;
                    }
                    // Customizing the link

                    value = value.Insert(endIndex, end);
                    value = value.Insert(startIndex, start);

                    text.text = value;
                    indexes[i] = endIndex + start.Length + end.Length;
                }
            }
        }

        private bool ContainsTrue(bool[] array)
        {
            foreach (var item in array)
                if (item)
                    return true;

            return false;
        }

        [System.Serializable]
        public struct Link
        {
            public string name;
            public UnityEvent onClick;
        }
    }
}