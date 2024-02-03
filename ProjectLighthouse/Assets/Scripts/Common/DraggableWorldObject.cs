using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWorldObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] protected GameObject dragElement;
    [SerializeField] protected RectTransform canvas;
    [SerializeField] protected GameObject prefabToInstantiate;

    protected virtual void Start()
    {
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if(dragElement == null)
        {
            dragElement = Instantiate(prefabToInstantiate);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit RayHit;
        bool isRayCast = Physics.Raycast(ray, out RayHit);

        if (isRayCast)

        {
            var ObjectHit = RayHit.transform.gameObject;
            var Hitpoint = RayHit.point;

            if (ObjectHit)
            {
                dragElement.transform.position = Hitpoint;
            }

        }

    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit RayHit;
        bool isRayCast = Physics.Raycast(ray, out RayHit);

        if (isRayCast)

        {
            var ObjectHit = RayHit.transform.gameObject;
            var Hitpoint = RayHit.point;

            if (ObjectHit)
            {
                dragElement.transform.position = Hitpoint;
            }

        }
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        Destroy(dragElement);
    }
}