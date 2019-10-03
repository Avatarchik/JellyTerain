// Class info from System.dll
// 
using UnityEngine;

namespace System.ComponentModel.Design
{
    public interface IDesignerHost
    {
      // Fields:
      // Properties:
  Container : IContainer
  InTransaction : Boolean
  Loading : Boolean
  RootComponent : IComponent
  RootComponentClassName : String
  TransactionDescription : String
      // Events:
      Activated : EventHandler
      Deactivated : EventHandler
      LoadComplete : EventHandler
      TransactionClosed : DesignerTransactionCloseEventHandler
      TransactionClosing : DesignerTransactionCloseEventHandler
      TransactionOpened : EventHandler
      TransactionOpening : EventHandler
      // Methods:
      public VoidComponentModel.Design.IDesignerHost::add_ActivatedEventHandler)
      public VoidComponentModel.Design.IDesignerHost::remove_ActivatedEventHandler)
      public VoidComponentModel.Design.IDesignerHost::add_DeactivatedEventHandler)
      public VoidComponentModel.Design.IDesignerHost::remove_DeactivatedEventHandler)
      public VoidComponentModel.Design.IDesignerHost::add_LoadCompleteEventHandler)
      public VoidComponentModel.Design.IDesignerHost::remove_LoadCompleteEventHandler)
      public VoidComponentModel.Design.IDesignerHost::add_TransactionClosedComponentModel.Design.DesignerTransactionCloseEventHandler)
      public VoidComponentModel.Design.IDesignerHost::remove_TransactionClosedComponentModel.Design.DesignerTransactionCloseEventHandler)
      public VoidComponentModel.Design.IDesignerHost::add_TransactionClosingComponentModel.Design.DesignerTransactionCloseEventHandler)
      public VoidComponentModel.Design.IDesignerHost::remove_TransactionClosingComponentModel.Design.DesignerTransactionCloseEventHandler)
      public VoidComponentModel.Design.IDesignerHost::add_TransactionOpenedEventHandler)
      public VoidComponentModel.Design.IDesignerHost::remove_TransactionOpenedEventHandler)
      public VoidComponentModel.Design.IDesignerHost::add_TransactionOpeningEventHandler)
      public VoidComponentModel.Design.IDesignerHost::remove_TransactionOpeningEventHandler)
      public ComponentModel.IContainerComponentModel.Design.IDesignerHost::get_Container()
      public BooleanComponentModel.Design.IDesignerHost::get_InTransaction()
      public BooleanComponentModel.Design.IDesignerHost::get_Loading()
      public ComponentModel.IComponentComponentModel.Design.IDesignerHost::get_RootComponent()
      public StringComponentModel.Design.IDesignerHost::get_RootComponentClassName()
      public StringComponentModel.Design.IDesignerHost::get_TransactionDescription()
      public VoidComponentModel.Design.IDesignerHost::Activate()
      public ComponentModel.IComponentComponentModel.Design.IDesignerHost::CreateComponentType)
      public ComponentModel.IComponentComponentModel.Design.IDesignerHost::CreateComponentTypeString)
      public ComponentModel.Design.DesignerTransactionComponentModel.Design.IDesignerHost::CreateTransaction()
      public ComponentModel.Design.DesignerTransactionComponentModel.Design.IDesignerHost::CreateTransactionString)
      public VoidComponentModel.Design.IDesignerHost::DestroyComponentComponentModel.IComponent)
      public ComponentModel.Design.IDesignerComponentModel.Design.IDesignerHost::GetDesignerComponentModel.IComponent)
      public TypeComponentModel.Design.IDesignerHost::GetTypeString)
    }
}
