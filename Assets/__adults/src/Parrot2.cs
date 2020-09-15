using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Parrot2 : MonoBehaviour
{
    public static Parrot2 me
    {
        get
        {
            if (_me == null)
                _me = GameObject.FindObjectOfType<Parrot2>();

            return _me;
        }
    }

    public Animator PARROT_MOVE;
    public Animator PARROT_ANIM;
    public ParrotCollider PARROT_COLLIDER;

    private static Parrot2 _me = null;

    public bool CanTalk()
    {
        AnimatorStateInfo info = PARROT_MOVE.GetCurrentAnimatorStateInfo(0);
        return (info.IsName("OnBoard") || info.IsName("OnWood"));
    }

    public bool IsOnWood()
    {
        return PARROT_MOVE.GetCurrentAnimatorStateInfo(0).IsName("OnWood");
    }

    public bool IsOnBoard()
    {
        return PARROT_MOVE.GetCurrentAnimatorStateInfo(0).IsName("OnBoard");
    }

    public bool IsOnIsland()
    {
        return PARROT_MOVE.GetCurrentAnimatorStateInfo(0).IsName("OnIsland");
    }

    public bool IsFlying1()
    {
        return PARROT_MOVE.GetCurrentAnimatorStateInfo(0).IsName("Flying1");
    }

    public bool IsOnSpecialMode()
    {
        return PARROT_MOVE.GetCurrentAnimatorStateInfo(0).IsName("OnSpecialMode");
    }

    public void INTRO()
    {
        PARROT_MOVE.SetTrigger("Intro");
    }

    public void MOVE_TO_WOOD()
    {
        PARROT_MOVE.SetTrigger("ToWood");
    }

    public void MOVE_TO_BOARD()
    {
        PARROT_MOVE.SetTrigger("ToBoard");
    }

    public void FLY()
    {
        PARROT_MOVE.SetTrigger("Fly");
    }

    public void SPECIAL_MODE()
    {
        PARROT_MOVE.SetTrigger("SpecialMode");
    }

    public void BACK()
    {
        PARROT_MOVE.SetTrigger("Back");
    }

    public void SetAnimFly()
    {
        PARROT_ANIM.SetBool("fly", true);
        PARROT_ANIM.SetBool("flyInPlace", false);
    }

    public void SetAnimFlyInPlace()
    {
        PARROT_ANIM.SetBool("fly", false);
        PARROT_ANIM.SetBool("flyInPlace", true);
    }

    public void SetAnimIdle()
    {
        PARROT_ANIM.SetBool("fly", false);
        PARROT_ANIM.SetBool("flyInPlace", false);
    }

    public void TakeOff()
    {
        PARROT_ANIM.SetTrigger("takeOff");
    }

    public void Talk01()
    {
        if (Config.PARROT_MOVE_MOUTH_WHEN_TALK)
            PARROT_ANIM.SetTrigger("talk_01");
    }

    public void Hit()
    {
        PARROT_ANIM.SetTrigger("hit");    
    }

    public void RESET_ANIMATOR()
    {
        /*PARROT_MOVE.ResetTrigger("ToWood");
        PARROT_MOVE.ResetTrigger("ToBoard");
        PARROT_MOVE.ResetTrigger("Fly");
        PARROT_MOVE.ResetTrigger("Back");
        PARROT_MOVE.ResetTrigger("Intro");
        PARROT_MOVE.ResetTrigger("SpecialMode");*/

        for (int a = 0; a < PARROT_MOVE.parameters.Length; a++)
        { 
            if(PARROT_MOVE.parameters[a].type == AnimatorControllerParameterType.Trigger)
                PARROT_MOVE.ResetTrigger(PARROT_MOVE.parameters[a].name);
        }
    }

    private void Update()
    {
        if (PARROT_COLLIDER.WasHitted())
        {
            Hit();
        }
    }

}
