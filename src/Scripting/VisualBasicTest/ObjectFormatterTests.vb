' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Globalization
Imports System.Threading.Thread
Imports Microsoft.CodeAnalysis.VisualBasic
Imports Microsoft.CodeAnalysis.Scripting
Imports Microsoft.CodeAnalysis.Scripting.VisualBasic
Imports Roslyn.Test.Utilities
Imports Xunit

Namespace Roslyn.VisualBasic.Runtime.UnitTests

    Public Class ObjectFormatterTests
        Private Shared ReadOnly Hexa As ObjectFormattingOptions = New ObjectFormattingOptions(useHexadecimalNumbers:=True)
        Private Shared ReadOnly MemberList As ObjectFormattingOptions = New ObjectFormattingOptions(memberFormat:=MemberDisplayFormat.List)
        Private Shared ReadOnly Inline As ObjectFormattingOptions = New ObjectFormattingOptions(memberFormat:=MemberDisplayFormat.Inline)

        <Fact()>
        Public Sub InlineCharacters()
            Assert.Equal("ChrW(20)", VisualBasicObjectFormatter.Instance.FormatObject(ChrW(20), Inline))
            Assert.Equal("vbBack", VisualBasicObjectFormatter.Instance.FormatObject(ChrW(&H8), Inline))
        End Sub

        <Fact(Skip:="IDK")>
        Public Sub QuotedStrings()
            Dim s = "a" & ChrW(&HFFFE) & ChrW(&HFFFF) & vbCrLf & "b"

            ' ObjectFormatter should substitute spaces for non-printable characters
            Assert.Equal("""a"" & ChrW(&HABCF) & ChrW(&HABCD) & vbCrLf & ""b""", VisualBasicObjectFormatter.Instance.FormatObject(s, Hexa.Copy(quoteStrings:=True)))
            Assert.Equal("a    b", VisualBasicObjectFormatter.Instance.FormatObject(s, Hexa.Copy(quoteStrings:=False)))
        End Sub

    End Class

End Namespace
