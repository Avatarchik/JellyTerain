// Class info from UnityEngine.dll
// 
using UnityEngine;

namespace UnityEngine
{
    public class RenderBuffer : ValueType
    {
      // Fields:
  m_RenderTextureInstanceID : Int32
  m_BufferPtr : IntPtr
      // Properties:
  loadAction : RenderBufferLoadAction
  storeAction : RenderBufferStoreAction
      // Events:
      // Methods:
      Void UnityEngine.RenderBuffer::SetLoadAction(UnityEngine.Rendering.RenderBufferLoadAction)
      Void UnityEngine.RenderBuffer::SetStoreAction(UnityEngine.Rendering.RenderBufferStoreAction)
      UnityEngine.Rendering.RenderBufferLoadAction UnityEngine.RenderBuffer::get_loadAction()
      Void UnityEngine.RenderBuffer::set_loadAction(UnityEngine.Rendering.RenderBufferLoadAction)
      UnityEngine.Rendering.RenderBufferStoreAction UnityEngine.RenderBuffer::get_storeAction()
      Void UnityEngine.RenderBuffer::set_storeAction(UnityEngine.Rendering.RenderBufferStoreAction)
      public IntPtr UnityEngine.RenderBuffer::GetNativeRenderBufferPtr()
    }
}
