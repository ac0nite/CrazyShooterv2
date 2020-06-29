using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageElementItem : MonoBehaviour
{
    [SerializeField] private Text _generalText = null;
    [SerializeField] private Text _detailsText = null;

    public void SetData(String _general, String _details = null)
    {
        _generalText.text = _general;
        _detailsText.text = _details;
    }
}
