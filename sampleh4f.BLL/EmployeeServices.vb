Imports sampleh4f.DAL
Imports sampleh4f.Mapper

Public Class EmployeeServices
    Public Function InsertEmployee(ByVal objEmployee As Employee) As Boolean
        Dim blnRet As Boolean = False
        Try
            Using _dbContext As New Hungry4FoodDataContext
                If objEmployee.SalesId = 0 Then
                    objEmployee.SalesId = _dbContext.Employees.DefaultIfEmpty().Max(Function(s) s.SalesId) + 1
                End If
                _dbContext.Employees.InsertOnSubmit(objEmployee)
                _dbContext.SubmitChanges()
                blnRet = True
            End Using
            Return blnRet
        Catch ex As Exception
            Return blnRet
        End Try
    End Function

    Public Function GetAllEmployee() As List(Of Employee)
        Dim employees As New List(Of Employee)
        Try
            Using _dbContext As New Hungry4FoodDataContext
                _dbContext.DeferredLoadingEnabled = False
                Dim emplyeeItems As IQueryable(Of Employee) = _dbContext.Employees
                employees = emplyeeItems.OrderByDescending(Function(a) a.SalesId).ToList
            End Using
            Return employees
        Catch ex As Exception
            Return employees
        End Try
    End Function

    Public Function GetEmployeeType() As List(Of EmployeeType)

        Using _dbContext As New Hungry4FoodDataContext
            Dim EmployeeTypes As IQueryable(Of EmployeeType) = _dbContext.EmployeeTypes.Where(Function(s) s.Active = True) '.Where(Function(a) a.StoreID = intStoreID)
            _dbContext.DeferredLoadingEnabled = False
            Return EmployeeTypes.ToList()
        End Using
    End Function

    Public Function GetActiveEmployee() As List(Of Employee)

        Using _dbContext As New Hungry4FoodDataContext
            Dim Employees As IQueryable(Of Employee) = _dbContext.Employees.Where(Function(s) s.Active = True)
            _dbContext.DeferredLoadingEnabled = False
            Return Employees.ToList()
        End Using
    End Function

    Public Function UpdateEmployee(ByVal objEmployee As Employee) As Boolean
        Dim blnRes As Boolean = False
        Using _dbContext As New Hungry4FoodDataContext
            Dim Employees As IQueryable(Of Employee) = _dbContext.Employees
            Dim result = _dbContext.Employees.Where(Function(a) a.SalesId = objEmployee.SalesId).SingleOrDefault()
            result.FirstName = objEmployee.FirstName
            result.LastName = objEmployee.LastName
            result.MiddleName = objEmployee.MiddleName
            result.StreetAdd = objEmployee.StreetAdd
            result.StoreID = objEmployee.StoreID
            result.State = objEmployee.State
            result.Ssn = objEmployee.Ssn
            result.City = objEmployee.City
            result.ZipCode = objEmployee.ZipCode
            result.Ph = objEmployee.Ph
            result.DrvPass = objEmployee.DrvPass
            result.PnchInPwd = objEmployee.PnchInPwd
            result.Email = objEmployee.Email
            result.Active = objEmployee.Active
            _dbContext.SubmitChanges()
            blnRes = True
        End Using
        Return blnRes

    End Function

    Public Function DeActivateEmployee(ByVal objEmployee As Employee) As Boolean
        Dim blnRes As Boolean = False
        Using _dbContext As New Hungry4FoodDataContext
            Dim Employees As IQueryable(Of Employee) = _dbContext.Employees
            Dim result = _dbContext.Employees.Where(Function(a) a.SalesId = objEmployee.SalesId).SingleOrDefault()
            result.FirstName = objEmployee.FirstName
            result.LastName = objEmployee.LastName
            result.MiddleName = objEmployee.MiddleName
            result.StreetAdd = objEmployee.StreetAdd
            result.StoreID = objEmployee.StoreID
            result.State = objEmployee.State
            result.Ssn = objEmployee.Ssn
            result.City = objEmployee.City
            result.ZipCode = objEmployee.ZipCode
            result.Ph = objEmployee.Ph
            result.DrvPass = objEmployee.DrvPass
            result.PnchInPwd = objEmployee.PnchInPwd
            result.Email = objEmployee.Email
            result.Active = False
            _dbContext.SubmitChanges()
            blnRes = True
        End Using
        Return blnRes

    End Function
    Public Function DeleteEmployee(ByVal intSalesID As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using _dbContext As New Hungry4FoodDataContext
            Dim Employee As IQueryable(Of Employee) = _dbContext.Employees.Where(Function(a) a.SalesId = intSalesID)
            _dbContext.Employees.DeleteAllOnSubmit(Employee)
            _dbContext.SubmitChanges()
            blnRet = True
        End Using
        Return blnRet
    End Function

    Public Function GetEmployeePrefix() As EmployeePrefix
        Try
            Dim employeePrefix As New EmployeePrefix
            Dim objStoreServices As New StoreInfoServices
            employeePrefix.storeInfos = objStoreServices.GetAllStoreInfoList()
            employeePrefix.employeeTypes = GetEmployeeType()
            'sat
            employeePrefix.employeeInfos = GetActiveEmployee()
            Return employeePrefix
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ''check availability of driver
    'Public Function GetAvailableDriverEmail(ByVal intStoreID As Integer) As List(Of Employee)
    '    Try
    '        Dim objDriverScheduleService As New DriverScheduleServices
    '        Dim driverAssignmentService As New DriverAssignmentServices
    '        Dim driverSchedules = objDriverScheduleService.GetActiveDriverAssignID()
    '        Dim lstdriverEmployee As New List(Of Employee)
    '        Dim objDriverAssignmentServices As DriverAssignmentServices
    '        'Dim intSalesID As Integer = objDriverAssignmentServices.GetActiveDriverAssignSalesID(intActiveDrvAssignID, intStoreID)

    '        Using _dbContext As New Hungry4FoodDataContext
    '            Dim driverAssignmentsActiver = _dbContext.DriverAssignments.Where(Function(a) a.Active = True And a.StoreID = intStoreID And a.StartDate.Value <= Now() And a.EndDate.Value >= Now()).ToList()

    '            Dim driverAssignmentsValid = From driverAssignment In driverAssignmentsActiver
    '                                         Join driverSchedule In driverSchedules
    '                                        On driverAssignment.Id Equals driverSchedule.DriverAssignID
    '                                         Select driverAssignment

    '            Dim driverEmployees = _dbContext.Employees.Where(Function(x) x.Active = True).ToList()

    '            Dim driverEmployeesValid = From driverEmployee In driverEmployees
    '                                       Join driverAssign In driverAssignmentsValid
    '                                           On driverEmployee.SalesId Equals driverAssign.SalesId
    '                                       Select driverEmployee

    '            lstdriverEmployee = driverEmployeesValid.ToList()
    '            'Dim Employee As IQueryable(Of Employee) = _dbContext.Employees.Where(Function(a) a.Active = 1 And a.EmployeeTypeId = 1 And a.StoreID = intStoreID And a.SalesId = intSalesID)
    '            'Dim strDriverEmail = Employee.Select(Function(a) a.Email).SingleOrDefault()
    '            'If strDriverEmail <> String.Empty Then
    '            '    Return strDriverEmail
    '            'Else

    '            'End If

    '        End Using
    '        Return lstdriverEmployee
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function

    Public Function CkeckEmployeeByEmail(ByVal strEmail As String) As Boolean
        Dim Rslt As Boolean = False
        Using _dbContext As New Hungry4FoodDataContext
            Dim Employees As IQueryable(Of Employee) = _dbContext.Employees.Where(Function(a) a.Email = strEmail)
            If Employees.Count > 0 Then
                Rslt = True
            End If
        End Using
        Return Rslt
    End Function

End Class
