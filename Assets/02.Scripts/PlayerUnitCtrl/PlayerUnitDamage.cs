using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitDamage : MonoBehaviour
{
    public float health = 200; //ü��
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
        Color startColor = spriteRenderer.color; // ���� ����
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0); // ��ǥ ���� (����)

        float elapsedTime = 0f; // ��� �ð�
        float duration = 1f; // ���� �ð�

        while (elapsedTime < duration)
        {
            // ��� �ð��� ����Ͽ� ���� ����
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� ���� ���� (������ ����)
        spriteRenderer.color = endColor;
        Destroy(gameObject); // ������Ʈ �ı�
    }
}
