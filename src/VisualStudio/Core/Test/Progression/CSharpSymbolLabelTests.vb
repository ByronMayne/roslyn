' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.VisualStudio.GraphModel
Imports Roslyn.Test.Utilities

Namespace Microsoft.VisualStudio.LanguageServices.UnitTests.Progression
    Public Class CSharpSymbolLabelTests
        <Fact, Trait(Traits.Feature, Traits.Features.Progression)>
        Sub NamedType()
            Using testState = New ProgressionTestState(
                    <Workspace>
                        <Project Language="C#" CommonReferences="true" FilePath="Z:\Project.csproj">
                            <Document FilePath="Z:\Project.cs">
                                class $$C { }
                            </Document>
                        </Project>
                    </Workspace>)

                testState.AssertMarkedSymbolLabelIs(GraphCommandDefinition.Contains.Id, "C", "C")
            End Using
        End Sub

        <Fact, Trait(Traits.Feature, Traits.Features.Progression)>
        Sub GenericNamedType()
            Using testState = New ProgressionTestState(
                    <Workspace>
                        <Project Language="C#" CommonReferences="true" FilePath="Z:\Project.csproj">
                            <Document FilePath="Z:\Project.cs"><![CDATA[[
                                class $$C<T> { }
                            ]]></Document>
                        </Project>
                    </Workspace>)

                testState.AssertMarkedSymbolLabelIs(GraphCommandDefinition.Contains.Id, "C<T>", "C<T>")
            End Using
        End Sub

        <Fact, Trait(Traits.Feature, Traits.Features.Progression)>
        Sub GenericMethod()
            Using testState = New ProgressionTestState(
                    <Workspace>
                        <Project Language="C#" CommonReferences="true" FilePath="Z:\Project.csproj">
                            <Document FilePath="Z:\Project.cs"><![CDATA[[
                                class C { void $$M<T>() { } }
                            ]]></Document>
                        </Project>
                    </Workspace>)

                testState.AssertMarkedSymbolLabelIs(GraphCommandDefinition.Contains.Id, "M<T>() : void", "C.M<T>() : void")
            End Using
        End Sub

        <Fact, Trait(Traits.Feature, Traits.Features.Progression)>
        Sub MethodWithParamsParameter()
            Using testState = New ProgressionTestState(
                    <Workspace>
                        <Project Language="C#" CommonReferences="true" FilePath="Z:\Project.csproj">
                            <Document FilePath="Z:\Project.cs">
                                class C { void $$M(params string[] foo) { } }
                            </Document>
                        </Project>
                    </Workspace>)

                testState.AssertMarkedSymbolLabelIs(GraphCommandDefinition.Contains.Id, "M(params string[]) : void", "C.M(params string[]) : void")
            End Using
        End Sub

        <Fact, Trait(Traits.Feature, Traits.Features.Progression)>
        Sub MethodWithOptionalParameter()
            Using testState = New ProgressionTestState(
                    <Workspace>
                        <Project Language="C#" CommonReferences="true" FilePath="Z:\Project.csproj">
                            <Document FilePath="Z:\Project.cs">
                                class C { void $$M(int i = 0) { } }
                            </Document>
                        </Project>
                    </Workspace>)

                testState.AssertMarkedSymbolLabelIs(GraphCommandDefinition.Contains.Id, "M([int]) : void", "C.M([int]) : void")
            End Using
        End Sub

        <Fact, Trait(Traits.Feature, Traits.Features.Progression)>
        Sub MethodWithRefAndOutParameters()
            Using testState = New ProgressionTestState(
                    <Workspace>
                        <Project Language="C#" CommonReferences="true" FilePath="Z:\Project.csproj">
                            <Document FilePath="Z:\Project.cs">
                                class C { void $$M(out string foo, ref string bar) { } }
                            </Document>
                        </Project>
                    </Workspace>)

                testState.AssertMarkedSymbolLabelIs(GraphCommandDefinition.Contains.Id, "M(out string, ref string) : void", "C.M(out string, ref string) : void")
            End Using
        End Sub

        <Fact, Trait(Traits.Feature, Traits.Features.Progression), WorkItem(545017)>
        Sub EnumMember()
            Using testState = New ProgressionTestState(
                    <Workspace>
                        <Project Language="C#" CommonReferences="true" FilePath="Z:\Project.csproj">
                            <Document FilePath="Z:\Project.cs">
                                enum E { $$M }
                            </Document>
                        </Project>
                    </Workspace>)

                testState.AssertMarkedSymbolLabelIs(GraphCommandDefinition.Contains.Id, "M", "E.M")
            End Using
        End Sub

        <Fact, Trait(Traits.Feature, Traits.Features.Progression), WorkItem(545014)>
        Sub Constructor()
            Using testState = New ProgressionTestState(
                    <Workspace>
                        <Project Language="C#" CommonReferences="true" FilePath="Z:\Project.csproj">
                            <Document FilePath="Z:\Project.cs">
                                class C { $$C() { } }
                            </Document>
                        </Project>
                    </Workspace>)

                testState.AssertMarkedSymbolLabelIs(GraphCommandDefinition.Contains.Id, "C()", "C.C()")
            End Using
        End Sub

        <Fact, Trait(Traits.Feature, Traits.Features.Progression), WorkItem(545014)>
        Sub Destructor()
            Using testState = New ProgressionTestState(
                    <Workspace>
                        <Project Language="C#" CommonReferences="true" FilePath="Z:\Project.csproj">
                            <Document FilePath="Z:\Project.cs">
                                class C { ~$$C() { } }
                            </Document>
                        </Project>
                    </Workspace>)

                testState.AssertMarkedSymbolLabelIs(GraphCommandDefinition.Contains.Id, "~C()", "C.~C()")
            End Using
        End Sub

        <Fact, Trait(Traits.Feature, Traits.Features.Progression), WorkItem(545013)>
        Sub ExplicitlyImplementedInterface()
            Using testState = New ProgressionTestState(
                    <Workspace>
                        <Project Language="C#" CommonReferences="true" FilePath="Z:\Project.csproj">
                            <Document FilePath="Z:\Project.cs">
                                using System;
                                class C : IDisposable { void IDisposable.$$Dispose() { } }
                            </Document>
                        </Project>
                    </Workspace>)

                testState.AssertMarkedSymbolLabelIs(GraphCommandDefinition.Contains.Id, "IDisposable.Dispose() : void", "C.Dispose() : void")
            End Using
        End Sub

        <Fact, Trait(Traits.Feature, Traits.Features.Progression), WorkItem(13229, "DevDiv_Projects/Roslyn"), WorkItem(545353)>
        Sub FixedFieldInStruct()
            Using testState = New ProgressionTestState(
                    <Workspace>
                        <Project Language="C#" CommonReferences="true" FilePath="Z:\Project.csproj">
                            <Document FilePath="Z:\Project.cs">
                                struct C { fixed int $$f[42]; }
                            </Document>
                        </Project>
                    </Workspace>)

                testState.AssertMarkedSymbolLabelIs(GraphCommandDefinition.Contains.Id, "f : int*", "C.f : int*")
            End Using
        End Sub

        <WorkItem(545011)>
        <Fact, Trait(Traits.Feature, Traits.Features.Progression), WorkItem(13229, "DevDiv_Projects/Roslyn")>
        Sub DelegateStyle()
            Using testState = New ProgressionTestState(
                    <Workspace>
                        <Project Language="C#" CommonReferences="true" FilePath="Z:\Project.csproj">
                            <Document FilePath="Z:\Project.cs">
                                delegate void $$Foo();
                            </Document>
                        </Project>
                    </Workspace>)

                testState.AssertMarkedSymbolLabelIs(GraphCommandDefinition.Contains.Id, "Foo() : void", "Foo : void")
            End Using
        End Sub
    End Class
End Namespace
