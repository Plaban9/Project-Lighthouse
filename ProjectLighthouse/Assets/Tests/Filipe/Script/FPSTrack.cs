using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSTrack : MonoBehaviour
{
    public float fps;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GetFPS", 0.5f, 0.5f);
    }

    public void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        text.text = fps + " fps";
    }
}
