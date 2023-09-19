using UnityEngine;


[CreateAssetMenu(fileName = "Cheat", menuName = "Data/Cheats", order = 1)]
public class CheatData : ScriptableObject
{
    public string m_Name;
    public string m_Description;
    public string m_Key;

    public CheatData(string m_Key)
    {
        this.m_Key = m_Key;
        this.m_Description = "";
        this.m_Name = "";
    }
    public bool IsValid() => m_Key != null;
}
