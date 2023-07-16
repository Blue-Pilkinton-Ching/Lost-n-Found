using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BloobyJoobyJoob.Menu {
    [CreateAssetMenu(fileName = "ButtonSettings", menuName = "ScriptableObjects/ButtonSettings", order = 0)]
    public class ButtonSettings : ScriptableObject {
        public float HoverFadeTime = 0.2f;
        public float ClickFadeTime = 0.1f;
        public float ClickDelay = 0.2f;
        public Color HoverColor = Color.grey;
        public Color ClickColor = Color.black;
    }
}

