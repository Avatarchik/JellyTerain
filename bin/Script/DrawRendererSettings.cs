// Class info from UnityEngine.dll
// 
using UnityEngine;

namespace UnityEngine.Experimental.Rendering
{
    public class DrawRendererSettings : ValueType
    {
      // Fields:
  public sorting : DrawRendererSortSettings
  public shaderPassName : ShaderPassName
  public inputFilter : InputFilter
  public rendererConfiguration : RendererConfiguration
  public flags : DrawRendererFlags
  _cullResults : IntPtr
      // Properties:
  cullResults : CullResults
      // Events:
      // Methods:
      public Void UnityEngine.Experimental.Rendering.DrawRendererSettings::.ctor(UnityEngine.Experimental.Rendering.CullResults,UnityEngine.Camera,UnityEngine.Experimental.Rendering.ShaderPassName)
      public Void UnityEngine.Experimental.Rendering.DrawRendererSettings::set_cullResults(UnityEngine.Experimental.Rendering.CullResults)
      Void UnityEngine.Experimental.Rendering.DrawRendererSettings::InitializeSortSettings(UnityEngine.Camera,UnityEngine.Experimental.Rendering.DrawRendererSortSettings&)
    }
}
