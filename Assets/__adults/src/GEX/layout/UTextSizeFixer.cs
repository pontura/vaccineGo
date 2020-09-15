using UnityEngine;
using UnityEngine.UI;
using GEX.graphics;
using UnityEngine.EventSystems;
using GEX.utils;

namespace GEX.layout
{
    [ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
    public class UTextSizeFixer : UIBehaviour
    {

        public float resizeRelation = 0.2f;

        protected override void OnRectTransformDimensionsChange()
        {

            UElement uelement = new UElement(this.transform.parent.gameObject);
            Text TXT = this.gameObject.GetComponent<Text>();

            if (TXT != null)
                TXT.fontSize = (int)(uelement.height * resizeRelation);

            uelement.destroy();
            uelement = null;

            base.OnRectTransformDimensionsChange();


        }


    }
}
