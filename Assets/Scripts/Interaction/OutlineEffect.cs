using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    public Material outlineMat;

    Camera cam;
    Camera outlineBufferCam;
    Shader outlineBufferShader;

    [SerializeField]
    RenderTexture preOutlineBuffer;

    [SerializeField]
    RenderTexture preOutlineDepthBuffer;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.Depth;

        outlineBufferCam = new GameObject().AddComponent<Camera>();
        outlineBufferCam.gameObject.name = "OutlineCamera";

        outlineBufferCam.CopyFrom(cam);
        outlineBufferCam.transform.parent = transform;
        outlineBufferCam.clearFlags = CameraClearFlags.Color;
        outlineBufferCam.backgroundColor = Color.black;
        outlineBufferCam.cullingMask = 1 << LayerMask.NameToLayer("Outline");
        outlineBufferCam.renderingPath = RenderingPath.Forward;
        outlineBufferCam.depthTextureMode = DepthTextureMode.None;
        outlineBufferCam.enabled = false;

        outlineBufferShader = Shader.Find("Custom/OutlineBuffer");
    }

    void Update()
    {
        if (!preOutlineBuffer || preOutlineBuffer.width != Screen.width || preOutlineBuffer.height != Screen.height)
        {
            preOutlineBuffer = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.R8);
            preOutlineBuffer.name = "Pre-outline buffer";
            preOutlineBuffer.Create();
        }

        if (!preOutlineDepthBuffer || preOutlineDepthBuffer.width != Screen.width || preOutlineDepthBuffer.height != Screen.height)
        {
            preOutlineDepthBuffer = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.Depth);
            preOutlineDepthBuffer.name = "Pre-outline depth buffer";
            preOutlineDepthBuffer.Create();
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        outlineBufferCam.SetTargetBuffers(preOutlineBuffer.colorBuffer, preOutlineDepthBuffer.depthBuffer);
        outlineBufferCam.RenderWithShader(outlineBufferShader, null);
        outlineMat.SetTexture("_SceneTex", source);
        outlineMat.SetTexture("_OutlineDepthBuffer", preOutlineDepthBuffer);
        Graphics.Blit(preOutlineBuffer, destination, outlineMat);
    }

}