using UnityEngine;

public class OperatingPrincipleState : ASState
{
    public GameObject[] parts;  // 需要变透明的物体部件
    public Material transparentMaterial;  // 透明材质
    private Material[] originalMaterials;  // 存储每个部件的原始材质

    public Transform largeGear;   // 大齿轮
    public Transform mediumGear;  // 中齿轮
    public Transform smallGear;   // 小齿轮
    public float rotationSpeed = 3f; // 旋转速度（可以调节）
    private bool isRotating = false;  // 是否正在旋转的标志

    [SerializeField]
    private string stateName;

    private void OnEnable()
    {
        // 获取每个部件的原始材质
        originalMaterials = new Material[parts.Length];
        for (int i = 0; i < parts.Length; i++) {
            originalMaterials[i] = parts[i].GetComponent<Renderer>().material;
        }
        SetTransparency(true);
        isRotating = true;
    }

    private void Update()
    {
        if (isRotating) {
            RotateGears();
        }
    }


    public override void Enter(ASState from)
    {
        this.gameObject.SetActive(true);
    }

    public override void Exit(ASState to)
    {
        this.gameObject.SetActive(false);
        SetTransparency(false);
        isRotating = false;
    }

    public override string GetName()
    {
        return stateName;
    }

    public void GotoHomePageState()
    {
        stateManager.SwitchState("HomePage");
    }

    public void GotoFreeDisassemblyState()
    {
        stateManager.SwitchState("FreeDisassembly");
    }

    public void GotoShowAnimationState()
    {
        stateManager.SwitchState("ShowAnimation");
    }

    // 设置物体的透明度
    private void SetTransparency(bool transparent)
    {
        for (int i = 0; i < parts.Length; i++) {
            Renderer renderer = parts[i].GetComponent<Renderer>();

            if (transparent) {
                // 切换到透明材质
                renderer.material = transparentMaterial;
            } else {
                // 恢复原始材质
                renderer.material = originalMaterials[i];
            }
        }
    }

    // 控制齿轮的旋转
    private void RotateGears()
    {
        // 控制三个齿轮的旋转
        largeGear.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        mediumGear.Rotate(Vector3.back, rotationSpeed * 2f * Time.deltaTime);  // 中齿轮反向旋转
        smallGear.Rotate(Vector3.forward, rotationSpeed * 4f * Time.deltaTime);  // 小齿轮加速旋转
    }
}
