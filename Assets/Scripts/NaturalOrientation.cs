using UnityEngine;
using System.Collections;

public class NaturalOrientation : MonoBehaviour
{

    public static bool tablet = false;
    


    void Start()
    {
#if UNITY_ANDROID
        if ((float) Screen.width / (float)Screen.height > 0.60f )
            tablet = true;
#elif UNITY_IOS
        float check = (float)Screen.width / (float)Screen.height;
        if (SystemInfo.deviceModel.Contains("iPad"))
            tablet = true;
        else if (ThisPlatform.IsIphoneX)
            tablet = false;
        else if ((float)Screen.width / (float)Screen.height > 0.60f )
            tablet = true;
#endif
    }
    public class ThisPlatform
    {

        // This property allows you to use ThisPlatform.IsIphoneX to determine if you should do anything special in your code when checking for iPhone X.
        public static bool IsIphoneX
        {
            get
            {
#if UNITY_IOS

                // If testing without an iPhone X, add FORCE_IPHONEX to your Scripting Define Symbols.
                // Useful when using Xcode's Simulator, or on any other device that is not an iPhone X.
#if FORCE_IPHONEX
                     return true;
#else

                // iOS.Device.generation doesn't reliably report iPhone X (sometimes it's "unknown"), but it's there in case it ever properly works.
                if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX)
                {
                    return true;
                }

                // As a last resort to see if the device is iPhone X, check the reported device model/identifier.
                /*string deviceModel = SystemInfo.deviceModel;
                if (deviceModel == IphoneX.Identifier_A || deviceModel == IphoneX.Identifier_B) {
                    return true;
                }*/
                float height = Screen.height;
                float width = Screen.width;
                float result = height + width;
                if (result == 3561f)
                    return true;

                return false;
#endif
#else
                return false;
#endif
            }
        }

    }
}
