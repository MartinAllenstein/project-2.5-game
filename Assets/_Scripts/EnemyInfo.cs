using UnityEngine;

[CreateAssetMenu(fileName = "New Info",menuName = "New Info/New Enemy Info")]
public class EnemyInfo : ScriptableObject
{
    public string EnemyName;
    public int BaseHealth;
    public int BaseStr;
    public int BaseInitiative;
    public GameObject Prefad;
}
