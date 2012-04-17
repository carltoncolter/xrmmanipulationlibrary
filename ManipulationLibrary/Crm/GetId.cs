// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 2011
//  File:		GetId.cs
//  Summary:	This workflow activity gets the id and logical name of the context 
//              entity
// ==================================================================================
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Crm
{
    public sealed class GetId : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();

            ID.Set(executionContext, context.PrimaryEntityId.ToString());
            LogicalName.Set(executionContext,context.PrimaryEntityName);
        }

        [Output("ID")]
        public OutArgument<string> ID { get; set; }

        [Output("LogicalName")]
        public OutArgument<string> LogicalName { get; set; }
    }
}