using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GEX.utils;
using DG.Tweening;


public class SeagullManager : MonoBehaviour
{

    public GameObject seagul;
    public GameObject spawn;
    private XTimer timer;
    private List<Seagull> seagulls;
    private const int TOTAL_SEAGULLS = 5;

    public void Start()
    {
        seagulls = new List<Seagull>();

        timer = new XTimer();
        timer.setDelay(10f);
        timer.start();

        CreateSeagull();
    }

    public void Update()
    {
        if (timer.update())
        {
            timer.setDelay(UnityEngine.Random.Range(3f, 10f));
            timer.start();

            //
            // Create New Seagulls
            //
            if (seagulls.Count < TOTAL_SEAGULLS)
                CreateSeagull();

            //
            // Check if dead
            //
            for (int a = 0; a < seagulls.Count; a++)
            {
                if (seagulls[a].CanRemove())
                {
                    seagulls[a].Destroy();
                    seagulls.Remove(seagulls[a]);
                    a = 0;
                }
            }
        }
    }

    private void CreateSeagull()
    {
        Vector3 pos = spawn.transform.position;
        pos.y += UnityEngine.Random.Range(0f, 20f);
        GameObject obj = GameObject.Instantiate(seagul, pos, Quaternion.identity);
        obj.transform.localScale = new Vector3(0, 0, 0);
        obj.transform.DOScale(1f, 5f);

        float rotY = UnityEngine.Random.Range(90f, 140f);

        obj.transform.Rotate(0, rotY, 0);

        obj.SetActive(true);
        var sgull = obj.GetComponent<Seagull>();
        seagulls.Add(sgull);
    }

}
