using UnityEngine;
using UnityEngine.UI;

public class SkinUIElement : MonoBehaviour
{
    public delegate void HandleOnClickSkin(SkinData skinData);
    public event HandleOnClickSkin OnClickSkin;

    [SerializeField] Camera m_Camera;
    [SerializeField] RawImage m_RawImage;
    [SerializeField] Image m_ButtonImage;
    [SerializeField] BrushMainMenu m_Brush;

    RenderTexture m_renderTexture;
    HandleOnClickSkin m_onClickSkin;
    SkinData m_skinData;

    void Awake()
    {
        m_renderTexture = new RenderTexture(256, 256, 16, UnityEngine.Experimental.Rendering.DefaultFormat.LDR);
        m_renderTexture.Create();

        m_Camera.targetTexture = m_renderTexture;

        m_RawImage.texture = m_renderTexture;
    }

    public void Setup(SkinData _SkinData, HandleOnClickSkin _OonClickSkin = null)
    {
        this.m_onClickSkin = _OonClickSkin;
        this.m_skinData = _SkinData;

        SetupSkin();
    }

    public void UpdateColor(Color _UpdateColor)
    {
        m_ButtonImage.color = _UpdateColor;
    }

    public void OnClick()
    {
        m_onClickSkin?.Invoke(m_skinData);
    }

    void SetupSkin()
    {
        m_Brush.Set(m_skinData);
    }
}
