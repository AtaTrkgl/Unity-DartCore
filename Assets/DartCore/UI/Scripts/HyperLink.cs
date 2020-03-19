using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class HyperLink : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public Link[] links;

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
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

    [System.Serializable]
    public struct Link
    {
        public string name;
        public UnityEvent onClick;
    }
}
