using System;
using UnityEngine;

namespace GEX.utils
{
    public class XTimer
    {
        private float time;
        private float time_locked;
        private bool _started;
        private bool DONE;

        public XTimer(float delay = 1)
        {
            _started =  false;
            DONE =      false;
            setDelay(delay);
        }

        public void setDelay(float delay)
        {
            time_locked = delay;
        }

        public void start()
        {
            time =      time_locked;
            _started =  true;
            DONE =      false;
        }

        public void stop()
        {
            _started = false;
            DONE = false;
        }

        public void forceFinish()
        {
            time = 0;
            DONE = true;
        }

        public float getTimeSet()
        {
            return time_locked;
        }

        public bool update()
        {
            if (_started)
            {
                if (time > 0)
                    time -= Time.deltaTime;
                else
                {
                    _started =  false;
                    DONE =      true;
                }
            }

            return DONE;
        }

        public int getTotalSeconds()
        {
            return (int)time;
        }

        public float getRemainingTime()
        {
            return time;
        }

        public float getElapsedTime()
        {
            return (time_locked - time);
        }

        public string getAsMMSS()
        {

            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time - minutes * 60);

            string retTime = string.Format("{0:00}:{1:00}", minutes, seconds);

            return retTime;
        }

        public float getCompletePercent()
        {
            if (time == 0)
                return 0;
            return time / time_locked;
        }

        public void destroy()
        { 
        
        }

    }
}
