#pragma warning disable 414

using UnityEngine;
using System;
using System.Collections.Generic;
using GEX.debug;
using GEX.config;

namespace GEX.media.sound
{
	class GSound
	{

        private static GameObject _root;
        //private static List<AudioSource> SOUND_CHANNEL;

        //private static AudioClip ad01, ad02;



        private static List<AudioClip> SOUND;
		private static List<AudioSource> SOUND_CHANNEL;
		private static List<int> POSITION;
        private static List<int> LENGTH;
        private static List<int> STATE;
        private static List<int> index;
		private static int sel;
		private static bool sync_valid = false;
        public static List<int> sample_sounds_count;
        private static GSoundData [] soundDataList;
        
        private static bool _startList;
        private static int _startListIndex;
		
		public const int STATE_PENDING = 	0;
		public const int STATE_PLAYING= 	1;
		public const int STATE_PAUSED= 	    2;
		public const int STATE_FINISHED=    3;
		
		private static bool INIT = false;

		// to avoid Warnings
		private static bool DBG_MSG_GSOUND {
			//get { return GConfig.DBG_MSG_GSOUND; }
			get { return GConfig.DBG_MSG_GSOUND; }
		}

        public static void initialize(GameObject root)
        {

            if(root != null)
                _root = root;

            if (!INIT)
			{
                if (DBG_MSG_GSOUND)
                    XConsole.println("<color=white>initializing GSound...</color>");
				SOUND = 					new List<AudioClip>();
				SOUND_CHANNEL = 			new List<AudioSource>();
				POSITION =					new List<int>();
				LENGTH =					new List<int>();
				STATE =						new List<int>();
                
                _startList =                false;
                _startListIndex =            0;
				
                //index =					new Vector.<uint>(32);	//max channels (on FLASH)
                //index = new List<int>(50);	//Force to be 50
                index = new List<int>(50);	//Force to be 50
                for (int i = 0; i < 50; i++)
                    index.Add(0);
                INIT = true;
                if (DBG_MSG_GSOUND)
                    XConsole.println("<color=white>GSound init OK!</color>");
			}
        
        }

        public static void add(int id, AudioClip sound, int length)
		{
            if (!INIT)
                initialize(null);

            //Debug.Log(SOUND);
            SOUND.Add(sound);
            SOUND_CHANNEL.Add(_root.AddComponent<AudioSource>());
            SOUND_CHANNEL[SOUND_CHANNEL.Count - 1].clip = sound;
			POSITION.Add(0);
			LENGTH.Add(length);
			STATE.Add(STATE_PENDING);
			index[id] = SOUND.Count - 1;

            if (DBG_MSG_GSOUND)
            {
                XConsole.println("<color=white>INDEX len:" + index.Count + "</color>");
                XConsole.println("<color=white>Sound id: " + id + " length: " + length + "</color>");
            }
		}

        public static void play(int id, float start = 0, int loops = 0)
		{
            if (DBG_MSG_GSOUND)
                XConsole.println("<color=white>play -> Sound id: " + id + "</color>");

			sel = index[id];

            if (STATE[sel] != STATE_PENDING)
                SOUND_CHANNEL[sel].Stop();

            //SOUND_CHANNEL[sel] = SOUND[sel].play(start, loops);
            SOUND_CHANNEL[sel].Play();
            STATE[sel] = STATE_PLAYING;

            SOUND_CHANNEL[sel].time = start;

            if (loops == 0xFFFFFF)
                SOUND_CHANNEL[sel].loop = true;

            if (DBG_MSG_GSOUND)
                XConsole.println("<color=white>Play:" + SOUND_CHANNEL[sel] + "   " + SOUND[sel] + "   loops: " + loops + "</color>");
			
			//Console.println("Sound id: " + id + " length: " + LENGTH[sel] + " state: " + STATE[sel]);
		}

        //play without creating a new channel
        public static void instantPlay(AudioClip audio)
        {
            AudioSource.PlayClipAtPoint(audio, new Vector3(0, 0, 0), 1);
        }

        public static void seek(int id, float pos)
        {
            sel = index[id];
            SOUND_CHANNEL[sel].time = pos;
        }

