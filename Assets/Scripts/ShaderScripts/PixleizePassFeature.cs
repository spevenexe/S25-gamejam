using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule.Util;
using System;
//using UnityEngine.Experimental.Rendering;
//using static UnityEditor.ShaderData;

//This example blits the active CameraColor to a new texture. It shows how to do a blit with material, and how to use the ResourceData to avoid another blit back to the active color target.
//This example is for API demonstrative purposes. 


// This pass blits the whole screen for a given material to a temp texture, and swaps the UniversalResourceData.cameraColor to this temp texture.
// Therefor, the next pass that references the cameraColor will reference this new temp texture as the cameraColor, saving us a blit. 
// Using the ResourceData, you can manage swapping of resources yourself and don't need a bespoke API like the SwapColorBuffer API that was specific for the cameraColor. 
// This allows you to write more decoupled passes without the added costs of avoidable copies/blits.
public class PixelizePass : ScriptableRenderPass
{
    const string m_PassName = "PixelizePass";

    // Material used in the blit operation.
    Material m_BlitMaterial;
    private int pixelScreenHeight, pixelScreenWidth;

    // Function used to transfer the material from the renderer feature to the render pass.
    public void Setup(Material mat, int screenHeight,float aspect)
    {
        m_BlitMaterial = mat;
        //The pass will read the current color texture. That needs to be an intermediate texture. It's not supported to use the BackBuffer as input texture. 
        //By setting this property, URP will automatically create an intermediate texture. 
        //It's good practice to set it here and not from the RenderFeature. This way, the pass is selfcontaining and you can use it to directly enqueue the pass from a monobehaviour without a RenderFeature.
        requiresIntermediateTexture = true;
        pixelScreenHeight = screenHeight;
        pixelScreenWidth = (int)(pixelScreenHeight * aspect + 0.5f);
        m_BlitMaterial.SetVector("_BlockCount", new Vector2(pixelScreenWidth, pixelScreenHeight));
        m_BlitMaterial.SetVector("_BlockSize", new Vector2(1.0f / pixelScreenWidth, 1.0f / pixelScreenHeight));
        m_BlitMaterial.SetVector("_HalfBlockSize", new Vector2(0.5f / pixelScreenWidth, 0.5f / pixelScreenHeight));
    }

    internal void Setup(Material mat, float pixelDensity, Camera camera)
    {
        m_BlitMaterial = mat;
        //The pass will read the current color texture. That needs to be an intermediate texture. It's not supported to use the BackBuffer as input texture. 
        //By setting this property, URP will automatically create an intermediate texture. 
        //It's good practice to set it here and not from the RenderFeature. This way, the pass is selfcontaining and you can use it to directly enqueue the pass from a monobehaviour without a RenderFeature.
        requiresIntermediateTexture = true;
        pixelScreenHeight = (int) (camera.pixelWidth / pixelDensity);
        pixelScreenWidth = (int) (camera.pixelHeight / pixelDensity);
        m_BlitMaterial.SetVector("_BlockCount", new Vector2(pixelScreenWidth, pixelScreenHeight));
        m_BlitMaterial.SetVector("_BlockSize", new Vector2(1.0f / pixelScreenWidth, 1.0f / pixelScreenHeight));
        m_BlitMaterial.SetVector("_HalfBlockSize", new Vector2(0.5f / pixelScreenWidth, 0.5f / pixelScreenHeight));
    }

    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        // UniversalResourceData contains all the texture handles used by the renderer, including the active color and depth textures
        // The active color and depth textures are the main color and depth buffers that the camera renders into
        var resourceData = frameData.Get<UniversalResourceData>();
        //This should never happen since we set m_Pass.requiresIntermediateTexture = true;
        //Unless you set the render event to AfterRendering, where we only have the BackBuffer. 
        if (resourceData.isActiveTargetBackBuffer)
        {
            Debug.LogError($"Skipping render pass. PixelizeEffectRendererFeature requires an intermediate ColorTexture, we can't use the BackBuffer as a texture input.");
            return;
        }

        // The destination texture is created here, 
        // the texture is created with the same dimensions as the active color texture
        var source = resourceData.activeColorTexture;

        var destinationDesc = renderGraph.GetTextureDesc(source);
        destinationDesc.name = $"CameraColor-{m_PassName}";
        destinationDesc.clearBuffer = false;
        destinationDesc.height = pixelScreenHeight;
        destinationDesc.width = pixelScreenWidth;
        destinationDesc.filterMode = FilterMode.Point;

        TextureHandle destination = renderGraph.CreateTexture(destinationDesc);

        RenderGraphUtils.BlitMaterialParameters para = new(source, destination, m_BlitMaterial, 0);
        renderGraph.AddBlitPass(para, passName: m_PassName);

        //FrameData allows to get and set internal pipeline buffers. Here we update the CameraColorBuffer to the texture that we just wrote to in this pass. 
        //Because RenderGraph manages the pipeline resources and dependencies, following up passes will correctly use the right color buffer.
        //This optimization has some caveats. You have to be careful when the color buffer is persistent across frames and between different cameras, such as in camera stacking.
        //In those cases you need to make sure your texture is an RTHandle and that you properly manage the lifecycle of it.
        resourceData.cameraColor = destination;
    }
}

public class PixleizePassFeature : ScriptableRendererFeature
{    
    [Tooltip("The material used when making the blit operation.")]
    public Material material;

    [Tooltip("The event where to inject the pass.")]
    public RenderPassEvent injectionPoint = RenderPassEvent.AfterRenderingPostProcessing;

    [Tooltip("The desired compressed size")]
    public int screenHeight = 144;

    // public float pixelDensity = 4;

    PixelizePass m_Pass;

    // Here you can create passes and do the initialization of them. This is called everytime serialization happens.
    public override void Create()
    {
        m_Pass = new PixelizePass();

        // Configures where the render pass should be injected.
        m_Pass.renderPassEvent = injectionPoint;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        #if UNITY_EDITOR
            if (renderingData.cameraData.isSceneViewCamera) return;
        #endif

        // Early exit if there are no materials.
        if (material == null)
        {
            Debug.LogWarning("PixelizePassRendererFeature material is null and will be skipped.");
            return;
        }


        m_Pass.Setup(material,screenHeight,renderingData.cameraData.camera.aspect);
        // m_Pass.Setup(material,pixelDensity,renderingData.cameraData.camera);
        renderer.EnqueuePass(m_Pass);        
    }
}