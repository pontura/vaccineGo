using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GEX.utils
{
	public class XGraphics
	{
        public static void setAsGrayscale(GameObject root, Material grayscaleMaterial)
        {
            List<GameObject> gobjs = XFinder.getAllChilds(root);
            gobjs.Add(root);

            for (int a = 0; a < gobjs.Count; a++)
            {
                Image img = gobjs[a].GetComponent<Image>();
                if (img != null)
                {
                    if (grayscaleMaterial != null)
                        img.material = grayscaleMaterial;
                    else
                        img.material = null;
                }
            }
        }

        public static void removeGrayscale(GameObject root)
        {
            setAsGrayscale(root, null);
        }

	}
}
