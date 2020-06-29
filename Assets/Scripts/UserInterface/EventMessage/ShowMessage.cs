using System;
using System.Collections;
using System.Collections.Generic;
using PolygonCrazyShooter;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class ShowMessage : MonoBehaviour
{
    [SerializeField] private RectTransform _listContentParent = null;
    [SerializeField] private MessageElementItem messageElementPrefab = null;
    [SerializeField] private Scrollbar _scrol = null;
    private Dictionary<int, MessageElementItem> _dictionaryMessage = new Dictionary<int, MessageElementItem>();
    [SerializeField] private float _MessageDisplayTime = 3f;

    private void Update()
    {
        _scrol.value = 0f;
    }

    public void Send(String _general, String _details, int _id , bool autoRemove = false)
    {
        if(_dictionaryMessage.ContainsKey(_id))
            return;
        
        var message = Instantiate(messageElementPrefab);
        message.SetData(_general, _details);
        
        _dictionaryMessage.Add(_id, message); 
        message.transform.SetParent(_listContentParent);

        if (autoRemove)
        {
            StartCoroutine(RemoveMessageCoroutine(_id));
        }
    }
    
    public void Remove(int _id)
    {
        Destroy(_dictionaryMessage[_id].gameObject);
        _dictionaryMessage.Remove(_id);
    }

    IEnumerator RemoveMessageCoroutine(int id)
    {
        yield return new WaitForSeconds(_MessageDisplayTime);
        Remove(id);
    }
}
