using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatUIElement : MonoBehaviour
{
    public delegate void HandleOnClickCheat(string _Key, bool _Value);
    public event HandleOnClickCheat OnClickCheat;

    [SerializeField] TMP_Text m_cheatName;
    [SerializeField] TMP_Text m_cheatDesc;
    [SerializeField] Toggle m_toggle;


    CheatData m_cheatData;
    string m_cheatKey;

    public void Setup(CheatData _CheatData)
    {
        this.m_cheatData = _CheatData;

        m_cheatName.text = _CheatData.m_Name;
        m_cheatDesc.text = _CheatData.m_Description;
        m_cheatKey = _CheatData.m_Key;

        m_toggle.onValueChanged.RemoveAllListeners();
        m_toggle.onValueChanged.AddListener(OnToggleClicked);
    }

    public void Set(bool value)
    {
        this.m_toggle.SetIsOnWithoutNotify(value);
    }
    
    private void OnToggleClicked(bool newValue)
    {
        OnClickCheat?.Invoke(m_cheatKey, newValue);
    }
}
