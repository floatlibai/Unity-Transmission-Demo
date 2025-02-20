using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[System.Serializable]
public struct MachineStruct
{
    // 存储所有零件
    public Transform MachineArray;
    // 存储所有零件的目标点
    public Transform MachineArray_Target;
    // 临时存储原点
    [HideInInspector] public Vector3 TempMachineArray;
    // 延时时间
    public float delaytime;
}

public class SplitManager : MonoBehaviour
{
    [Header("零件拆卸时间")]
    [Range(0.1f, 10)]
    public float duration = 1;
    [Header("零件面板：")]
    public MachineStruct[] MachineStruct;
    [Header("按钮：")]
    public Button btn_Split;
    public Button btn_Merge;

    void Start()
    {
        for (int i = 0; i < MachineStruct.Length; i++) {
            MachineStruct[i].TempMachineArray = MachineStruct[i].MachineArray.position;
        }

        btn_Split.onClick.AddListener(() => {
            StartCoroutine(SequenceSplit(duration));
        });

        btn_Merge.onClick.AddListener(() => {
            StartCoroutine(SequenceMerge(duration));
        });
    }

    IEnumerator SequenceSplit(float split_duration)
    {
        for (int i = 0; i < MachineStruct.Length; i++) {
            yield return new WaitForSeconds(MachineStruct[i].delaytime);
            MachineStruct[i].MachineArray.DOMove(
                    MachineStruct[i].MachineArray_Target.position,
                    split_duration
                );
        }
    }

    IEnumerator SequenceMerge(float reverse_duration)
    {
        for (int i = MachineStruct.Length - 1; i >= 0; i--) {
            yield return new WaitForSeconds(MachineStruct[i].delaytime);
            MachineStruct[i].MachineArray.DOMove(
                    MachineStruct[i].TempMachineArray,
                    reverse_duration
                );
        }
    }

    public void ReturnHomeAction()
    {
        // 恢复每个零件到初始位置
        for (int i = MachineStruct.Length - 1; i >= 0; i--) {
            MachineStruct[i].MachineArray.position = MachineStruct[i].TempMachineArray;
        }
    }

}
