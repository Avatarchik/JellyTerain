// Class info from UnityEngine.dll
// 
using UnityEngine;

namespace UnityEngine.Experimental.Director
{
    public class PlayableGraph : ValueType
    {
      // Fields:
  m_Handle : IntPtr
  m_Version : Int32
      // Properties:
  isDone : Boolean
  playableCount : Int32
  scriptOutputCount : Int32
  rootPlayableCount : Int32
      // Events:
      // Methods:
      public Boolean UnityEngine.Experimental.Director.PlayableGraph::IsValid()
      Boolean UnityEngine.Experimental.Director.PlayableGraph::IsValidInternal(UnityEngine.Experimental.Director.PlayableGraph&)
      public UnityEngine.Experimental.Director.PlayableGraph UnityEngine.Experimental.Director.PlayableGraph::CreateGraph()
      Void UnityEngine.Experimental.Director.PlayableGraph::InternalCreate(UnityEngine.Experimental.Director.PlayableGraph&)
      public Boolean UnityEngine.Experimental.Director.PlayableGraph::get_isDone()
      Boolean UnityEngine.Experimental.Director.PlayableGraph::InternalIsDone(UnityEngine.Experimental.Director.PlayableGraph&)
      public Void UnityEngine.Experimental.Director.PlayableGraph::Play()
      Void UnityEngine.Experimental.Director.PlayableGraph::InternalPlay(UnityEngine.Experimental.Director.PlayableGraph&)
      public Void UnityEngine.Experimental.Director.PlayableGraph::Stop()
      Void UnityEngine.Experimental.Director.PlayableGraph::InternalStop(UnityEngine.Experimental.Director.PlayableGraph&)
      public Int32 UnityEngine.Experimental.Director.PlayableGraph::get_playableCount()
      Int32 UnityEngine.Experimental.Director.PlayableGraph::InternalPlayableCount(UnityEngine.Experimental.Director.PlayableGraph&)
      public UnityEngine.Experimental.Director.ScriptPlayableOutput UnityEngine.Experimental.Director.PlayableGraph::CreateScriptOutputString)
      Boolean UnityEngine.Experimental.Director.PlayableGraph::InternalCreateScriptOutput(UnityEngine.Experimental.Director.PlayableGraph&String,UnityEngine.Experimental.Director.PlayableOutput&)
      public Int32 UnityEngine.Experimental.Director.PlayableGraph::get_scriptOutputCount()
      Int32 UnityEngine.Experimental.Director.PlayableGraph::internalScriptOutputCount(UnityEngine.Experimental.Director.PlayableGraph&)
      public UnityEngine.Experimental.Director.ScriptPlayableOutput UnityEngine.Experimental.Director.PlayableGraph::GetScriptOutputInt32)
      Boolean UnityEngine.Experimental.Director.PlayableGraph::InternalGetScriptOutput(UnityEngine.Experimental.Director.PlayableGraph&Int32,UnityEngine.Experimental.Director.PlayableOutput&)
      public UnityEngine.Experimental.Director.PlayableHandle UnityEngine.Experimental.Director.PlayableGraph::CreatePlayable()
      public UnityEngine.Experimental.Director.PlayableHandle UnityEngine.Experimental.Director.PlayableGraph::CreateGenericMixerPlayable()
      public UnityEngine.Experimental.Director.PlayableHandle UnityEngine.Experimental.Director.PlayableGraph::CreateGenericMixerPlayableInt32)
      public UnityEngine.Experimental.Director.PlayableHandle UnityEngine.Experimental.Director.PlayableGraph::CreateScriptPlayable()
      Boolean UnityEngine.Experimental.Director.PlayableGraph::InternalCreatePlayable(UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableHandle&)
      Boolean UnityEngine.Experimental.Director.PlayableGraph::INTERNAL_CALL_InternalCreatePlayable(UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableHandle&)
      Object UnityEngine.Experimental.Director.PlayableGraph::InternalCreateScriptPlayable(UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableHandle&Type)
      Object UnityEngine.Experimental.Director.PlayableGraph::INTERNAL_CALL_InternalCreateScriptPlayable(UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableHandle&Type)
      public Void UnityEngine.Experimental.Director.PlayableGraph::Destroy()
      Void UnityEngine.Experimental.Director.PlayableGraph::DestroyInternal(UnityEngine.Experimental.Director.PlayableGraph&)
      public Boolean UnityEngine.Experimental.Director.PlayableGraph::Connect(UnityEngine.Experimental.Director.PlayableHandleInt32,UnityEngine.Experimental.Director.PlayableHandleInt32)
      public Boolean UnityEngine.Experimental.Director.PlayableGraph::Connect(UnityEngine.Experimental.Director.PlayableInt32,UnityEngine.Experimental.Director.PlayableInt32)
      Boolean UnityEngine.Experimental.Director.PlayableGraph::ConnectInternal(UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableHandleInt32,UnityEngine.Experimental.Director.PlayableHandleInt32)
      Boolean UnityEngine.Experimental.Director.PlayableGraph::INTERNAL_CALL_ConnectInternal(UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableHandle&Int32,UnityEngine.Experimental.Director.PlayableHandle&Int32)
      public Void UnityEngine.Experimental.Director.PlayableGraph::Disconnect(UnityEngine.Experimental.Director.PlayableInt32)
      public Void UnityEngine.Experimental.Director.PlayableGraph::Disconnect(UnityEngine.Experimental.Director.PlayableHandleInt32)
      Void UnityEngine.Experimental.Director.PlayableGraph::DisconnectInternal(UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableHandle&Int32)
      Void UnityEngine.Experimental.Director.PlayableGraph::INTERNAL_CALL_DisconnectInternal(UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableHandle&Int32)
      public Void UnityEngine.Experimental.Director.PlayableGraph::DestroyPlayable(UnityEngine.Experimental.Director.PlayableHandle)
      Void UnityEngine.Experimental.Director.PlayableGraph::InternalDestroyPlayable(UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableHandle&)
      Void UnityEngine.Experimental.Director.PlayableGraph::INTERNAL_CALL_InternalDestroyPlayable(UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableHandle&)
      public Void UnityEngine.Experimental.Director.PlayableGraph::DestroyOutput(UnityEngine.Experimental.Director.ScriptPlayableOutput)
      Void UnityEngine.Experimental.Director.PlayableGraph::InternalDestroyOutput(UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableOutput&)
      public Void UnityEngine.Experimental.Director.PlayableGraph::DestroySubgraph(UnityEngine.Experimental.Director.PlayableHandle)
      Void UnityEngine.Experimental.Director.PlayableGraph::InternalDestroySubgraph(UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableHandle)
      Void UnityEngine.Experimental.Director.PlayableGraph::INTERNAL_CALL_InternalDestroySubgraph(UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableHandle&)
      public Void UnityEngine.Experimental.Director.PlayableGraph::Evaluate()
      public Void UnityEngine.Experimental.Director.PlayableGraph::EvaluateSingle)
      Void UnityEngine.Experimental.Director.PlayableGraph::InternalEvaluate(UnityEngine.Experimental.Director.PlayableGraph&Single)
      public Int32 UnityEngine.Experimental.Director.PlayableGraph::get_rootPlayableCount()
      Int32 UnityEngine.Experimental.Director.PlayableGraph::InternalRootPlayableCount(UnityEngine.Experimental.Director.PlayableGraph&)
      public UnityEngine.Experimental.Director.PlayableHandle UnityEngine.Experimental.Director.PlayableGraph::GetRootPlayableInt32)
      Void UnityEngine.Experimental.Director.PlayableGraph::InternalGetRootPlayableInt32,UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableHandle&)
      Void UnityEngine.Experimental.Director.PlayableGraph::INTERNAL_CALL_InternalGetRootPlayableInt32,UnityEngine.Experimental.Director.PlayableGraph&,UnityEngine.Experimental.Director.PlayableHandle&)
    }
}