        public static void pause(int id)
		{
			/*sel = index[id];
			
			if (STATE[sel] != STATE_PENDING)
			{
				POSITION[sel] = SOUND_CHANNEL[sel].position;
				SOUND_CHANNEL[sel].stop();
				STATE[sel] = STATE_PAUSED;
			}*/
		}
		
		public static void resume(int id)
		{
			/*var position:Number;
			sel =	index[id];
			
			if (STATE[sel] != STATE_PENDING)
			{
				Console.println("DBG --- pos: " + POSITION[sel]);
				
				if(STATE[sel] == STATE_PLAYING)
					SOUND_CHANNEL[sel].stop();
				play(id, POSITION[sel]);
			}*/
		}
		
		public static void stop(int id)
		{
			sel = index[id];
			
			if (STATE[sel] != STATE_PENDING)
			{
				POSITION[sel] = 0;
                SOUND_CHANNEL[sel].Stop();
				STATE[sel] = STATE_FINISHED;
			}
		}

        public static void playAsList(GSoundData [] sndData)
        {
            soundDataList =     sndData;
            _startList =        true;
            _startListIndex =    0;

            play(soundDataList[_startListIndex].id, soundDataList[_startListIndex].start, convertToLoopInt(soundDataList[_startListIndex].loop));
        }

        public static int getState(int id)
		{
			sel = index[id];
			
			return STATE[sel];
		}
		
		public static int getLength(int id)
		{
			sel = index[id];
			
			return LENGTH[sel];
		}
		
		public static bool isComplete(int id)
		{
			sel = index[id];
			
			if (STATE[sel] != STATE_PENDING)
			{
				/*if (SOUND_CHANNEL[sel].position >= LENGTH[sel])
					return true;*/
                if(!SOUND_CHANNEL[sel].isPlaying)
                    return true;
			}
			
			return false;
		}
		
		public static void stopAll()
		{
			if (SOUND != null)
			{
				for (int a = 0; a < getTotal(); a++ )
				{
					if(STATE[a] != STATE_PENDING)
						SOUND_CHANNEL[a].Stop();
				}

                _startList = false;

			}
		}
		
		public static void destroyAll(bool stopSounds)
		{
            if (stopSounds)
			    stopAll();
			
			if (SOUND != null)
			{
				for (int a = 0; a < getTotal(); a++ )
				{
                    GameObject.Destroy(SOUND_CHANNEL[a]);
					SOUND_CHANNEL[a] = null;
					
					//SOUND[a].close();
					SOUND[a] = null;
					
					POSITION[a] =	0;
					LENGTH[a] =		0;
					STATE[a] =		0;
				}
				
			
				if (index != null)
				{
					for (int b = 0; b < index.Count; b++ )
						index[b] = 0;
				}
			}
			
			sample_sounds_count =	null;
			sync_valid = false;
			
			SOUND_CHANNEL = null;
			SOUND =			null;
			POSITION =		null;
			LENGTH =		null;
			STATE =			null;
			index =			null;
			INIT =			false;

            if (DBG_MSG_GSOUND)
                XConsole.println("<color=orange>SOUND -> Destroyed</color>");
		}
		
		public static int getTotal()
		{
			return SOUND.Count;
		}
        
        private static int convertToLoopInt(bool loop)
        {
            if (loop)
                return 0xFFFFFF;
            else
                return 0;
        }

        public static void update()
        {
           
            if (_startList)
            {
                sel = soundDataList[_startListIndex].id;

                if (!SOUND_CHANNEL[sel].isPlaying)
                {
                    _startListIndex++;

                    if (_startListIndex >= soundDataList.Length)
                    {
                        _startListIndex = 0;
                        _startList = false;
                    }
                    else
                    {
                        sel = soundDataList[_startListIndex].id;
                        play(soundDataList[_startListIndex].id, soundDataList[_startListIndex].start, convertToLoopInt(soundDataList[_startListIndex].loop));
                    }

                }

            }
        
        
        }

		/*public static int  getPosition(int id)
		{
			sel = index[id];
			
			if (STATE[sel] == STATE_PENDING)
				return 0;
			else
				return SOUND_CHANNEL[sel].position;
		}*/


	}
}
