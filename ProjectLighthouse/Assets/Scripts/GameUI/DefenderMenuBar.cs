using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DefenderMenuBar : MonoBehaviour
{
    [SerializeField] GameObject menuBarGO;
    [SerializeField] GameObject expandButton;
    [SerializeField] Transform turrentsMenuUI;

    private void OnEnable()
    {
        TimeController.dayNightCycleStartNotifier += DayCycle;
    }

    private void OnDisable()
    {
        TimeController.dayNightCycleStartNotifier -= DayCycle;
    }

    private void DayCycle(DayNightCycle cycle)
    {
        switch (cycle)
        {
            case DayNightCycle.DAY:
                menuBarGO.transform.DOScale(Vector3.one, .25f).SetEase(Ease.InQuint);
                break;
            case DayNightCycle.NIGHT:
                menuBarGO.transform.DOScale(Vector3.zero, .25f).SetEase(Ease.InQuint).OnComplete(() => OnMenuToggled(false));
                break;
        }
    }

    public void OnMenuToggled(bool isExpanded)
    {
        var targetRotation = isExpanded ? 0f : -180f;
        expandButton.transform.DORotate(Vector3.forward * targetRotation, .25f);
        turrentsMenuUI.DOScaleX(isExpanded ? 1f : 0f, .25f).SetEase(Ease.InQuint);
    }
}
