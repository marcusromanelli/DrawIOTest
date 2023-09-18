using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkinView : View<SelectSkinView>
{
    [SerializeField] Text m_LevelCrownText;
    [SerializeField] Image[] m_coloredImages;
    [SerializeField] Transform m_prefabContainer;
    [SerializeField] SkinUIElement m_skinUIPrefab;
    [SerializeField] MainMenuView m_MainMenuView;
    [SerializeField] SkinUIElement m_SelectedBrush;

    Color _titleColor;
    List<SkinUIElement> skinUIElements;
    StatsManager m_StatsManager;
    int m_IdSkin = 0;


    private void Awake()
    {
        base.Awake();

        m_StatsManager = StatsManager.Instance;
        m_IdSkin = m_StatsManager.FavoriteSkin;
    }

    private void Start()
    {
        TryInitialize();
    }
    public void SetTitleColor(Color _titleColor)
    {
        this._titleColor = _titleColor;

        UpdateColors();
    }
    public void OnReturnButton()
    {
        this.Transition(false);
        m_MainMenuView.Transition(true);
    }

    void TryInitialize()
    {
        if (skinUIElements != null)
            return;

        skinUIElements = new List<SkinUIElement>();

        var existingSkins = GameManager.Instance.m_Skins;

        foreach (var skinData in existingSkins)
        {
            var prefab = Instantiate(m_skinUIPrefab, m_prefabContainer);
            prefab.Setup(skinData, OnClickSkin);

            skinUIElements.Add(prefab);
        }

        var currentSelectedSkin = GameManager.Instance.GetPlayerSkin();
        m_SelectedBrush.Setup(currentSelectedSkin);
    }

    void UpdateColors()
    {
        TryInitialize();

        foreach (var image in m_coloredImages)
        {
            image.color = _titleColor;

            foreach (var skin in skinUIElements)
                skin.UpdateColor(_titleColor);
        }
    }

    void OnClickSkin(SkinData selectedSkin)
    {
        GameManager.Instance.SetSkin(selectedSkin);

        m_SelectedBrush.Setup(selectedSkin);
    }
}
