Friend NotInheritable Class NativeMethods

    Public Declare Function GetScrollPos Lib "user32.dll" ( _
            ByVal hWnd As IntPtr, _
            ByVal nBar As Integer) As Integer
    Public Declare Function SetScrollPos Lib "user32.dll" ( _
            ByVal hWnd As IntPtr, _
            ByVal nBar As Integer, _
            ByVal nPos As Integer, _
            ByVal bRedraw As Boolean) As Integer
    Public Declare Function PostMessageA Lib "user32.dll" ( _
            ByVal hwnd As IntPtr, _
            ByVal wMsg As Integer, _
            ByVal wParam As Integer, _
            ByVal lParam As Integer) As Boolean
    Public Declare Auto Function SendMessage Lib "user32.dll" ( _
            ByVal hWnd As IntPtr, _
            ByVal wMsg As Int32, _
            ByVal wParam As IntPtr, _
            ByVal lParam As IntPtr) As IntPtr

End Class
