using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class MainCanvas : MonoBehaviour
{
    private static MainCanvas _me = null;
    
    public Text outp;

    public static MainCanvas me { get { if (_me == null) _me = GameObject.FindObjectOfType<MainCanvas>(); return _me; } }

    
    public void Start()
    { 
    
    }

    public void Update()
    { 
    
    }
}
