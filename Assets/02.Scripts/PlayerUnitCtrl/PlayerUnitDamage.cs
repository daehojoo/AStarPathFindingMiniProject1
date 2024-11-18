using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitDamage : MonoBehaviour
{
    public float health = 200; //체력
    public float maxHealth = 200;
    public SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Canvas Canvas;
    public Image hpBar;
    public Text hpText;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Canvas = transform.GetChild(0).GetComponent<Canvas>();
        hpBar = Canvas.transform.GetChild(2).GetComponent<Image>();
        hpText = Canvas.transform.GetChild(3).GetComponent<Text>();
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        hpBar.fillAmount = health / maxHealth;
        if (hpBar.fillAmount < 0.5f)
            hpBar.color = Color.red;
        hpText.text = health.ToString();


        if (health <= 0)
        {
            StartCoroutine(AlphaChange());
            hpText.text = "0";
            hpBar.fillAmount = 0 / maxHealth;

        }
    }

    IEnumerator AlphaChange()
    {
        Color startColor = spriteRenderer.color; // 시작 색상
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0); // 목표 색상 (투명)

        float elapsedTime = 0f; // 경과 시간
        float duration = 1f; // 지속 시간

        while (elapsedTime < duration)
        {
            // 경과 시간에 비례하여 색상 보간
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 색상 설정 (완전히 투명)
        spriteRenderer.color = endColor;
        Destroy(gameObject); // 오브젝트 파괴
    }
}
