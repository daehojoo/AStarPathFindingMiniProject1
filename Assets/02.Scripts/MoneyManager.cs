using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public int money; // ���� ���� �ݾ�
    public Text text;
    void Start()
    {
        money = 0; // �ʱ� �ݾ� ����
        StartCoroutine(IncrementMoney()); // �� ���� �ڷ�ƾ ����
    }

    void Update()
    {
        text.text = $"GOLD : {money.ToString()}";
        // ���⼭ Update�� ����� �ʿ�� ������,
        // �ٸ� ����� �߰��ϰ� ������ ����� �� �ֽ��ϴ�.
    }

    private IEnumerator IncrementMoney()
    {
        while (true) // ���� ����
        {
            money += 3; // 1�ʸ��� �� ����
            yield return new WaitForSeconds(1f); // 1�� ���
        }
    }

    // �ٸ� ��ũ��Ʈ���� ȣ���� �� �ִ� �޼���
    public void AddMoney(int amount)
    {
        money += amount; // ������ �ݾ� �߰�
        Debug.Log("Money Added: " + amount + ", Total Money: " + money);
    }
}
