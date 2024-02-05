using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DefenderMenuBar : MonoBehaviour
{
    [SerializeField] GameObject expandButton;

    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickExpandButton()
    {
        expandButton.gameObject.SetActive(false);
        rectTransform.DOMoveY(0, 0.25f).SetEase(Ease.InQuart);
    }

    public void OnClickCollapseButton()
    {
        rectTransform.DOMoveY(-230, 0.25f).SetEase(Ease.InQuart).onComplete += () => { expandButton.gameObject.SetActive(true); };
    }
}
