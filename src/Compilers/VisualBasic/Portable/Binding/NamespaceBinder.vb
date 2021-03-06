﻿' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.Collections.Concurrent
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.CodeAnalysis.RuntimeMembers
Imports Microsoft.CodeAnalysis.Text
Imports Microsoft.CodeAnalysis.VisualBasic.Symbols
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax
Imports Roslyn.Utilities
Imports TypeKind = Microsoft.CodeAnalysis.TypeKind

Namespace Microsoft.CodeAnalysis.VisualBasic

    ''' <summary>
    ''' A namespace binder provides the context for a namespace symbol; e.g., looking up names
    ''' inside the namespace.
    ''' </summary>
    Friend Class NamespaceBinder
        Inherits Binder

        Private ReadOnly m_nsSymbol As NamespaceSymbol

        Public Sub New(containingBinder As Binder, nsSymbol As NamespaceSymbol)
            MyBase.New(containingBinder)
            m_nsSymbol = nsSymbol
        End Sub

        Public Overrides ReadOnly Property ContainingNamespaceOrType As NamespaceOrTypeSymbol
            Get
                Return m_nsSymbol
            End Get
        End Property

        Public ReadOnly Property NamespaceSymbol As NamespaceSymbol
            Get
                Return m_nsSymbol
            End Get
        End Property

        Public Overrides ReadOnly Property ContainingType As NamedTypeSymbol
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property ContainingMember As Symbol
            Get
                Return m_nsSymbol
            End Get
        End Property

        Public Overrides ReadOnly Property IsInQuery As Boolean
            Get
                Return False
            End Get
        End Property

        Friend Overrides Sub LookupInSingleBinder(lookupResult As LookupResult,
                                                      name As String,
                                                      arity As Integer,
                                                      options As LookupOptions,
                                                      originalBinder As Binder,
                                                      <[In], Out> ByRef useSiteDiagnostics As HashSet(Of DiagnosticInfo))
            ' Look it up in the associated namespace.
            originalBinder.LookupMember(lookupResult, m_nsSymbol, name, arity, options Or LookupOptions.IgnoreExtensionMethods, useSiteDiagnostics)
        End Sub

        ''' <summary>
        ''' Collect extension methods with the given name that are in scope in this binder.
        ''' The passed in ArrayBuilder must be empty. Extension methods from the same containing type
        ''' must be grouped together. 
        ''' </summary>
        Protected Overrides Sub CollectProbableExtensionMethodsInSingleBinder(name As String,
                                                                      methods As ArrayBuilder(Of MethodSymbol),
                                                                      originalBinder As Binder)
            Debug.Assert(methods.Count = 0)
            m_nsSymbol.AppendProbableExtensionMethods(name, methods)
        End Sub

        Protected Overrides Sub AddExtensionMethodLookupSymbolsInfoInSingleBinder(nameSet As LookupSymbolsInfo,
                                                                                   options As LookupOptions,
                                                                                   originalBinder As Binder)
            m_nsSymbol.AddExtensionMethodLookupSymbolsInfo(nameSet, options, originalBinder)
        End Sub

        Friend Overrides Sub AddLookupSymbolsInfoInSingleBinder(nameSet As LookupSymbolsInfo, options As LookupOptions, originalBinder As Binder)
            ' Get names from the associated namespace
            originalBinder.AddMemberLookupSymbolsInfo(nameSet, m_nsSymbol, options Or LookupOptions.IgnoreExtensionMethods)
        End Sub
    End Class

End Namespace
