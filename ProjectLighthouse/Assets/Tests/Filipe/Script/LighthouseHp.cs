using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighthouseHp : MonoBehaviour
{

    //move this out later
    [Header("Lighthouse Health")]
    [SerializeField] public Material healthBarMaterial;
    [SerializeField] public GameObject healthBar;
    [SerializeField] public float lightHouseMaxHp;
    private float _lightHouseCurHp;
    private float _localScale;
    private Coroutine coroutine;

    private void Awake()
    {
        _localScale = healthBar.transform.localScale.x;
        _lightHouseCurHp = lightHouseMaxHp;
        healthBarMaterial.SetFloat("_Percentage", _lightHouseCurHp / lightHouseMaxHp);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Lighthouse takes damage");
        _lightHouseCurHp -= damage;
        if(coroutine!=null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(Pulse(0.25f + 1f - 1*(_lightHouseCurHp / lightHouseMaxHp)));
        healthBarMaterial.SetFloat("_Percentage", _lightHouseCurHp / lightHouseMaxHp);
        if(_lightHouseCurHp < 0f)
        {
            _lightHouseCurHp = 0f;
            GameManager.Instance.SetGameOver(true);
        }
    }

    private IEnumerator Pulse(float intensity)
    {
        float multiplier = intensity;
        while(multiplier > 0)
        {
            healthBarMaterial.SetFloat("_Alpha", Mathf.Clamp(0.45f + 0.55f*multiplier,0f,1f));
            healthBar.transform.localScale = 
                new Vector3(
                    _localScale + 30 * multiplier,
                    _localScale + 30 * multiplier,
                    _localScale + 30 * multiplier);
            multiplier -= 2*Time.deltaTime;
            yield return null;
        }

    }
    }
