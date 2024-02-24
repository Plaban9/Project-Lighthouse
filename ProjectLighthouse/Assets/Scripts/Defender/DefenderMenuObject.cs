using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class DefenderMenuObject : DraggableWorldObject
{
    [SerializeField] Image defenderIcon;
    [SerializeField] LayerMask layerMask;

    protected override void Start()
    {
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

        if (Physics.Raycast(ray, out hit, 1000f, layerMask))
        {
            var sp = hit.transform.GetComponentInParent<DefenderSpawnPoint>();
            if (sp != null)
            {
                sp.SpawnDefender(prefabToInstantiate);
            }
        }
    }
}
