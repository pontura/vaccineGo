#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace GEX.components
{
    [CustomEditor(typeof(UGridBuilder))]
	public class UGridBuilderEditor : Editor
	{


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UGridBuilder myScript = (UGridBuilder)target;
            if (GUILayout.Button("Apply"))
            {
                myScript.arrange();
            }
        }



      

	}
}
#endif
