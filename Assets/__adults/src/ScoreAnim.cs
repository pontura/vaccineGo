using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

public class ScoreAnim : MonoBehaviour
{
    
    private void Start()
    {
        Vector3 originalScale = transform.localScale * 1.3f;

        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(originalScale, 1f).OnComplete(OnAnimComplete);
    }

    private void OnAnimComplete()
    {
        transform.DOMoveY(transform.position.y - 1f, 1f);
        transform.DOScale(0f, 1f).OnComplete(OnAnimComplete2);
    }

    private void OnAnimComplete2()
    {
        Destroy(this.gameObject);
    }
}
