using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileInteraction : MonoBehaviour 
{
    public Unity_iOS_Interaction interactionController;
    public Button camBtn;
    public Button albumBtn;

    private void Start()
    {
        camBtn.onClick.AddListener(OnClickCamBtnm);
        albumBtn.onClick.AddListener(OnClickAlbumBtn);
    }

    private void OnClickCamBtnm()
    {
#if UNITY_IOS
        interactionController.OpenMobileCamera();
#endif
    }

    private void OnClickAlbumBtn()
    {
#if UNITY_IOS
        interactionController.OpenMobileAlbum();
#endif
    }
}
