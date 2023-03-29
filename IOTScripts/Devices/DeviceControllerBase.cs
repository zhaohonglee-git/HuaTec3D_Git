using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceControllerBase : MonoBehaviour
{
    public string ProductKey, DeviceName, DeviceSecret;
    [Space(20)]
    public List<Transform> AllTransforms = new List<Transform>();

    [Header("�豸�Խ�IOT��ƽ̨Json��ʽ���ݵ�����Keys")]
    public List<string> AllComponentsKeys = new List<string>();
    public Transform Sensors;
    public Transform Actuators;

    private void Start()
    {
        for (int i = 0; i < Sensors.childCount; i++)
        {
            Transform theTransform = Sensors.GetChild(i);
            AllTransforms.Add(theTransform);
            AllComponentsKeys.Add(theTransform.GetComponent<SensorsBase>().Identifier);
        }

        for (int i = 0; i < Actuators.childCount; i++)
        {
            Transform theTransform = Actuators.GetChild(i);
            AllTransforms.Add(theTransform);
            ActuatorsBase theActuatorsBase = theTransform.GetComponent<ActuatorsBase>();

            for (int j = 0; j < theActuatorsBase.ActuatorsBaseFuncs.Count; j++)
            {
                AllComponentsKeys.Add(theActuatorsBase.ActuatorsBaseFuncs[j].Identifier);
            }
        }

    }
}
