using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour 
{
    public Image m_VibrationButton;
    public Sprite m_VibrationOnSprite;
    public Sprite m_VibrationOffSprite;
    public Image m_CheatModeButton;
    public Animator m_BarAnim;
    public CheatsView m_cheatsView;
    public MainMenuView m_mainView;


    public const string VisibleAnimation = "Visible";

    // Cache
    private MobileHapticManager m_Haptic;

    // Buffer
    private bool m_PanelVisible;

    private bool Vibration
    {
        get
        {
            return (MobileHapticManager.s_Vibrate);
        }
        set
        {
            MobileHapticManager.s_Vibrate = value;
            PlayerPrefs.SetInt(Constants.c_VibrationSave, value ? 1 : 0); // Converting bool to int
        }
    }

    private void Awake()
    {
        m_Haptic = MobileHapticManager.Instance;
        Vibration = PlayerPrefs.GetInt(Constants.c_VibrationSave, 1) == 1; // Converting int to bool

        m_PanelVisible = false;

        m_BarAnim.SetBool(VisibleAnimation, m_PanelVisible);

        var isCheatModeEnabled = CheatManager.IsEnabled;
        m_BarAnim.SetBool("Cheats", isCheatModeEnabled);
        m_CheatModeButton.gameObject.SetActive(isCheatModeEnabled);
    }

    public void ClickVibrateButton()
    {
        Vibration = !Vibration;
        RefreshButtonsVisual();

        if (Vibration)
            m_Haptic.Vibrate(MobileHapticManager.E_FeedBackType.ImpactHeavy);
    }

    public void ClickSettingsButton()
    {
        m_PanelVisible = !m_PanelVisible;
        m_BarAnim.SetBool(VisibleAnimation, m_PanelVisible);
    }

    private void RefreshButtonsVisual()
    {
        m_VibrationButton.sprite = Vibration ? m_VibrationOnSprite : m_VibrationOffSprite;
    }

    public void ClickCheatModeButton()
    {
        m_mainView.Transition(m_cheatsView.IsVisible);
        m_cheatsView.Transition(!m_cheatsView.IsVisible);
    }
}
