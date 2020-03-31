using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
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
        foreach (var link in links)
        {
            // Locating the link
            string value = text.text;

            string desiredStart = $"<link={link.name}>";
            string desiredEnd = "</link>";

            int startIndex = value.IndexOf(desiredStart);
            int endIndex = value.IndexOf(desiredEnd, startIndex + desiredStart.Length) + desiredEnd.Length;

            // Customizing the link

            string start;
            if (calculateAlpha)
                start = $"<color=#{ColorUtility.ToHtmlStringRGB(linkColor)}ff>";
            else
                start = $"<color=#{ColorUtility.ToHtmlStringRGBA(linkColor)}>";

            string end = "</color>";

            if (makeBold)
            {
                start = start.Insert(0,"<b>");
                end = end.Insert(0,"</b>");
            }
            if (addUnderLines)
            {
                // adding the underline inside colors so that the
                // underlines are not set white.
                start = start.Insert(start.Length, "<u>");
                end = end.Insert(end.Length, "</u>");
            }

            value = value.Insert(endIndex, end);
            value = value.Insert(startIndex, start);

            Debug.Log(value);
            text.text = value;
        }
    }

    [System.Serializable]
    public struct Link
    {
        public string name;
        public UnityEvent onClick;
    }
}
