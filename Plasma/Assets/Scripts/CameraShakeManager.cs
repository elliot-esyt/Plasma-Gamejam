using UnityEngine;
using Cinemachine;

public class CameraShakeManager : MonoBehaviour
{

    // varibles
    public static CameraShakeManager instance; 
    [SerializeField] private float globalShakeForce = 1f;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }
}
