// *****************************************************
// CIXReader
// SpellEditorBase_Mono.cs
// 
// Author: Steve Palmer (spalmer@cix)
// 
// Created: 20/06/2015 17:40
//  
// Copyright (C) 2013-2015 CIX Online Ltd. All Rights Reserved.
// *****************************************************

using System.Windows.Forms;
using CIXReader.Utilities;

namespace CIXReader.Forms
{
    /// <summary>
    /// Spell editor base for Mono.
    /// This is intentionally empty since the Windows implementation merely
    /// overrode the Editor property to attach the spelling checker. Mono has
    /// no spelling checker so no override is required, but we need to have
    /// the class anyway.
    /// </summary>
    internal class SpellEditorBase : MessageEditorBase
    {
    }
}