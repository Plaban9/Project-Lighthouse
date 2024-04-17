using UnityEngine;
using static TimeController;

public class LightSimulator : MonoBehaviour
{
    [SerializeField] private float _rotationDegressPerSecond = 1f;

    [SerializeField] public float rotateSpeedMovement = 0f;
    float rotateVelocity = 0f;
    [SerializeField] public bool spinRegardless = false;

    DayNightCycle currentState = DayNightCycle.NIGHT;
    Quaternion rotationToLookAt;

    private void Start()
    {
        InvokeRepeating("MoveToMouse",0f,0.01f);
    }

    private void FixedUpdate()
    {
        if(currentState == DayNightCycle.DAY || spinRegardless)
        {
            RotateAroundY();
        }
        else
        {
            float rotationY =
        Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            rotationToLookAt.eulerAngles.y,
            ref rotateVelocity,
            rotateSpeedMovement * (Time.fixedDeltaTime * 5f));

            transform.eulerAngles = new Vector3(0, rotationY, 0);

        }
    }

    private void RotateAroundY()
    {
        transform.Rotate(Vector3.up * Time.fixedDeltaTime * _rotationDegressPerSecond);
    }


    private void MoveToMouse()
    {
        if(currentState == DayNightCycle.NIGHT)
        {
            RaycastHit hit;

            if (
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 
                out hit, Mathf.Infinity))
            {
                rotationToLookAt = 
                    Quaternion.LookRotation(hit.point - transform.position);
            }
        }
        
    }

    private void OnEnable()
    {
        TimeController.dayNightCycleStartNotifier += DayNightEventHandler;
    }

    private void OnDisable()
    {
        TimeController.dayNightCycleStartNotifier -= DayNightEventHandler;
    }

    private void DayNightEventHandler(DayNightCycle dayNightCycle)
    {
        switch (dayNightCycle)
        {
            case DayNightCycle.DAY:
                currentState = DayNightCycle.DAY;
                break;
            case DayNightCycle.NIGHT:
                currentState = DayNightCycle.NIGHT;
                break;
        }
    }
}
