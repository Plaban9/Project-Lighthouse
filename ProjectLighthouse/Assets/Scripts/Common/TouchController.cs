using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TouchController : MonoBehaviour
{
    [SerializeField] SellDefender sellDefender;
    [SerializeField] LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                var sp = hit.transform.GetComponentInParent<DefenderSpawnPoint>();
                if (sp != null && sp.HasDefender())
                {
                    sellDefender.Show(sp.transform, sp.defenderData).Subscribe(_ =>
                    {
                        sp.SellDefender();
                    }).AddTo(this);
                }
            }
            else
            {
                sellDefender.Hide();
            }
        }
    }
}
