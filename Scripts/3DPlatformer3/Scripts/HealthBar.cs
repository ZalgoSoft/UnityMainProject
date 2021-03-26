using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float maxHealth = 10;
    public float curHealth = 10;
    public float healthBarLength = 10;
    float percentHealth = 1f;
    GameObject healthBar;
    private void Start()
    {
        healthBar = transform.Find("Healthbar").gameObject;
        curHealth = maxHealth;
    }
    void FixedUpdate()
    {
        // Vector2 siz = new Vector2(
        //   healthBar.GetComponent<SpriteRenderer>().sprite.texture.width,
        // healthBar.GetComponent<SpriteRenderer>().sprite.texture.height);
        //GUI.Box(new Rect(100, 100, healthBarLength, 20), curHealth + "/" + maxHealth);
        healthBar.transform.forward = -Camera.main.transform.forward;
        //for (int y = 0; y < siz.y; y++)        {
        //  for (int x = 0; x < siz.x; x++)           {

        //}        }
    }
    public void AddjustCurrentHealth(float current, float maximum)
    {
        curHealth = current;
        //healthBar.transform.localScale = new Vector3(healthBarLength * percentHealth, 1f, 1f);
        healthBar.transform.localScale = new Vector3(healthBarLength * percentHealth, 1f, 1f);
        healthBar.transform.position = transform.position + Vector3.up;

        percentHealth = Mathf.InverseLerp(0, maxHealth, current);
        healthBar.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, percentHealth);
    }
}