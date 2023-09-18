using System.Collections.Generic;
using UnityEngine;

public class CheatsView : View<CheatsView> {

    [SerializeField] CheatsView m_cheatsView;
    [SerializeField] MainMenuView m_mainView;
    [SerializeField] Transform m_cheatsElementContainer;
    [SerializeField] CheatUIElement m_cheatUIElementPrefab;

    private List<CheatUIElement> m_cheatElements;


    public void OnClickReturn()
    {
        m_mainView.Transition(m_cheatsView.IsVisible);
        m_cheatsView.Transition(!m_cheatsView.IsVisible);
    }

    protected override void OnPreIn()
    {
        TryInitialize();
    }

    private void TryInitialize()
    {
        if(m_cheatElements == null)
        {
            m_cheatElements = new List<CheatUIElement>();

            var allCheats = CheatManager.GetAllCheats();

            foreach(var cheat in allCheats)
            {
                var element = Instantiate(m_cheatUIElementPrefab, m_cheatsElementContainer);
                element.Setup(cheat.Key);
                element.Set(cheat.Value);

                element.OnClickCheat += ClickedCheat;
            }
        }
    }

    private void ClickedCheat(string _Key, bool _Value)
    {
        CheatManager.ToggleCheat(_Key, _Value);
    }
}
