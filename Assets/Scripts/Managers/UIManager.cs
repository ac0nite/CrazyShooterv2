using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PolygonCrazyShooter;
using UnityEngine;

public class UIManager : SingletoneGameObject<UIManager>
{
    private List<UI_Panel> _panels = null;

    protected override void Awake()
    {
        base.Awake();
        _panels = GetComponentsInChildren<UI_Panel>(true).ToList();
    }

    private void Start()
    {
        ShowPanel(UI_PanelType.GamePlay);
    }

    public void ShowPanel(UI_PanelType type)
    {
        foreach (var panel in _panels)
        {
            panel.gameObject.SetActive(panel.Type == type);
        }
    }
}
