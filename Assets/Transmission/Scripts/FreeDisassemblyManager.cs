using UnityEngine;
using System.Collections.Generic;

public class FreeDisassembly : MonoBehaviour
{
    public GameObject[] parts; // 所有零件
    private GameObject selectedPart = null;
    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>(); // 存储零件的初始位置
    private bool isFreeDisassemblyMode = false;
    RaycastHit hit;
    Ray ray;
    private float clickTimeThreshold = 0.2f; // 最长点击时间阈值，单位：秒
    private float clickTime = 0f; // 记录点击时间

    void Start()
    {
        // 存储每个零件的初始位置
        foreach (var part in parts) {
            initialPositions[part] = part.transform.position;
        }
    }

    void Update()
    {
        if (isFreeDisassemblyMode) {
            // 鼠标点击选中零件
            if (Input.GetMouseButtonDown(0)) {
                // 记录点击时间
                clickTime = Time.time;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit)) {
                    SelectPart(hit.transform.gameObject);
                }
            }

            if (selectedPart != null && Input.GetMouseButton(0)) {
                // 判断是否是点击还是拖拽
                if (Time.time - clickTime < clickTimeThreshold) {
                    // 如果点击时间较短，执行点击操作
                    return;
                }

                // 动态计算 CameraZDistance
                float CameraZDistance = Camera.main.WorldToScreenPoint(transform.position).z;
                Vector3 ScreenPosition =
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance); //z axis added to screen point
                Vector3 NewWorldPosition =
                    Camera.main.ScreenToWorldPoint(ScreenPosition); //Screen point converted to world point
                selectedPart.transform.position = NewWorldPosition;
            }
        }
    }

    // 选择零件
    void SelectPart(GameObject part)
    {
        if (selectedPart != null) {
            // 取消当前选中，恢复Outline组件状态
            DeselectPart();
        }

        selectedPart = part;

        // 激活Outline组件
        Outline outline = selectedPart.GetComponent<Outline>();
        if (outline != null) {
            outline.enabled = true; // 启用Outline效果
        }
    }

    // 取消选中零件
    void DeselectPart()
    {
        if (selectedPart != null) {
            Outline outline = selectedPart.GetComponent<Outline>();
            if (outline != null) {
                outline.enabled = false; // 关闭Outline效果
            }
        }
        selectedPart = null;
    }

    // 启用自由拆卸模式
    public void EnableFreeDisassemblyMode()
    {
        isFreeDisassemblyMode = true;
    }

    // 关闭自由拆卸模式，回到初始状态
    public void DisableFreeDisassemblyMode()
    {
        isFreeDisassemblyMode = false;
        if (selectedPart != null) {
            DeselectPart();
        }

        // 恢复每个零件到初始位置
        foreach (var part in parts) {
            if (initialPositions.ContainsKey(part)) {
                part.transform.position = initialPositions[part];
            }
        }
    }
}
