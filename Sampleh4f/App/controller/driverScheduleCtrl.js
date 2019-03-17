app.controller("driverCtrl", ["$scope", '$http', 'httpService', '$filter', 'queryStringService'
    , function ($scope, $http, httpService, $filter, queryStringService) {
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


        $scope.SaveDriverSchedule = function () {

            if (ValidateDriverScheduleSetup()) {
                try {
                    //$scope.driverAssignment.StoreID = $scope.storeInfo.StoreID;
                    //$scope.driverAssignment.SalesId = $scope.employeeInfo.SalesId;
                    $scope.driverSchedule.DriverAssignID = getUrlParameter('driverAssignId');
                    var Param = { driverSchedule: $scope.driverSchedule };

                    httpService.post("DriverScheduleSetup.aspx/SaveDriverSchedule", Param).then(
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
            // angular.forEach($scope.driverSchedules, function (driverSchedule, key) {
            //     var startTime = new Date(driverSchedule.StartTime.match(/\d+/)[0] * 1);
            //     driverSchedule.StartTime = $filter('date')(startTime, 'HH:mm a');
            //     var endTime = new Date(driverSchedule.EndTime.match(/\d+/)[0] * 1);
            //     driverSchedule.EndTime = $filter('date')(endTime, 'HH:mm a');

            //     var index = $scope.driverSchedules.indexOf(driverSchedule);
            //     $scope.driverSchedules[index] = driverSchedule;
            //});
            //for driverschedulebyDrvID
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
                        $scope.filterdriverSchedules = JSON.parse(response.data.d);
                        // alert(JSON.stringify($scope.filterdriverSchedules));
                        UpdateTimeFormateDriverSchedule();
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
                $scope.driverSchedule = item;
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

        //Timing


        //        $scope.time = {
        //            twelve: new Date(),
        //            twentyfour: new Date()
        //        };

        //        $scope.message = {
        //            hour: 'Hour is required',
        //            minute: 'Minute is required',
        //            meridiem: 'Meridiem is required'
        //        }

        ////data table
        //        $scope.data = GetAllDriverAssignment();
        //        $scope.dataTableOpt = {
        //            //custom datatable options 
        //            // or load data through ajax call also
        //            "aLengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'All']],
        //        };

        //delete Schedule
        $scope.DelDriverSchedule = function (item) {


            try {
                $scope.driverSchedule = item;
                //$scope.ddriverSchedule.Id = $scope.driverSchedule.Id;
                //$scope.ddriverSchedule.Day = $scope.driverSchedule.Day;
                //$scope.ddriverSchedule.StartTime = $scope.driverSchedule.StartTime;
                //$scope.ddriverSchedule.EndTime = $scope.driverSchedule.EndTime;
                //$scope.ddriverSchedule.Active = $scope.driverSchedule.Active;
                //$scope.ddriverSchedule.DriverAssignID = $scope.driverSchedule.DriverAssignID;
                // var Param = { driverSchedule: $scope.ddriverSchedule };
                var Param = { intID: $scope.driverSchedule.Id };

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