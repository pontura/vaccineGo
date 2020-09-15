using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GEX.utils;

namespace InGameBoard
{

    public class InGameBoardModule : MonoBehaviour
    {

        public static InGameBoardModule me
        {
            get
            {
                if (_me == null)
                    _me = GameObject.FindObjectOfType<InGameBoardModule>();

                return _me;
            }
        }

        enum MoveTo
        { 
            FLY,
            WOOD
        }

        public GameObject container;
        public InfoObj INFO_OBJ;
        public WinObj WIN_OBJ;
        public Material overlayMat;
        public GameObject dolphin;
        public GameObject panel;
        public Animator dolphinAnimator;
        public SoundPlayer SOUND;


        public String[] IntroMessages;
        public LevelData[] LEVEL_DATA;
        private LevelData CURR_LEVEL_DATA;

        private String STATE = "";
        private int extra_state = 0;
        private int counter = 0;
        private int dolphinTalkCount = 1;
        private bool finished = false;
        private int currLevel = 0;
        private MoveTo moveTo;
        private XTimer continueTimer;

        private XTimer timer;
        private bool goodJobMsg = false;
        private bool timesUp = false;

        private static InGameBoardModule _me = null;

        private void Start()
        {
            timer = new XTimer();
            continueTimer = new XTimer();
        }

        public void Talk(int lvl = -1) // -1 == intro
        {
            //dolphinAnimator.SetTrigger("talk");
            //SOUND.Play("fx_dolphin_0" + dolphinTalkCount);
            Parrot2.me.Talk01();

            if (lvl == -1)
            {
                SOUND.PlayLang("mission_intro");
                /*if(GAME.ageMode == AgeMode.KIDS)
                    SOUND.PlayLang("mission_intro");
                else
                    SOUND.PlayLang("mission_intro_adults");*/
                
            }
            else if (lvl == -2)
            {
                SOUND.PlayLang("instructions");
            }
            else if (lvl == -3)
            {
                SOUND.PlayLang("glow_breath");
            }
            else
                SOUND.PlayLang("mission_game_" + lvl);
        }

        public void TalkSpecial()
        {
            SOUND.PlayLang("mission_special_mode");
        }

        public void ByeBye()
        {
            SOUND.PlayLang("bye");
        }

        public bool TalkSpecialBeQuiet(int number = 0)
        {
            /*if (number % 2 == 0)
                SOUND.PlayLang("special_mode_be_quiet_" + 0);
            else
                SOUND.PlayLang("special_mode_be_quiet_" + 1);
            
            return true;*/

            if (number == 1)
            {
                SOUND.PlayLang("special_mode_be_quiet_" + 0);
                return true;
            }
            else
                return false;


            
        }

        public void StopTalk()
        {
            SOUND.Stop();
        }

        public bool TalkFinished()
        {
            return SOUND.HasFinished();
        }

        public void GameFinishedAudio()
        {
            SOUND.PlayLang("game_finished");
        }

        public void NextGameAudio()
        {
            SOUND.PlayLang("next_game");       
        }

        // ************************************
        //                INTRO
        // ************************************
        public void ShowIntro()
        {
            Show(false);

            //INFO_OBJ.gameObject.SetActive(true);
            //INFO_OBJ.msgText.text = "";

            SetInfo(IntroMessages[0]);

            // ---------------------------
            InGameScore.me.Show();
            Parrot2.me.MOVE_TO_WOOD();
            finished = false;
            counter = 0;
            STATE = "INTRO";
            extra_state = 0;
            // ---------------------------
        }

        private void EndIntro()
        {
            Hide();
            /*if (GAME.ageMode == AgeMode.KIDS)
                Hide();
            else if (GAME.ageMode == AgeMode.ADULTS)
                Hide(false);*/
        }
        // ************************************


