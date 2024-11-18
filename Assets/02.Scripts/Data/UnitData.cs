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
    public Sprite Sprite;//�ʻ�ȭ �̹���
    public GameObject attackMotion;//��������Ʈ

    public string unitName;//�����̸�
    public int health;//ü��
    public int attackPower;//���ݷ�
    public int shild;//����
    public float attackSpeed;//����
    public float moveSpeed;//�̼�
    public int population;//�α���
    public int gold;//���󸶵����
    public float makeTime;//����ð�
    public UnitOrBuilding unitOrBuilding;//�ǹ�������������
    public ControllableType controllableType;//�÷��̾�������������������
    public UnitType unitType;//�������� ������,�������� ����
    
   
}
