using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] GameObject prefabToInstantiate;

    [SerializeField] RectTransform dragElement;

    [SerializeField] RectTransform canvas;


    // Original position
    Vector2 oriLocalPointerPos;
    Vector3 oriPanelLocalPos;
    Vector2 oriPos;

    private void Start()
    {
        oriPos = dragElement.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
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

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("eventData pos: "+ eventData.position);
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

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            if(hit.collider.CompareTag("Player"))
            {
                Debug.Log("Pointing FLoor!");
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Destroy(dragElement);
        dragElement.gameObject.SetActive(false);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 1000f))
        {
            Vector3 worldPoint = hit.point;
            
            Instantiate(prefabToInstantiate, worldPoint, Quaternion.identity);
        }
    }
}