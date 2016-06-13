﻿using Mono.Cecil;
using System.Collections.Generic;
using Telerik.JustDecompiler.Decompiler;
using Telerik.JustDecompiler.Decompiler.GotoElimination;
using Telerik.JustDecompiler.Languages.CSharp;
using Telerik.JustDecompiler.Steps;
using Telerik.JustDecompiler.Steps.SwitchByString;

namespace Telerik.JustDecompiler.Languages
{
    public static partial class LanguageFactory
    {
        private class CSharpV6 : CSharpV5, ICSharp
        {
            public override int Version
            {
                get
                {
                    return 6;
                }
            }

            public override bool SupportsGetterOnlyAutoProperties
            {
                get
                {
                    return true;
                }
            }

            public override bool SupportsInlineInitializationOfAutoProperties
            {
                get
                {
                    return true;
                }
            }

            public override bool SupportsExceptionFilters
            {
                get
                {
                    return true;
                }
            }
            
            internal override IDecompilationStep[] LanguageDecompilationSteps(bool inlineAggressively)
            {
                return new IDecompilationStep[]
                {
                    new OutParameterAssignmentAnalysisStep(),
                    new RebuildAsyncStatementsStep(),
                    new RebuildYieldStatementsStep(),
                    new RemoveDelegateCachingStep(),
                    // RebuildAnonymousDelegatesStep needs to be executed before the RebuildLambdaExpressions step
                    new RebuildAnonymousDelegatesStep(),
                    new RebuildLambdaExpressions(),
                    new ResolveDynamicVariablesStep(),
                    new GotoCancelation(),
                    new CombinedTransformerStep(),
                    new MergeUnaryAndBinaryExpression(),
                    new RemoveLastReturn(),
                    new RebuildSwitchByString(),
                    new RebuildForeachStatements(),
                    new RebuildForeachArrayStatements(),
                    new RebuildForStatements(),
                    new RebuildLockStatements(),
                    new RebuildFixedStatements(),
                    new RebuildUsingStatements(),
                    new RenameEnumValues(),
                    new FixMethodOverloadsStep(),
                    new RebuildExpressionTreesStep(),
                    new TransformMemberHandlersStep(),
                    new CodePatternsStep(inlineAggressively),
                    // TransformCatchClausesFilterExpressionStep needs to be after CodePatternsStep,
                    // because it works only if the TernaryConditionPattern has been applied.
                    new TransformCatchClausesFilterExpressionStep(),
                    new DetermineCtorInvocationStep(),
                    new DeduceImplicitDelegates(),
                    new RebuildLinqQueriesStep(),
                    new CreateIfElseIfStatementsStep(),
                    new ParenthesizeExpressionsStep(),
                    new RemoveUnusedVariablesStep(),
                    // RebuildCatchClausesFilterStep needs to be before DeclareVariablesOnFirstAssignment and after RemoveUnusedVariablesStep.
                    // RebuildCatchClausesFilterStep contains pattern matching and need to be after TransformCatchClausesFilterExpressionStep.
                    new RebuildCatchClausesFilterStep(),
                    new DeclareVariablesOnFirstAssignment(),
                    new DeclareTopLevelVariables(),
                    new AssignOutParametersStep(),
                    // There were a lot of issues when trying to merge the SelfAssignment step with the CombinedTransformerStep.
                    new SelfAssignement(),
                    new RenameSplitPropertiesMethodsAndBackingFields(),
                    new RenameVariables(),
                    new CastEnumsToIntegersStep(),
                    new CastIntegersStep(),
                    new ArrayVariablesStep(),
                    new CaseGotoTransformerStep(),
                    new UnsafeMethodBodyStep(),
                    new DetermineDestructorStep(),
                    // DependsOnAnalysisStep must be always last step, because it make analysis on the final decompilation result.
				    new DependsOnAnalysisStep(),
                };
            }

            protected override IDecompilationStep[] LanguageFilterMethodDecompilationSteps(bool inlineAggressively)
            {
                return new IDecompilationStep[]
                {
                    new DeclareVariablesOnFirstAssignment(),
                    new DeclareTopLevelVariables(),
                    new AssignOutParametersStep(),
                    // There were a lot of issues when trying to merge the SelfAssignment step with the CombinedTransformerStep.
                    new SelfAssignement(),
                    new RenameSplitPropertiesMethodsAndBackingFields(),
                    new RenameVariables(),
                    new CastEnumsToIntegersStep(),
                    new CastIntegersStep(),
                    new ArrayVariablesStep(),
                    new CaseGotoTransformerStep(),
                    new UnsafeMethodBodyStep(),
                    new DetermineDestructorStep(),
                    // DependsOnAnalysisStep must be always last step, because it make analysis on the final decompilation result.
				    new DependsOnAnalysisStep(),
                };
            }
        }
    }
}