        // ************************************
        //                SHOW NEXT LEVEL
        // ************************************
        public void ShowNextLevelInfo(int level, bool timesUp)
        {
            this.currLevel = level;
            this.timesUp = timesUp;

            if (Config.PARROT_MOVE_TO_BOARD_AFTER_WIN)
                Show();

            //WIN_OBJ.msgText.text = "";

            //CURR_LEVEL_DATA = LEVEL_DATA[currLevel];
            
            WIN_OBJ.scoreText.text = "Score: " + InGameScore.me.GetScore();
            //WIN_OBJ.msgText.text = CURR_LEVEL_DATA.messages[0];
            WIN_OBJ.gameObject.SetActive(true);

            if (level < WIN_OBJ.levelItems.Length)
                WIN_OBJ.levelItems[level].SetComplete(true);
            
            /*INFO_OBJ.gameObject.SetActive(true);
            INFO_OBJ.msgText.text = "";

            CURR_LEVEL_DATA = LEVEL_DATA[currLevel];

            SetInfo(CURR_LEVEL_DATA.messages[0]);*/

            //DolphinTalk();

            continueTimer.setDelay(2f);
            continueTimer.start();

            if(Config.PARROT_MOVE_TO_BOARD_AFTER_WIN)
                Parrot2.me.MOVE_TO_BOARD();

            finished = false;
            counter = 0;
            goodJobMsg = false;
            STATE = "NEXT_LEVEL";
            extra_state = 0;

            
        }

        private void EndNextLevel()
        {
            Hide();
        }
        // ************************************

        private void Show(bool boardVisible = true)
        {
            INFO_OBJ.gameObject.SetActive(false);
            WIN_OBJ.gameObject.SetActive(false);

            container.SetActive(true);

            FxMat.FadeInBlack(overlayMat, 0.9f, 0.8f, OnFadeInComplete);

            ScaleIn(dolphin);
            ScaleIn(panel);

            if (boardVisible)
            { }
            else
                panel.SetActive(false);
        }

        public void RELOCO()
        {
            this.gameObject.transform.position = new Vector3(100, 2, 2);
        }

        public void Hide(bool boardVisible = true)
        {
            FxMat.FadeOutBlack(overlayMat, 0.0f, 0.6f, OnFadeOutComplete);
            ScaleOut(dolphin);
            

            if (boardVisible)
            {
                ScaleOut(panel);
            }
            else
            {
                panel.transform.localScale = new Vector3(0, 0, 0);
                panel.SetActive(true);
            }

        }

        // *************************************
        public bool HasFinished()
        {
            return finished;
        }

        public void ResetComplete()
        {
            finished = false;
            STATE = "";
        }

        public void ForceFinished()
        {
            container.SetActive(false);
            finished = true;
            STATE = "";
        }


        private void OnFadeInComplete()
        {

        }

        private void OnFadeOutComplete()
        {
            container.SetActive(false);
            finished = true;
        }

        private void ScaleIn(GameObject gobj)
        {
            //Vector3 scale = gobj.transform.localScale;
            gobj.transform.localScale = new Vector3(0, 0, 0);
            gobj.transform.DOScale(1f, 1f);
        }

        private void ScaleOut(GameObject gobj)
        {
            Vector3 scale = gobj.transform.localScale;
            gobj.transform.DOScale(0f, 0.5f);
        }

        private void SetInfo(String msg)
        {
            INFO_OBJ.msgText.text = msg;
            //DolphinTalk();
        }

        /*private void DolphinTalk()
        {
            //dolphinAnimator.SetTrigger("talk");
            //SOUND.Play("fx_dolphin_0" + dolphinTalkCount);

            dolphinTalkCount++;
            if (dolphinTalkCount > 3)
                dolphinTalkCount = 1;
        }*/

        private bool Continue()
        {
#if UNITY_EDITOR
            return (Input.GetMouseButtonDown(0));
#else
            return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
#endif
        }

