using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PausePanel : UI_Panel
{
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void OnResumeButtonClick()
    {
        UIManager.Instance.ShowPanel(UI_PanelType.GamePlay);
    }
}
