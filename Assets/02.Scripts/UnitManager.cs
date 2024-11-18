//using System.Collections.Generic;
//using UnityEngine;

//public class UnitManager : MonoBehaviour
//{
//    public List<UnitData> UnitList;
//    public Sprite scvSprite;
//    public Sprite warriorSprite;
//    public Sprite wizardSprite;
//    public Sprite archorSprite;
//    public Sprite knightSprite;
//    public Sprite 유비sprite;
//    public Sprite 관우sprite;
//    public Sprite 장비sprite;
//    public Sprite 조조sprite;






//    public void Start()
//    {
//        UnitList = new List<UnitData>();

//        // 유닛 데이터 추가          이름 체력  공    방  이속  공속 초상화
//        UnitList.Add(new UnitData("일꾼", 40,    4,   0,   3,   2, scvSprite));
//        UnitList.Add(new UnitData("전사", 80,    6,   1,   4,   4, warriorSprite));
//        UnitList.Add(new UnitData("책사", 40,    20,  0,   3,   1, wizardSprite));
//        UnitList.Add(new UnitData("궁수", 60,    10,  1,   3,   2, archorSprite));
//        UnitList.Add(new UnitData("기마병", 100, 10,  2,   7,   2, knightSprite));
//        UnitList.Add(new UnitData("유비", 400,    25,  2,   5,   5, 유비sprite));
//        UnitList.Add(new UnitData("관우", 450,    40,  2,   4,   3, 관우sprite));
//        UnitList.Add(new UnitData("장비", 600,    20,  3,   4,   2, 장비sprite));
//        UnitList.Add(new UnitData("조조", 1000,   40,  4,   5,   5, 조조sprite));





//    }

//    public UnitData GetUnitData(string name)
//    {
//        return UnitList.Find(유닛 => 유닛.name == name);
//    }
//}
