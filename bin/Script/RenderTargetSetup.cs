// Class info from UnityEngine.dll
// 
using UnityEngine;

namespace UnityEngine
{
    public class RenderTargetSetup : ValueType
    {
      // Fields:
  public color : RenderBuffer[]
  public depth : RenderBuffer
  public mipLevel : Int32
  public cubemapFace : CubemapFace
  public depthSlice : Int32
  public colorLoad : RenderBufferLoadAction[]
  public colorStore : RenderBufferStoreAction[]
  public depthLoad : RenderBufferLoadAction
  public depthStore : RenderBufferStoreAction
      // Properties:
      // Events:
      // Methods:
      public Void UnityEngine.RenderTargetSetup::.ctor(UnityEngine.RenderBuffer[],UnityEngine.RenderBufferInt32,UnityEngine.CubemapFace,UnityEngine.Rendering.RenderBufferLoadAction[],UnityEngine.Rendering.RenderBufferStoreAction[],UnityEngine.Rendering.RenderBufferLoadAction,UnityEngine.Rendering.RenderBufferStoreAction)
      public Void UnityEngine.RenderTargetSetup::.ctor(UnityEngine.RenderBuffer,UnityEngine.RenderBuffer)
      public Void UnityEngine.RenderTargetSetup::.ctor(UnityEngine.RenderBuffer,UnityEngine.RenderBufferInt32)
      public Void UnityEngine.RenderTargetSetup::.ctor(UnityEngine.RenderBuffer,UnityEngine.RenderBufferInt32,UnityEngine.CubemapFace)
      public Void UnityEngine.RenderTargetSetup::.ctor(UnityEngine.RenderBuffer,UnityEngine.RenderBufferInt32,UnityEngine.CubemapFaceInt32)
      public Void UnityEngine.RenderTargetSetup::.ctor(UnityEngine.RenderBuffer[],UnityEngine.RenderBuffer)
      public Void UnityEngine.RenderTargetSetup::.ctor(UnityEngine.RenderBuffer[],UnityEngine.RenderBufferInt32)
      public Void UnityEngine.RenderTargetSetup::.ctor(UnityEngine.RenderBuffer[],UnityEngine.RenderBufferInt32,UnityEngine.CubemapFace)
      UnityEngine.Rendering.RenderBufferLoadAction[] UnityEngine.RenderTargetSetup::LoadActions(UnityEngine.RenderBuffer[])
      UnityEngine.Rendering.RenderBufferStoreAction[] UnityEngine.RenderTargetSetup::StoreActions(UnityEngine.RenderBuffer[])
    }
}
