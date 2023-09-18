using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : View<MainMenuView>
{
    private const string m_BestScorePrefix = "BEST SCORE ";
    private const string c_SkinSelectionCheatName = "ENABLE_SKIN_SELECTION_VIEW";

    public Text m_BestScoreText;
    public Image m_BestScoreBar;
    public Image m_selectSkinImage;
    public GameObject m_BestScoreObject;
    public InputField m_InputField;
    public List<Image> m_ColoredImages;
    public List<Text> m_ColoredTexts;

    public GameObject m_BrushGroundLight;
    public SkinUIElement m_BrushesPrefab;
    private int m_IdSkin;
    public GameObject m_PointsPerRank;
    public RankingView m_RankingView;
    public SelectSkinView m_SelectSkinView;
    public Transform m_SelectBrushButtonsContainer;
    public Transform m_SelectSkinViewButtonContainer;

    [Header("Ranks")]
    public string[] m_Ratings;

    private StatsManager m_StatsManager;
    private Color c_currentColor;

    protected override void Awake()
    {
        base.Awake();

        m_StatsManager = StatsManager.Instance;
        m_IdSkin = m_StatsManager.FavoriteSkin;

        CheatManager.RegisterCheatTrigger(c_SkinSelectionCheatName, OnSkinSelectionCheatToggled);

        var skinSelectionStatus = CheatManager.GetCheatStatus(c_SkinSelectionCheatName);
        ToggleSkinSelectionView(skinSelectionStatus);
    }
    private void OnSkinSelectionCheatToggled(bool _Value)
    {
        ToggleSkinSelectionView(_Value);
    }
    private void ToggleSkinSelectionView(bool isEnabled)
    {
        m_SelectBrushButtonsContainer.gameObject.SetActive(!isEnabled);
        m_SelectSkinViewButtonContainer.gameObject.SetActive(isEnabled);
    }

    public void OnPlayButton()
    {
        if (m_GameManager.currentPhase == GamePhase.MAIN_MENU)
            m_GameManager.ChangePhase(GamePhase.LOADING);
    }
    public void OnSelectSkinButton()
    {
        this.Transition(false);
        m_SelectSkinView.Transition(true);
    }

    protected override void OnGamePhaseChanged(GamePhase _GamePhase)
    {
        base.OnGamePhaseChanged(_GamePhase);

        switch (_GamePhase)
        {
            case GamePhase.MAIN_MENU:
                m_BrushGroundLight.SetActive(true);
                Transition(true);
                break;

            case GamePhase.LOADING:
                m_BrushGroundLight.SetActive(false);

                m_BrushesPrefab.gameObject.SetActive(false);

                if (m_Visible)
                    Transition(false);
                break;
        }
    }

    public void SetTitleColor(Color _Color)
    {
        c_currentColor = _Color;

        m_BrushesPrefab.gameObject.SetActive(true);
        int favoriteSkin = Mathf.Min(m_StatsManager.FavoriteSkin, m_GameManager.m_Skins.Count - 1);
        m_BrushesPrefab.Setup(m_GameManager.m_Skins[favoriteSkin]);


        string playerName = m_StatsManager.GetNickname();
        m_selectSkinImage.color = c_currentColor;

        if (playerName != null)
            m_InputField.text = playerName;

        for (int i = 0; i < m_ColoredImages.Count; ++i)
            m_ColoredImages[i].color = c_currentColor;

        for (int i = 0; i < m_ColoredTexts.Count; i++)
            m_ColoredTexts[i].color = c_currentColor;
            
        m_RankingView.gameObject.SetActive(true);
        m_RankingView.RefreshNormal();

        m_SelectSkinView.SetTitleColor(c_currentColor);
    }

    public void OnSetPlayerName(string _Name)
    {
        m_StatsManager.SetNickname(_Name);
    }

    public string GetRanking(int _Rank)
    {
        return m_Ratings[_Rank];
    }

    public int GetRankingCount()
    {
        return m_Ratings.Length;
    }

    public void LeftButtonBrush()
    {
        ChangeBrush(m_IdSkin - 1);
    }

    public void RightButtonBrush()
    {
       ChangeBrush(m_IdSkin + 1);
    }

    public void ChangeBrush(int _NewBrush)
    {
        _NewBrush = Mathf.Clamp(_NewBrush, 0, GameManager.Instance.m_Skins.Count);
        m_IdSkin = _NewBrush;
        if (m_IdSkin >= GameManager.Instance.m_Skins.Count)
            m_IdSkin = 0;
        GameManager.Instance.m_PlayerSkinID = m_IdSkin;
        int favoriteSkin = Mathf.Min(m_StatsManager.FavoriteSkin, m_GameManager.m_Skins.Count - 1);

        var Skin = GameManager.Instance.m_Skins[favoriteSkin];
        m_BrushesPrefab.Setup(Skin);

        m_StatsManager.FavoriteSkin = m_IdSkin;
        GameManager.Instance.SetColor(GameManager.Instance.ComputeCurrentPlayerColor(true, 0));
    }
}
