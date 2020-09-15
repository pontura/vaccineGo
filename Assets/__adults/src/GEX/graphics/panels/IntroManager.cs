using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using GEX.utils;
//using com.data;
using GEX.transition;

namespace GEX.graphics.panels
{
    public enum IntroType
    {
        VIDEO = 0,
        GUI = 1
    }

    public enum IntroState
    {
        NONE = -1,
        SHOW_NEXT = 0,
        UPDATE_CURRENT = 1,
        TRANSITION_EXIT = 2,
        TRANSITION_EXIT_DESTROY = 3,
    }

    public struct IntroElement
    {
        public int resId;
        public IntroType type;
        public float seconds;

        public IntroElement(int resId, IntroType type, float seconds = 1f)
        {
            this.resId = resId;
            this.type = type;
            this.seconds = seconds;
        }
    }

    public class IntroManager
    {
       /* private static bool _finished = false;
        private static GameObject VIDEO_GOBJ;
        private static MovieTexture currMovie;
        private static GameObject CANVAS_GOBJ;
        private static XTimer timer;
        private static IntroElement[] elements;
        private static IntroElement currElement;
        private static int currIndex;

        private static IntroState state = IntroState.NONE;

        public static void initialize(IntroElement[] elements)
        {
            IntroManager.elements = elements;
            IntroManager.currIndex = -1;
            VIDEO_GOBJ = null;
            CANVAS_GOBJ = null;
            timer = new XTimer();
            _finished = false;
        }


        public static void show()
        {
            IntroManager.currIndex = -1;
            state = IntroState.SHOW_NEXT;
            _finished = false;
        }

        public static void update()
        {

            bool _continue = false;

            if (Input.anyKeyDown)
            {
                removeElements();
                TransitionFX.destroy();
                state = IntroState.SHOW_NEXT;
            }

            // ********************************
            // ****        SHOW_NEXT       ****
            // ********************************
            if (state == IntroState.SHOW_NEXT)
            {
                currIndex++;

                if (currIndex > (elements.Length - 1))
                {
                    state = IntroState.NONE;
                    destroy();
                }
                else
                {
                    currElement = elements[currIndex];

                    //
                    // Video
                    //
                    if (currElement.type == IntroType.VIDEO)
                    {
                        playVideo(currElement.resId);
                    }
                    //
                    // GUI
                    //
                    else if (currElement.type == IntroType.GUI)
                    {
                        timer.setDelay(currElement.seconds);
                        timer.start();
                        playGUI(currElement.resId);
                    }

                    state = IntroState.UPDATE_CURRENT;
                }
            }
            // ********************************
            // ****        SHOW_NEXT       ****
            // ********************************
            else if (state == IntroState.UPDATE_CURRENT)
            {
                //
                // Video
                //
                if (currElement.type == IntroType.VIDEO)
                {
                    if (!currMovie.isPlaying)
                        _continue = true;
                }
                //
                // GUI
                //
                else if (currElement.type == IntroType.GUI)
                {
                    if (timer.update())
                        _continue = true;
                }

                // 
                if (_continue)
                {
                    state = IntroState.TRANSITION_EXIT;
                    TransitionFX.fadeIn();
                }

            }
            // ********************************
            // ****        FADE_OUT        ****
            // ********************************
            else if (state == IntroState.TRANSITION_EXIT)
            {
                if (TransitionFX.update())
                {
                    removeElements();

                    TransitionFX.destroy();
                    state = IntroState.SHOW_NEXT;
                }

            }

        }

        private static void removeElements()
        {
            if (VIDEO_GOBJ != null)
            {
                GameObject.Destroy(VIDEO_GOBJ);
                VIDEO_GOBJ = null;
            }
            if (CANVAS_GOBJ != null)
            {
                GameObject.Destroy(CANVAS_GOBJ);
                CANVAS_GOBJ = null;
            }
        }

        public static bool hasFinished()
        {
            return _finished;
        }

        public static void destroy()
        {
            _finished = true;
        }

        private static void playVideo(int videoID)
        {
            VIDEO_GOBJ = Res.getVideo(videoID);
            currMovie = VIDEO_GOBJ.renderer.material.mainTexture as MovieTexture;
            currMovie.Play();
        }

        private static void playGUI(int id)
        {
            CANVAS_GOBJ = XGUI.addGUI(Res.getGUIElement(id, false));
            //CANVAS_GOBJ.SetActive(false);
            CANVAS_GOBJ.SetActive(true);
            XGuiFX.setAlpha(CANVAS_GOBJ, 0f);
            XGuiFX.setAlpha(CANVAS_GOBJ, 1, 0.3f);
        }*/

    }
}
