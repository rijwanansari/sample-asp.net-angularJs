Imports System.Web.Script.Serialization
Imports System.Web.Services
Imports Sampleh4f.DAL
Imports Sampleh4f.BLL
Imports System.Threading.Tasks
Imports System.Web.Script.Services

Public Class DriverMain1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <WebMethod()>
    Public Shared Function DeleteCartItem(ByVal strParam As String) As String
        Try
            Return Now.ToString + " " + strParam
        Catch ex As Exception
            Return "Error-server"
        End Try

    End Function

    <WebMethod()>
    Public Shared Function SaveEmployee(ByVal employee As Employee) As String
        Dim blnrtn As Boolean = False
        Dim js As New JavaScriptSerializer
        Try
            Dim employeeServices As New EmployeeServices()
            ' 'Return employeeServices.InsertEmployee(employee)

            ' employee.FirstName = employee.FirstName + " server" + Now.ToString
            ' Return employee
            If employeeServices.CkeckEmployeeByEmail(employee.Email) Then
                Return js.Serialize("This email is already registered!!")
            End If
            If employeeServices.InsertEmployee(employee) = True Then
                Return js.Serialize("Successful")
            Else
                Return js.Serialize("Failure")
            End If

        Catch ex As Exception
            Return js.Serialize("Server Error: " + ex.Message)
        End Try
    End Function

    <WebMethod()>
    Public Shared Function GetAllEmployee() As String
        Dim js As New JavaScriptSerializer
        Try
            Dim employeeServices As New EmployeeServices()
            Return js.Serialize(employeeServices.GetAllEmployee())
        Catch ex As Exception
            Return "Server Error " + ex.Message
        End Try
    End Function
    <WebMethod>
    Public Shared Function GetEmployeePrefix() As String
        Dim js As New JavaScriptSerializer
        Try
            Dim objEmployeeServices As New EmployeeServices
            Return js.Serialize(objEmployeeServices.GetEmployeePrefix())
        Catch ex As Exception
            Return "Server Error " + ex.Message
        End Try
    End Function


    <WebMethod>
    Public Sub LoadEmployeeType()
        Dim objEmployeeServices As New EmployeeServices
        Dim lstEmployeeType As List(Of EmployeeType) = Nothing
        lstEmployeeType = objEmployeeServices.GetEmployeeType()

    End Sub


    <WebMethod()>
    Public Shared Function UpdateEmployee(ByVal employee As Employee) As String
        Dim blnrtn As Boolean = False
        Dim js As New JavaScriptSerializer
        Try
            Dim employeeServices As New EmployeeServices()

            If employeeServices.UpdateEmployee(employee) = True Then
                Return js.Serialize("Successful")
            Else
                Return js.Serialize("Failure")
            End If

        Catch ex As Exception
            Return js.Serialize("Server Error: " + ex.Message)
        End Try
    End Function
    <WebMethod()>
    Public Shared Function DeActivateEmployee(ByVal employee As Employee) As String
        Dim blnrtn As Boolean = False
        Dim js As New JavaScriptSerializer
        Try
            Dim employeeServices As New EmployeeServices()

            If employeeServices.DeActivateEmployee(employee) = True Then
                Return js.Serialize("Successful")
            Else
                Return js.Serialize("Failure")
            End If

        Catch ex As Exception
            Return js.Serialize("Server Error: " + ex.Message)
        End Try
    End Function


End Class