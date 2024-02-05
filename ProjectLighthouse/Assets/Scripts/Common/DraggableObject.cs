using UnityEngine;
using UnityEngine.EventSystems;

// For Drag canvas object to world
public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] protected RectTransform dragElement;
    [SerializeField] protected RectTransform canvas;

    // Original position
    Vector2 oriLocalPointerPos;
    Vector3 oriPanelLocalPos;
    Vector2 oriPos;

    protected virtual void Start()
    {
        oriPos = dragElement.localPosition;
        
        if(dragElement == null)
            dragElement = GetComponent<RectTransform>();

        if(canvas == null)
            canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        dragElement.gameObject.SetActive(true);
        dragElement.localPosition = oriPos;

        oriPanelLocalPos = dragElement.localPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas,
            eventData.position,
            eventData.pressEventCamera,
            out oriLocalPointerPos);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 localPointerPos;

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas,
            eventData.position,
            eventData.pressEventCamera,
            out localPointerPos))
        {
            Vector3 offsetToOri = localPointerPos - oriLocalPointerPos;

            dragElement.localPosition = oriPanelLocalPos + offsetToOri;
        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        dragElement.gameObject.SetActive(false);
    }
}