        private void Update()
        {
            /*OVRInput.Controller activeController = OVRInput.GetActiveController();
            if (activeController == null)
                return;*/


            // ************************************
            //                INTRO
            // ************************************
            if (STATE == "INTRO")
            {
                if (extra_state == 0)
                {
                    if (Parrot2.me.CanTalk())
                    {
                        if(Config.PARROT_INTRO_FLY_TO_LEFT)
                            Parrot2.me.INTRO();
                        Talk();
                        extra_state = 1;
                    }
                }
                else if (extra_state == 1)
                {
                    if (TalkFinished())
                    {
                        InGameScore.me.Reset();
                        InGameScore.me.Show();
                        //Parrot2.me.MOVE_TO_WOOD();
                        extra_state = 2;
                    }
                }
                else if (extra_state == 2)
                {
                    if (Parrot2.me.IsOnWood())
                    {
                        ScaleOut(panel);
                        Talk(-2);
                        extra_state = 3;
                    }
                }
                else if (extra_state == 3)
                {
                    if (TalkFinished())
                    {
                        Talk(-3);
                        extra_state = 4;
                    }
                }
                else if (extra_state == 4)
                {
                    if (TalkFinished())
                    {
                        timer.setDelay(1f);
                        timer.start();
                        extra_state = 5;
                    }
                }
                else if (extra_state == 5)
                {
                    if (timer.update())
                    {
                        //Parrot2.me.MOVE_TO_WOOD();
                        // IMPORTANT!!! DO NOT TALK. Cause anim is overwritten//Talk(1); // talk level 1
                        //EndIntro();
                        InGameScore.me.Show();
                        extra_state = 6;
                    }
                }
                else if (extra_state == 6)
                {
                    if (Parrot2.me.CanTalk())
                    {
                        Talk(1);
                        EndIntro();
                        extra_state = 0xFF;
                    }
                }


            }
            // ************************************
            //                NEXT LEVEL
            // ************************************
            else if (STATE == "NEXT_LEVEL")
            {
                if (extra_state == 0)
                {
                    if (Parrot2.me.CanTalk()) // Check possible bug here
                    {
                        if (!goodJobMsg)
                        {
                            if (timesUp)
                            {
                                NextGameAudio();
                                timesUp = false;
                            }
                            else
                            {
                                if ((currLevel >= 0) && (currLevel <= 3))
                                    SOUND.PlayLang("mission_complete_" + currLevel);
                                else
                                    SOUND.PlayLang("mission_complete_" + UnityEngine.Random.Range(0, 4));
                            }

                            goodJobMsg = true;
                        }

                        if (SOUND.HasFinished() && /*Continue() || */continueTimer.update())
                        {
                            if (Config.PARROT_TOGGLE_FLY)
                            {
                                if (currLevel % 2 == 0)
                                    moveTo = MoveTo.WOOD;
                                else
                                    moveTo = MoveTo.FLY;
                            }
                            else
                                moveTo = MoveTo.WOOD;


                            if (moveTo == MoveTo.WOOD)
                                Parrot2.me.MOVE_TO_WOOD();
                            else if(moveTo == MoveTo.FLY)
                                Parrot2.me.FLY();
                            
                            InGameScore.me.Show();
                            timer.setDelay(1f);
                            timer.start();
                            extra_state = 1;
                        }
                    }
                }
                else if (extra_state == 1)
                {
                    if (timer.update())
                    {
                        ScaleOut(panel);
                        extra_state = 2;
                    }
                }
                else if (extra_state == 2)
                {
                    if (moveTo == MoveTo.WOOD)
                    {
                        if (Parrot2.me.IsOnWood()) 
                            extra_state = 3;
                    }
                    else if (moveTo == MoveTo.FLY)
                    {
                        if (Parrot2.me.IsFlying1()) 
                            extra_state = 3;
                    }
                }
                else if (extra_state == 3)
                {
                    Talk(currLevel + 2);
                    EndNextLevel();
                    extra_state = 0xFF;
                }

                /*if (extra_state == 0)
                {
                    if (Parrot2.me.CanTalk())
                    {
                        Talk(currLevel + 1);
                        extra_state = 1;
                    }
                }
                else if (extra_state == 1)
                {
                    if (Continue())
                    {
                        WIN_OBJ.gameObject.SetActive(false);
                        INFO_OBJ.gameObject.SetActive(true);
                        EndNextLevel();*/
                        
                        /*if (counter == 0)
                        {
                            WIN_OBJ.gameObject.SetActive(false);
                            INFO_OBJ.gameObject.SetActive(true);
                        }

                        counter++;

                        if (counter > (CURR_LEVEL_DATA.messages.Length - 1))
                        {
                            EndNextLevel();
                        }
                        else
                        {
                            SetInfo(CURR_LEVEL_DATA.messages[counter]);
                        }*/
                    /*}
                }*/
            }
        }


    }

}