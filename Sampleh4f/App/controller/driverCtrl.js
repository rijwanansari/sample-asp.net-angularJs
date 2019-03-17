app.controller("driverCtrl", ["$scope", '$http', 'httpService', '$filter', 'queryStringService'
    , function ($scope, $http, httpService, $filter, queryStringService) {

        $scope.employee = {

        };
        $scope.uemployee = {

        };
        $scope.demployee = {};
        $scope.employees = [];
        $scope.employeeTypes = [];
        $scope.employeePrefix = {
            employeeTypes: null,
            storeInfos: null,
            employeeInfos:null
        };
        $scope.update = true;

        $scope.DeleteCartDemo = function () {
            try {
                var Param = { strParam: 'Demo' };

                httpService.post("DriverMain1.aspx/DeleteCartItem", Param).then(
                    function success(response) {
                        alert(response.data.d);
                        $scope.ServerResponse = response.data.d;
                    }, function error() {
                        alert("failure response");
                    }
                );

            } catch (e) {
                alert("Error!!" + e.message + "   " + e.name + e.stack);
            }
        };
        $scope.SaveEmployee = function () {
            if (ValidateEmployeeSetup()) {
            try {
                $scope.employee.EmployeeTypeId = $scope.employeeType.Id;
                $scope.employee.StoreID = $scope.storeInfo.StoreID;

               
                var Param = { employee: $scope.employee };

                httpService.post("DriverMain1.aspx/SaveEmployee", Param).then(
                    function success(response) {
                        //alert(response.data.d);
                        $scope.ServerResponse = response.data.d;    
                        alert("Record Saved Successfully !!");
                        $scope.GetAllEmployee();
                        $scope.CancelEmployee();

                    }, function error() {
                        alert("failure response SaveEmployee");
                    }
                );
            } catch (e) {
                alert('SaveEmployee ' + e.message);
                }
            }
        };

        
        function ValidateEmployeeSetup() {

            $scope.employeeform.$setSubmitted();
            angular.forEach($scope.employeeform.$error.required, function (form, key) {
                form.$pristine = false;
                if (key === 0)
                    document.getElementsByName(form.$name)[0].focus();
            });

            if ($scope.employeeform.$valid === false) {
                return false;
            } else
                return true

        };

        $scope.GetAllEmployee = function () {
            try {

                var Param = {};

                httpService.post("DriverMain1.aspx/GetAllEmployee", Param).then(
                    function success(response) {
                        //alert(response.data.d);
                        $scope.ServerResponse = response.data.d;
                        $scope.employees = JSON.parse(response.data.d);
                    }, function error() {
                        alert("failure response GetAllEmployee");
                    }
                );
            } catch (e) {
                alert('GetAllEmployee ' + e.message);
            }
        };
        var GetReference = function () {
            try {
                var Param = {};
                httpService.post("DriverMain1.aspx/GetEmployeePrefix", Param).then(
                    function success(response) {
                        //alert(response.data.d);
                        $scope.employeePrefix = JSON.parse(response.data.d);
                    }, function error() {
                        alert("failure response GetReference");
                    }
                );
            } catch (e) {
                alert('GetReference ' + e.message);
            }
        };
       
        $scope.OnInit = function () {
            GetReference();
            $scope.GetAllEmployee();           
        }
        $scope.EditEmployee = function (item) {
            try {
                $scope.employee = item;
                $scope.storeInfo = $filter('filter')($scope.employeePrefix.storeInfos, { StoreID: item.StoreID }, true)[0];
                $scope.employeeType = $filter('filter')($scope.employeePrefix.employeeTypes, { Id: item.EmployeeTypeId }, true)[0];
                $scope.update = false;
            } catch (e) {

            }
        };
        $scope.UpdateEmployee = function () {
            $scope.update = true;

            try {
                $scope.uemployee.EmployeeTypeId = $scope.employeeType.Id;
                $scope.uemployee.StoreID = $scope.storeInfo.StoreID;

                $scope.uemployee.FirstName = $scope.employee.FirstName;
                $scope.uemployee.LastName = $scope.employee.LastName;
                $scope.uemployee.StreetAdd = $scope.employee.StreetAdd;
                $scope.uemployee.City = $scope.employee.City;
                $scope.uemployee.State = $scope.employee.State;
                $scope.uemployee.ZipCode = $scope.employee.ZipCode;

                $scope.uemployee.Ph = $scope.employee.Ph;
                $scope.uemployee.DrvPass = $scope.employee.DrvPass;
                $scope.uemployee.Email = $scope.employee.Email;
                $scope.uemployee.Active = $scope.employee.Active;
                $scope.uemployee.SalesId = $scope.employee.SalesId;



               // alert($scope.storeInfo.StoreID + "  " + $scope.employeeType.Id);
                var Param = { employee: $scope.uemployee};

                httpService.post("DriverMain1.aspx/UpdateEmployee", Param).then(
                    function success(response) {
                        //alert(response.data.d);
                        $scope.ServerResponse = response.data.d;
                        alert("Record Updated Successfully");
                        $scope.GetAllEmployee();
                        $scope.CancelEmployee();
                       
                    }, function error() {
                        alert("failure response UpdateEmployee");
                    }
                );
            } catch (e) {
                alert('UpdateEmployee ' + e.message);
            }

        };
        $scope.CancelEmployee = function () {
            try {
                $scope.employee = null;
                // $scope.driverAssignment.StartDate = null;

            } catch (e) {

            }
        };
        
        $scope.DeleteEmployee = function () {
            $scope.update = true;

            try {
                //$scope.employee.EmployeeTypeId = $scope.employeeType.Id;
                //$scope.employee.StoreID = $scope.storeInfo.StoreID;
                $scope.demployee.EmployeeTypeId = $scope.employeeType.Id;
                $scope.demployee.StoreID = $scope.storeInfo.StoreID;

                $scope.demployee.FirstName = $scope.employee.FirstName;
                $scope.demployee.LastName = $scope.employee.LastName;
                $scope.demployee.StreetAdd = $scope.employee.StreetAdd;
                $scope.demployee.City = $scope.employee.City;
                $scope.demployee.State = $scope.employee.State;
                $scope.demployee.ZipCode = $scope.employee.ZipCode;

                $scope.demployee.Ph = $scope.employee.Ph;
                $scope.demployee.DrvPass = $scope.employee.DrvPass;
                $scope.demployee.Email = $scope.employee.Email;
                $scope.demployee.Active = $scope.employee.Active;
                $scope.demployee.SalesId = $scope.employee.SalesId;
                var Param = { employee: $scope.demployee };
                httpService.post("DriverMain1.aspx/DeActivateEmployee", Param).then(
                    function success(response) {
                        //alert(response.data.d);
                        $scope.ServerResponse = response.data.d;
                        alert("Employee Deactivated Successfully");
                        $scope.CancelEmployee();
                        $scope.GetAllEmployee();
                      
                    }, function error() {
                        alert("failure response DeleteEmployee");
                    }
                );
            } catch (e) {
                alert('DeleteEmployee ' + e.message);
            }

        };


        //Driver Assignment here
        $scope.driverAssignment = {

        };
        $scope.udriverAssignment = {

        };
        $scope.driverAssignments = [];
        $scope.update = true;
        $scope.SaveDriverAssignment = function () {
            if (ValidateDriverAssignmentSetup()) {

                try {
                    $scope.driverAssignment.StoreID = $scope.storeInfo.StoreID;
                    $scope.driverAssignment.SalesId = $scope.employeeInfo.SalesId;

                    var Param = { driverAssignment: $scope.driverAssignment };

                    httpService.post("DriverAssignmentSetup.aspx/SaveDriverAssignment", Param).then(
                        function success(response) {

                            $scope.ServerResponse = response.data.d;
                            alert("Record Saved Successfully !!");
                            $scope.CancelDriverAssignment();
                            $scope.GetAllDriverAssignment();
                        }, function error() {
                            alert("failure response SaveDriverAssignment");
                        }
                    );
                } catch (e) {
                    alert('SaveDriverAssignment ' + e.message);
                }
            }
        };
        //validation
        function ValidateDriverAssignmentSetup() {

            $scope.driverAssignmentform.$setSubmitted();
            angular.forEach($scope.driverAssignmentform.$error.required, function (form, key) {
                form.$pristine = false;
                if (key === 0)
                    document.getElementsByName(form.$name)[0].focus();
            });

            if ($scope.driverAssignmentform.$valid === false) {
                return false;
            } else
                return true

        };

        $scope.GetDateFormat = function (date) {
            alert(date);
            var getDate = new Date(date.match(/\d+/)[0] * 1);
            rgetDate = $filter('date')(getDate, 'MM/dd/yyyy');
            return rgetDate;
        };
        function parseJsonDate(jsonDateString) {
            alert(jsonDateString);
            return new Date(parseInt(jsonDateString.replace('/Date(', '')));
        }
        var UpdateDateFormateDriverAssignment = function(){
            angular.forEach($scope.driverAssignments, function (driverAssignment, key) {
                var startDate = new Date(driverAssignment.StartDate.match(/\d+/)[0] * 1);
                driverAssignment.StartDate = $filter('date')(startDate, 'MM/dd/yyyy');
                var endDate = new Date(driverAssignment.EndDate.match(/\d+/)[0] * 1);
                driverAssignment.EndDate = $filter('date')(endDate, 'MM/dd/yyyy');
                var index = $scope.driverAssignments.indexOf(driverAssignment);
                $scope.driverAssignments[index] = driverAssignment;
            });
        };
        $scope.GetAllDriverAssignment = function () {
            try {

                var Param = {};

                httpService.post("DriverAssignmentSetup.aspx/GetAllDriverAssignment", Param).then(
                    function success(response) {
                       // $scope.driverAssignment.StartDate = Date.parse(scope.driverAssignment.StartDate);
                        $scope.ServerResponse = response.data.d;
                        $scope.driverAssignments = JSON.parse(response.data.d);
                         UpdateDateFormateDriverAssignment();
                    }, function error() {
                        alert("failure response GetAllDriverAssignment");
                    }
                );
            } catch (e) {
                alert('GetAllDriverAssignment ' + e.message);
            }
        };

        $scope.OnInitAssignment = function () {
            GetReference();
            $scope.GetAllDriverAssignment();
        }
        $scope.EditDriverAssignment = function (item) {
            try {
                $scope.driverAssignment = item;
                $scope.storeInfo = $filter('filter')($scope.employeePrefix.storeInfos, { StoreID: item.StoreID }, true)[0];
                $scope.employeeInfo = $filter('filter')($scope.employeePrefix.employeeInfos, { SalesId: item.SalesId }, true)[0];
                //$scope.driverAssignment.StartDate = Date.parse(scope.item.StartDate);
                //var startDate = new Date(item.StartDate.match(/\d+/)[0] * 1);
                //$scope.driverAssignment.StartDate = $filter('date')(startDate, 'MM/dd/yyyy');
                //var endDate = new Date(item.EndDate.match(/\d+/)[0] * 1);
                //$scope.driverAssignment.EndDate = $filter('date')(endDate, 'MM/dd/yyyy');
                $scope.update = false;
            } catch (e) {
                alert("EditDriverAssignment " + e.message);
            }
        };
        $scope.UpdateDriverAssignment = function () {
            $scope.update = true;

            try {
                $scope.udriverAssignment.SalesId = $scope.employeeInfo.SalesId;
                $scope.udriverAssignment.StoreID = $scope.storeInfo.StoreID;

                $scope.udriverAssignment.StartDate = $scope.driverAssignment.StartDate;
                $scope.udriverAssignment.EndDate = $scope.driverAssignment.EndDate;
                $scope.udriverAssignment.Active = $scope.driverAssignment.Active;
                $scope.udriverAssignment.Id = $scope.driverAssignment.Id;

                var Param = { driverAssignment: $scope.udriverAssignment };

                httpService.post("DriverAssignmentSetup.aspx/UpdateDriverAssignment", Param).then(
                    function success(response) {
                        //alert(response.data.d);
                        $scope.ServerResponse = response.data.d;
                        alert("Record Updated Successfully");
                        $scope.GetAllDriverAssignment();
                        
                        CancelDriverAssignment();
                    }, function error() {
                        alert("failure response UpdateDriverAssignment");
                    }
                );
            } catch (e) {
                alert('UpdateDriverAssignment ' + e.message);
            }

        };
        //$scope.DriverScheduleSetup = function(){
        //    try {
        //        httpService.post("DriverScheduleSetup.aspx")
        //    } catch (e) {
        //        alert('DriverScheduleSetup ' + e.message);
        //    }
        
        //};
        $scope.CancelDriverAssignment = function () {
            try {
                $scope.driverAssignment = null;
                } catch (e) {

            }
        };
        //delete Driver Assignment 
        $scope.DelDriverAssignment = function (item) {
            try {
                $scope.driverAssignment = item;
                var Param = { intID: $scope.driverAssignment.Id };

                httpService.post("DriverAssignmentSetup.aspx/DeleteDriverAssignment", Param).then(
                    function success(response) {

                        $scope.ServerResponse = response.data.d;
                        //$scope.GetAllDriverSchedule();
                        alert("Record deleted Successfully");
                        $scope.GetAllDriverAssignment();
                    }, function error() {
                        alert("failure response DeleteDriverAssignment");
                    }
                );
            } catch (e) {
                alert('DeleteDriverAssignment ' + e.message);
            }

        };
        //End Driver Assignment


        //Driver Schedule


        $scope.driverSchedule = {
        };
        $scope.udriverSchedule = {
        };
        $scope.ddriverSchedule = {
        };
        $scope.driverSchedules = [];
        $scope.filterdriverSchedules = [];
        $scope.update = true;
        $scope.driverScheduleMapper = {
            driverSchedule: null,
            startTime: null,
            endTime:null
        }
        $scope.SaveDriverSchedule = function () {

            if (ValidateDriverScheduleSetup()) {
                try {
                    //$scope.driverAssignment.StoreID = $scope.storeInfo.StoreID;
                    //$scope.driverAssignment.SalesId = $scope.employeeInfo.SalesId;
                    $scope.driverSchedule.DriverAssignID = getUrlParameter('driverAssignId');
                    var startTime = $scope.driverSchedule.StartTime;
                    var endTime = $scope.driverSchedule.EndTime;
                    $scope.driverScheduleMapper.driverSchedule = $scope.driverSchedule;
                    $scope.driverScheduleMapper.startTime = startTime;
                    $scope.driverScheduleMapper.endTime = endTime;
                    var Param = {
                        driverScheduleMapper: $scope.driverScheduleMapper
                    };

                    httpService.post("DriverScheduleSetup.aspx/SaveDriverScheduleNew", Param).then(
                        function success(response) {

                            $scope.ServerResponse = response.data.d;
                            //$scope.GetAllDriverSchedule();
                            alert("Record Saved Successfully");
                            $scope.GetAllDriverScheduleByDriverAssgnId($scope.driverSchedule.DriverAssignID);
                            CancelDriverSchedule();
                        }, function error() {
                            alert("failure response SaveDriverSchedule");
                        }
                    );
                } catch (e) {
                    alert('SaveDriverSchedule ' + e.message);
                }
            }
        };
        //validation
        function ValidateDriverScheduleSetup() {

            $scope.driverScheduleform.$setSubmitted();
            angular.forEach($scope.driverScheduleform.$error.required, function (form, key) {
                form.$pristine = false;
                if (key === 0)
                    document.getElementsByName(form.$name)[0].focus();
            });

            if ($scope.driverScheduleform.$valid === false) {
                return false;
            } else
                return true

        };

        var UpdateTimeFormateDriverSchedule = function () {           
            angular.forEach($scope.filterdriverSchedules, function (driverSchedule, key) {
                var startTime = new Date(driverSchedule.StartTime.match(/\d+/)[0] * 1);
                driverSchedule.StartTime = $filter('date')(startTime, 'HH:mm a');
                var endTime = new Date(driverSchedule.EndTime.match(/\d+/)[0] * 1);
                driverSchedule.EndTime = $filter('date')(endTime, 'HH:mm a');

                var index = $scope.driverSchedules.indexOf(driverSchedule);
                $scope.driverSchedules[index] = driverSchedule;
            });
        };

        $scope.GetAllDriverSchedule = function () {
            try {
                var Param = {};

                httpService.post("DriverScheduleSetup.aspx/GetAllDriverSchedule", Param).then(
                    function success(response) {
                        $scope.ServerResponse = response.data.d;
                        $scope.driverSchedules = JSON.parse(response.data.d);
                        // $scope.filterdriverSchedules = $filter('filter')($scope.driverSchedules, { DriverAssignID: driverAssignId }, true);
                        // alert(JSON.stringify($scope.filterdriverSchedules));
                        UpdateTimeFormateDriverSchedule();
                    }, function error() {
                        alert("failure response GetAllDriverSchedule");
                    }
                );
            } catch (e) {
                alert('GetAllDriverSchedule ' + e.message);
            }
        };

        $scope.GetAllDriverScheduleByDriverAssgnId = function (driverAssignId) {
            try {
                var Param = { intDrvAssgnID: driverAssignId };
                //  var Param = driverAssignId;
                httpService.post("DriverScheduleSetup.aspx/GetAllDriverScheduleByDrvAssgnID", Param).then(
                    function success(response) {
                        $scope.ServerResponse = response.data.d;
                        $scope.filterdriverSchedulesMappers = JSON.parse(response.data.d);
                        $scope.filterdriverSchedules = $scope.filterdriverSchedulesMappers;
                        // alert(JSON.stringify($scope.filterdriverSchedules));
                       // UpdateTimeFormateDriverSchedule();
                    }, function error() {
                        alert("failure response GetAllDriverScheduleByDriverAssgnId");
                    }
                );

            } catch (e) {
                alert('GetAllDriverScheduleByDriverAssgnId ' + e.message);
            }
        };

        $scope.OnInitSchedule = function () {
            var driverAssignId = getUrlParameter('driverAssignId')
            $scope.GetAllDriverScheduleByDriverAssgnId(driverAssignId);
            // $scope.GetAllDriverSchedule();
            // alert(driverAssignId);
        }
        function getUrlParameter(name) {
            name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
            var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
            var results = regex.exec(location.search);
            return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
        };
        $scope.EditDriverSchedule = function (item) {
            try {
                $scope.driverSchedule = item.driverSchedule;
                $scope.driverSchedule.StartTime = item.startDate;
                $scope.driverSchedule.EndTime = item.endDate;
                // alert(JSON.stringify($scope.driverSchedule));
                //var datenow = new Date();
                //$scope.driverSchedule.StartTime = $filter('date')(datenow, 'HH:mm a');
                ////var endTime = new Date(driverSchedule.EndTime.match(/\d+/)[0] * 1);
                //$scope.driverSchedule.EndTime = $filter('date')(datenow, 'HH:mm a');
                $scope.update = false;
            } catch (e) {

            }
        };
        $scope.UpdateDriverSchedule = function () {
            $scope.update = true;

            try {
                $scope.udriverSchedule.Id = $scope.driverSchedule.Id;
                $scope.udriverSchedule.Day = $scope.driverSchedule.Day;
                $scope.udriverSchedule.StartTime = $scope.driverSchedule.StartTime;
                $scope.udriverSchedule.EndTime = $scope.driverSchedule.EndTime;
                $scope.udriverSchedule.Active = $scope.driverSchedule.Active;
                $scope.udriverSchedule.DriverAssignID = $scope.driverSchedule.DriverAssignID;
                var Param = { driverSchedule: $scope.udriverSchedule };

                httpService.post("DriverScheduleSetup.aspx/UpdateDriverSchedule", Param).then(
                    function success(response) {
                        //alert(response.data.d);
                        $scope.ServerResponse = response.data.d;
                        alert("Record Updated Successfully");
                        $scope.GetAllDriverScheduleByDriverAssgnId($scope.driverSchedule.DriverAssignID);
                        $scope.CancelDriverSchedule();
                    }, function error() {
                        alert("failure response UpdateDriverSchedule");
                    }
                );
            } catch (e) {
                alert('UpdateDriverSchedule ' + e.message);
            }

        };

        $scope.CancelDriverSchedule = function () {
            try {
                $scope.driverSchedule = null;
            } catch (e) {

            }
        };
        //End Driver Schedule


        //delete Schedule
        $scope.DelDriverSchedule = function (item) {


            try {
                $scope.driverScheduleTemp = item;
                //$scope.ddriverSchedule.Id = $scope.driverSchedule.Id;
                //$scope.ddriverSchedule.Day = $scope.driverSchedule.Day;
                //$scope.ddriverSchedule.StartTime = $scope.driverSchedule.StartTime;
                //$scope.ddriverSchedule.EndTime = $scope.driverSchedule.EndTime;
                //$scope.ddriverSchedule.Active = $scope.driverSchedule.Active;
                //$scope.ddriverSchedule.DriverAssignID = $scope.driverSchedule.DriverAssignID;
                // var Param = { driverSchedule: $scope.ddriverSchedule };
                var Param = { intID: $scope.driverScheduleTemp.Id };

                httpService.post("DriverScheduleSetup.aspx/DeleteDriverSchedule", Param).then(
                    function success(response) {
                        $scope.ServerResponse = response.data.d;
                        alert("Record deleted Successfully");
                        // $scope.GetAllDriverScheduleByDriverAssgnId($scope.driverSchedule.DriverAssignID);
                        $scope.GetAllDriverScheduleByDriverAssgnId(getUrlParameter('driverAssignId'))

                    }, function error() {
                        alert("failure response DelDriverSchedule");
                    }
                );
            } catch (e) {
                alert('DelDriverSchedule ' + e.message);
            }

        };

    }]);