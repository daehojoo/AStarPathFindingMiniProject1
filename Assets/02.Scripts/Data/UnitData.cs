using UnityEngine;
public enum UnitOrBuilding
{
    Unit,
    Building
}
public enum ControllableType
{
    Player,
    Enemy
}
public enum UnitType
{
    Attack,
    Support
}

[CreateAssetMenu(fileName = "New Unit", menuName = "Units/Unit Data")]

public class UnitData : ScriptableObject
{
    public Sprite Sprite;//초상화 이미지
    public GameObject attackMotion;//공격이펙트

    public string unitName;//유닛이름
    public int health;//체력
    public int attackPower;//공격력
    public int shild;//방어력
    public float attackSpeed;//공속
    public float moveSpeed;//이속
    public int population;//인구수
    public int gold;//돈얼마드는지
    public float makeTime;//생산시간
    public UnitOrBuilding unitOrBuilding;//건물인지유닛인지
    public ControllableType controllableType;//플레이어유닛인지적유닛인지
    public UnitType unitType;//유닛종류 공격형,서포터형 구분
    
   
}
