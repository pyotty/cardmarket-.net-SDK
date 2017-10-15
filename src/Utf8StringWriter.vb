Public Class Utf8StringWriter
    Inherits IO.StringWriter

    Public Overrides ReadOnly Property Encoding As Text.Encoding
        Get
            Return New Text.UTF8Encoding(False)
        End Get
    End Property
End Class


