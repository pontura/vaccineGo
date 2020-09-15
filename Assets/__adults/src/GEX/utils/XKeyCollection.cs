using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GEX.utils
{
	public class XKeyCollection
	{
        public List<XKey> keys;

        public XKeyCollection()
        {
            keys = new List<XKey>();
        }

        public String getKeyValue(String name)
        {
            for (int a = 0; a < keys.Count; a++)
            {
                if (keys[a].name == name)
                    return keys[a].value;
            }

            return "";
        }

        public int getIntValue(String name)
        {
            String str = getKeyValue(name);
            return int.Parse(str);
        }

        public int [] getIntArrayValue(String name)
        {
            String str = getKeyValue(name).Trim();

            if (str == "")
                return null;

            String [] strArr = str.Split(new char[] { '|' });
            int [] intArr = new int[strArr.Length];

            for(int a = 0; a < strArr.Length; a++)
                intArr[a] = int.Parse(strArr[a]);

            return intArr;
        }

        public decimal getDecimalValue(String name)
        {
            String str = getKeyValue(name);
            return decimal.Parse(str);
        }

        public void addKey(XKey key)
        {
            keys.Add(key);
        }

	}
}
