using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HealthBar : MonoBehaviour
{
    public float maxHealth = 10;
    [SerializeField]
    float curHealth = 10;
    public float healthBarLength = 10;
    [SerializeField]
    float percentHealth = 1f;
    GameObject healthBar;
    private void Start()
    {
        healthBar = transform.Find("Healthbar").gameObject;
        curHealth = maxHealth;
        healthBar.transform.localScale = Vector3.zero;
    }
    void FixedUpdate()
    {
        healthBar.transform.forward = -Camera.main.transform.forward;
    }
    public void AddjustCurrentHealth(float current, float maximum)
    {
        curHealth = current;
        percentHealth = Mathf.InverseLerp(0, maxHealth, current);
        healthBar.transform.localScale = new Vector3(healthBarLength * percentHealth, 1f, 1f);
        healthBar.transform.position = transform.position + Vector3.up;
        healthBar.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, percentHealth);
    }
}