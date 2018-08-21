using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Unity_iOS_Interaction : MonoBehaviour 
{
    [DllImport("__Internal")]
    private static extern void IOS_OpenCamera();
    [DllImport("__Internal")]
    private static extern void IOS_OpenAlbum();

    public void OpenMobileCamera()
    {
        IOS_OpenCamera();
    }
    public void OpenMobileAlbum()
    {
        IOS_OpenAlbum();
    }

    private void CallbackFromUnity(string imageName)
    {
        if (imageName == "")
            Debug.Log("[zqdebug]ios calls unity : Select Image Canceled");
        else
            Debug.Log("[zqdebug]ios calls unity: " + imageName);
    }
}
