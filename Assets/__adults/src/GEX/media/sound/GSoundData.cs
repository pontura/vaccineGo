using System;
using System.Collections.Generic;


namespace GEX.media.sound
{
	public class GSoundData
	{
        public int      id;
        public float    start;
        public bool     loop;

        public GSoundData(int id, float start = 0, bool loop = false)
        {

            this.id =       id;
            this.start =    start;
            this.loop =     loop;


        }







	}
}
