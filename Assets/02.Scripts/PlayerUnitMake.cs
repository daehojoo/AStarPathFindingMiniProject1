using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitMake : MonoBehaviour
{
    public GameObject pirate;
    public GameObject archor;   
    public MoneyManager moneyManager;
    public Transform spawnPos;
    // Start is called before the first frame update
    void Start()
    {
        moneyManager = GetComponent<MoneyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PirateBtn()
    {
        if (moneyManager.money > 50f)
        {
            Instantiate(pirate, spawnPos.position, Quaternion.identity);
        }
    }
    public void ArchorBtn()
    {
        if (moneyManager.money > 100f)
        {
            Instantiate(pirate, spawnPos.position, Quaternion.identity);
        }
    }
}
