using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public int money; // 현재 보유 금액
    public Text text;
    void Start()
    {
        money = 0; // 초기 금액 설정
        StartCoroutine(IncrementMoney()); // 돈 증가 코루틴 시작
    }

    void Update()
    {
        text.text = $"GOLD : {money.ToString()}";
        // 여기서 Update를 사용할 필요는 없지만,
        // 다른 기능을 추가하고 싶으면 사용할 수 있습니다.
    }

    private IEnumerator IncrementMoney()
    {
        while (true) // 무한 루프
        {
            money += 3; // 1초마다 돈 증가
            yield return new WaitForSeconds(1f); // 1초 대기
        }
    }

    // 다른 스크립트에서 호출할 수 있는 메서드
    public void AddMoney(int amount)
    {
        money += amount; // 지정된 금액 추가
        Debug.Log("Money Added: " + amount + ", Total Money: " + money);
    }
}
