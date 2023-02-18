using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake i { get; private set; }
    private CinemachineVirtualCamera vmc;
    private Camera cam;

    private void Awake() 
    {
        i = this;
        vmc = GetComponent<CinemachineVirtualCamera>();    
        cam = GetComponentInParent<Camera>();
    }

    public void Shake(float intensity)
    {
        CinemachineBasicMultiChannelPerlin cbmcp = vmc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cbmcp.m_AmplitudeGain = intensity;
    }

    public void StopShake()
    {
        CinemachineBasicMultiChannelPerlin cbmcp = vmc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = 0;
        cam.transform.rotation = Quaternion.Euler(0,0,0);
    }
}
