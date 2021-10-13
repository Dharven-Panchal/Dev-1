Public Class CasetypeDTO

    Private id As Integer
    Public Property propID() As Integer
        Get
            Return id
        End Get
        Set(ByVal value As Integer)
            id = value
        End Set
    End Property

    Private casetype As String
    Public Property propCaseType() As String
        Get
            Return casetype
        End Get
        Set(ByVal value As String)
            casetype = value
        End Set
    End Property

    Private CTValue As String
    Public Property propCaseValue() As String
        Get
            Return CTValue
        End Get
        Set(ByVal value As String)
            CTValue = value
        End Set
    End Property
End Class
