// Class info from UnityEngine.dll
// 
using UnityEngine;

namespace UnityEngine
{
    class SliderHandler : ValueType
    {
      // Fields:
  position : Rect
  currentValue : Single
  size : Single
  start : Single
  end : Single
  slider : GUIStyle
  thumb : GUIStyle
  horiz : Boolean
  id : Int32
      // Properties:
      // Events:
      // Methods:
      public Void UnityEngine.SliderHandler::.ctor(UnityEngine.RectSingleSingleSingleSingle,UnityEngine.GUIStyle,UnityEngine.GUIStyleBooleanInt32)
      public Single UnityEngine.SliderHandler::Handle()
      Single UnityEngine.SliderHandler::OnMouseDown()
      Single UnityEngine.SliderHandler::OnMouseDrag()
      Single UnityEngine.SliderHandler::OnMouseUp()
      Single UnityEngine.SliderHandler::OnRepaint()
      UnityEngine.EventType UnityEngine.SliderHandler::CurrentEventType()
      Int32 UnityEngine.SliderHandler::CurrentScrollTroughSide()
      Boolean UnityEngine.SliderHandler::IsEmptySlider()
      Boolean UnityEngine.SliderHandler::SupportsPageMovements()
      Single UnityEngine.SliderHandler::PageMovementValue()
      Single UnityEngine.SliderHandler::PageUpMovementBound()
      UnityEngine.Event UnityEngine.SliderHandler::CurrentEvent()
      Single UnityEngine.SliderHandler::ValueForCurrentMousePosition()
      Single UnityEngine.SliderHandler::ClampSingle)
      UnityEngine.Rect UnityEngine.SliderHandler::ThumbSelectionRect()
      Void UnityEngine.SliderHandler::StartDraggingWithValueSingle)
      UnityEngine.SliderState UnityEngine.SliderHandler::SliderState()
      UnityEngine.Rect UnityEngine.SliderHandler::ThumbRect()
      UnityEngine.Rect UnityEngine.SliderHandler::VerticalThumbRect()
      UnityEngine.Rect UnityEngine.SliderHandler::HorizontalThumbRect()
      Single UnityEngine.SliderHandler::ClampedCurrentValue()
      Single UnityEngine.SliderHandler::MousePosition()
      Single UnityEngine.SliderHandler::ValuesPerPixel()
      Single UnityEngine.SliderHandler::ThumbSize()
      Single UnityEngine.SliderHandler::MaxValue()
      Single UnityEngine.SliderHandler::MinValue()
    }
}
