using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cubequad
{
    public class HeadersFont : MonoBehaviour
    {
        public Font HeaderDefaultFont;
        public static Font HeadersDefaultFont;
        // Start is called before the first frame update
        void Start()
        {
            HeadersDefaultFont = HeaderDefaultFont;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}