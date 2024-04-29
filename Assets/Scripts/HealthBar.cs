using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HealthBar : MonoBehaviour
{
    [SerializeField] RectTransform healthbarFill;
    [SerializeField] RectTransform turnMeterFill;
    [SerializeField] GameObject[] shieldIcons;
    Canvas canvas;
    Camera cam;
    PlayerScript playerScript;
    SortingGroup sortingGroup;
    float healthbarInitialScale;
    float turnMeterInitialScale;

    private void Awake()
    {
        playerScript = GetComponentInParent<PlayerScript>();
        healthbarInitialScale = healthbarFill.localScale.x;
        turnMeterInitialScale = turnMeterFill.localScale.x;
    }

    private void Start()
    {
        sortingGroup = GetComponentInParent<SortingGroup>();   
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = cam;
    }

    private void Update()
    {
        canvas.sortingOrder = sortingGroup.sortingOrder;
    }

    public void UpdateHealthbar()
    {
        float healthRatio = (float)playerScript.health / (float)playerScript.maxHealth;
        healthbarFill.localScale = new Vector3(healthbarInitialScale * healthRatio, healthbarFill.localScale.y, healthbarFill.localScale.z);
    }

    public void UpdateTurnMeterBar()
    {
        float turnMeterRatio = (float)playerScript.turnMeter / 1000f;
        if(turnMeterRatio > 1)
        {
            turnMeterRatio = 1;
        }
        turnMeterFill.localScale = new Vector3(turnMeterInitialScale * turnMeterRatio, turnMeterFill.localScale.y, turnMeterFill.localScale.z);
    }

    public void UpdateArmorIcons()
    {
        for(int i = 0; i < shieldIcons.Length; i++)
        {
            if(i < playerScript.armor)
            {
                shieldIcons[i].SetActive(true);
            }
            else
            {
                shieldIcons[i].SetActive(false);
            }
        }
    }
}
