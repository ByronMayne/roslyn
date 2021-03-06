// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.
#region Assembly Microsoft.VisualStudio.Debugger.Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// References\Debugger\v2.0\Microsoft.VisualStudio.Debugger.Engine.dll

#endregion
using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Debugger.Clr;
using Microsoft.VisualStudio.Debugger.ComponentInterfaces;

namespace Microsoft.VisualStudio.Debugger.Evaluation
{
    //
    // Summary:
    //     Options and target context to use while performing the inspection operation.
    [Guid("0807c826-3338-dd99-2f3a-202ba8fb9da7")]
    public class DkmInspectionContext
    {
        internal DkmInspectionContext(IDkmClrFormatter formatter, DkmEvaluationFlags evaluationFlags, uint radix)
        {
            _formatter = formatter;
            this.EvaluationFlags = evaluationFlags;
            this.Radix = radix;
        }

        private readonly IDkmClrFormatter _formatter;

        public readonly DkmEvaluationFlags EvaluationFlags;

        //
        // Summary:
        //     The radix to use when formatting integer data. Currently supported values are
        //     '16' and '10'.
        public readonly uint Radix;

        public string GetTypeName(DkmClrType clrType)
        {
            // The real version does some sort of dynamic dispatch that ultimately calls this method.
            return _formatter.GetTypeName(this, clrType);
        }
    }
}
