using UnityEngine;

[CreateAssetMenu(fileName = "New Info",menuName = "New Info/New Character Info")]
public class CharacterInfo : ScriptableObject
{
    public string CharacterName;
    public int StaringLevel;
    public int BaseHealth;
    public int BaseStrength;
    public int BaseInitiative;
    public GameObject Prefad;
}
