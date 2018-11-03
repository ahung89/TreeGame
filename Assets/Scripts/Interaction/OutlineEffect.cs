using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    private Camera cam;
    private Camera outlineBufferCam;
    private Shader outlineBufferShader;
    private RenderTexture preOutlineBuffer;

    public Material outlineMat;

    void Awake()
    {
        cam = GetComponent<Camera>();
        outlineBufferCam = new GameObject().AddComponent<Camera>();
        outlineBufferCam.gameObject.name = "OutlineCamera";

        outlineBufferCam.CopyFrom(cam);
        outlineBufferCam.transform.parent = transform;
        outlineBufferCam.clearFlags = CameraClearFlags.Color;
        outlineBufferCam.backgroundColor = Color.black;
        outlineBufferCam.cullingMask = 1 << LayerMask.NameToLayer("Outline");
        outlineBufferCam.enabled = false;

        outlineBufferShader = Shader.Find("OutlineBuffer");
    }

    void Update()
    {
        if (!preOutlineBuffer || preOutlineBuffer.width != Screen.width || preOutlineBuffer.height != Screen.height)
        {
            preOutlineBuffer = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.R8);
            preOutlineBuffer.Create();
            outlineBufferCam.targetTexture = preOutlineBuffer;
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        outlineBufferCam.RenderWithShader(outlineBufferShader, "");
        outlineMat.SetTexture("_SceneTex", source);
        Graphics.Blit(preOutlineBuffer, destination, outlineMat);
    }
}