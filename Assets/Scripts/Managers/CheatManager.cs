using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.CompilerServices;
using static CheatManager;
using Newtonsoft.Json.Linq;

[DefaultExecutionOrder(-1)]
public class CheatManager : PersistentSingleton<CheatManager>
{
    [Serializable]
    public struct StartCheatData
    {
        public CheatData m_Cheat;
        public bool m_startValue;
    }
    [Serializable]
    public struct CheatData
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

    public delegate void HandleOnCheatTrigger(bool newValue);

    public static bool IsEnabled{
        get
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            return Instance.Enabled;
#else
            return false;
#endif
        }
    }

    [SerializeField] bool Enabled = true;
    [SerializeField] List<StartCheatData> m_presetCheats = new List<StartCheatData>();

    Dictionary<CheatData, bool> m_cheatList = new Dictionary<CheatData, bool>();
    Dictionary<string, List<Action<bool>>> m_cheatTriggers = new Dictionary<string, List<Action<bool>>>();

    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        foreach(var cheat in m_presetCheats)
        {
            RegisterCheat(cheat.m_Cheat, cheat.m_startValue);
        }
    }
    public static void RegisterCheatTrigger(string _Key, Action<bool> _OnTrigger)
    {
        Instance._registerCheatTrigger(_Key, _OnTrigger);
    }
    public void _registerCheatTrigger(string _Key, Action<bool> _OnTrigger)
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR

        if (!m_cheatTriggers.ContainsKey(_Key))
        {
            m_cheatTriggers[_Key] = new List<Action<bool>>();
        }

        m_cheatTriggers[_Key].Add(_OnTrigger);
#endif
    }

    private static void RegisterCheat(CheatData _CheatData, bool _DefaultStat)
    {
        Instance._registerCheat(_CheatData, _DefaultStat);
    }
    private static void RegisterCheat(string _Key, bool _DefaultStat)
    {
        Instance._registerCheat(_Key, _DefaultStat);
    }

    private void _registerCheat(string _Key, bool _DefaultStat)
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        var cheatData = new CheatData(_Key);

        _registerCheat(cheatData, _DefaultStat);
#endif
    }

    private void _registerCheat(CheatData _CheatData, bool _DefaultStat)
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        if (m_cheatList.ContainsKey(_CheatData))
        {
            Debug.LogWarning(string.Format("Cheat already registered. Switching value to {0}", _DefaultStat));
            ToggleCheat(_CheatData.m_Key, _DefaultStat);
            return;
        }

        m_cheatList[_CheatData] = _DefaultStat;
#endif
    }

    public static void ToggleCheat(string _Key, bool _Value)
    {
        Instance._toggleCheat(_Key, _Value);
    }
    private void _toggleCheat(string _Key, bool _Value)
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        var cheatData = _findCheatData(_Key);

        if (!cheatData.IsValid())
        {
            RegisterCheat(_Key, _Value);
            return;
        }

        m_cheatList[cheatData] = _Value;

        _triggerCheat(_Key, _Value);
#endif
    }

    private void _triggerCheat(string _Key, bool _Value)
    {
        if (!m_cheatTriggers.ContainsKey(_Key)){
            return;
        }

        foreach (var trigger in m_cheatTriggers[_Key])
            trigger(_Value);
    }
    public static Dictionary<CheatData, bool> GetAllCheats()
    {
        return Instance._getAllCheats();
    }
    private Dictionary<CheatData, bool> _getAllCheats()
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        return m_cheatList;
#else
        return null;
#endif
    }

    public static bool GetCheatStatus(string _Key)
    {
        return Instance._getCheatStatus(_Key);
    }

    private bool _getCheatStatus(string _Key)
    {
        var cheatData = _findCheatData(_Key);

        if (!cheatData.IsValid())
            return false;

        return m_cheatList[cheatData];
    }
    private CheatData _findCheatData(string cheatData)
    {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        foreach (var data in m_cheatList.Keys)
            if (cheatData == data.m_Key)
                return data;

         return new CheatData();
#else
        return null;
#endif
    }
}
