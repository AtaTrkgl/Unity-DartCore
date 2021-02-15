using UnityEngine;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace DartCore.UI
{
    [HelpURL("https://github.com/AtaTrkgl/Unity-DartCore/wiki/DartCore.UI#5-tooltip")]
    public class DraggableWindowContainer : MonoBehaviour
    {
        #region Unity Editor

#if UNITY_EDITOR
        [MenuItem("DartCore/UI/Draggable Window Container"),
         MenuItem("GameObject/UI/DartCore/Draggable Window Container")]
        public static void CreateDraggableWindowContainer()
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("DraggableWindowContainer"),
                Selection.activeGameObject.transform, false);
            obj.name = "New Draggable Window Container";
        }
#endif

        #endregion

        [Header("Configuration")] [SerializeField]
        private float padding;

        public float followTime = .1f;

        private RectTransform containerTrans;
        private RectTransform draggableWindowTrans;
        private RectTransform canvas;
        private DraggableWindow draggableWindow;
        private Vector2 cursorOffset;
        private bool isDragging = false;
        private Vector4 boundaries;

        private void Awake()
        {
            containerTrans = GetComponent<RectTransform>();
            draggableWindowTrans = transform.Find("DraggableWindow").GetComponent<RectTransform>();
            draggableWindow = transform.Find("DraggableWindow").GetComponent<DraggableWindow>();
            canvas = GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>();
            UpdatePadding();
        }

        private void Update()
        {
            if (draggableWindow.isCursorOn && Input.GetMouseButtonDown(0))
            {
                cursorOffset = (Vector2) Input.mousePosition / canvas.lossyScale -
                               (Vector2) draggableWindowTrans.localPosition;
                isDragging = true;
            }

            if (isDragging)
            {
                if (Input.GetMouseButton(0))
                    FollowCursor();
                if (Input.GetMouseButtonUp(0))
                    isDragging = false;
            }
        }

        public void SetPadding(float val)
        {
            padding = val;
            UpdatePadding();
        }

        private void FollowCursor()
        {
            var desiredPos = (Vector2) Input.mousePosition / canvas.lossyScale - cursorOffset;
            draggableWindowTrans.DOLocalMove(new Vector2(
                    Mathf.Clamp(desiredPos.x, boundaries.x, boundaries.y),
                    Mathf.Clamp(desiredPos.y, boundaries.z, boundaries.w)),
                followTime).SetUpdate(true);
        }

        private void UpdatePadding()
        {
            var sizeDelta = containerTrans.sizeDelta;
            var delta = draggableWindowTrans.sizeDelta;

            boundaries = new Vector4(
                -sizeDelta.x / 2 + delta.x / 2 + padding,
                sizeDelta.x / 2 - delta.x / 2 - padding,
                -sizeDelta.y / 2 + delta.y / 2 + padding,
                sizeDelta.y / 2 - delta.y / 2 - padding
            );
        }
    }
}