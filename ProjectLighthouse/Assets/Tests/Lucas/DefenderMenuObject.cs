using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class DefenderMenuObject : DraggableObject
{
    [SerializeField] Image defenderIcon;
    [SerializeField] GameObject prefabToInstantiate;

    protected override void Start()
    {
        if(dragElement == null)
        {
            dragElement = Instantiate(defenderIcon.gameObject, transform).GetComponent<RectTransform>();
            dragElement.sizeDelta = new Vector2(80, 80);
            dragElement.gameObject.SetActive(false);
        }

        base.Start();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        defenderIcon.DOFade(0.4f, 0f);

        DefenderSpawnManager.Instance.SetIsDraggingDefenderFromMenu(true);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        defenderIcon.DOFade(1, 0f);

        DefenderSpawnManager.Instance.SetIsDraggingDefenderFromMenu(false);


        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000f) && hit.collider.CompareTag("SpawnPoint"))
        {
            Vector3 worldPoint = hit.point;
            Instantiate(prefabToInstantiate, worldPoint, Quaternion.identity);
        }
    }
}
