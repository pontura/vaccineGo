using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEX.utils
{
    

	public class XUtils
	{

        public static int[] decodeIntStrArr(String str)
        {
            String [] strArr = str.Split(new char[] { ',' });
            int [] intArr = new int[strArr.Length];

            for(int a = 0; a < strArr.Length; a++)
                intArr[a] = int.Parse(strArr[a]);

            return intArr;
        }

        public static XKeyCollection decodeKeys(String str)
        {

            String[] strArr = str.Split(new char[] { ',' });
            String[] data;
            XKeyCollection collection = new XKeyCollection();

            for (int a = 0; a < strArr.Length; a++)
            {
                data = strArr[a].Split(new char[] { ':' });
                collection.addKey(new XKey(data[0], data[1]));
            }

            return collection;
        }

	}
}
