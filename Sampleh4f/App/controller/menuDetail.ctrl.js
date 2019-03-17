app.controller("menuDetailCtrl", ["$scope", '$http', 'httpService', '$filter', 'queryStringService'
    , function ($scope, $http, httpService, $filter, queryStringService) {

        $scope.Title = "Menu Details";
        var defaultFilters = {
            Sid: '',
            sm: null
        };
        $scope.storeId = "";
        $scope.itemDetails = [];
        $scope.itemDetailsFilter = {
            itemDetails: []
        };
        $scope.itemDetailsFilters = [];
        $scope.item = {};
        $scope.items = [];
        $scope.itemActive = {};
        $scope.itemDetailsActive = {
            itemDetails: []
        };
        $scope.promo = {}
        $scope.promos = []
        $scope.responseData = {};
        $scope.pageLoading = false;
        $scope.menuDetailLoading = false;
        $scope.fmenuDetailLoading = false;
        $scope.collaspeIn = "collapse in";
        $scope.expandFirstMenu = true;


        $scope.OnInit = function () {
            var storeId = getUrlParameter('Sid');
            if (storeId !== "") {
                $scope.storeId = storeId;
                $scope.GetAllItems($scope.storeId);
                $scope.GetAllPromos($scope.storeId);
                CheckStoreOpen($scope.storeId);
            }
            else {
                $scope.GetAllItems(0);
                $scope.GetAllPromos(0);
            }


        };

        $scope.GetAllItemDetails = function (itemNo) {
            try {
                var Param = { intStoreId: 1, mainItemNo: itemNo };
                $scope.menuDetailLoading = true;
                httpService.post("NewMenuDetails.aspx/GetAllItems", Param).then(
                    function success(response) {
                        $scope.itemDetails = JSON.parse(response.data.d);
                        $scope.itemDetailsFilters[itemNo] = $scope.itemDetails;
                        $scope.menuDetailLoading = false;
                    }, function error() {
                        alert("failure response GetAllItems");
                    }
                );
            } catch (e) {
                alert('GetAllItemDetails ' + e);
            }
            return $scope.itemDetails
        };

        $scope.GetAllItemDetailsActive = function (itemNo, storeId) {
            try {
                var Param = { intStoreId: storeId, mainItemNo: itemNo };
                $scope.pageLoading = true;
                httpService.post("NewMenuDetails.aspx/GetAllItems", Param).then(
                    function success(response) {
                        $scope.itemDetails = JSON.parse(response.data.d);
                        $scope.itemDetailsActive = $scope.itemDetails;
                        $scope.pageLoading = false;
                    }, function error() {
                        alert("failure response GetAllItems");
                    }
                );
            } catch (e) {
                alert('GetAllItemDetails ' + e);
            }
            return $scope.itemDetails
        };

        $scope.GetAllItems = function (storeId) {
            try {
                var Param = { intStoreId: storeId };
                //  var Param = driverAssignId;
                $scope.pageLoading = true;
                httpService.post("NewMenuDetails.aspx/GetAllMainItems", Param).then(
                    function success(response) {
                        $scope.items = JSON.parse(response.data.d);
                        $scope.itemActive = $scope.items[0];
                        $scope.LoadActiveMenuDetails($scope.itemActive);
                        $scope.pageLoading = false;
                    }, function error() {
                        alert("failure response GetAllDriverScheduleByDriverAssgnId");
                    }
                );
            } catch (e) {
                alert('GetAllItemDetails ' + e);
            }
        };
        $scope.GetAllPromos = function (storeId) {
            try {
                $scope.responseData = {};
                var Param = { intStoreId: storeId };
                //  var Param = driverAssignId;
                //$scope.pageLoading = true;
                httpService.post("NewMenuDetails.aspx/GetAllPromo", Param).then(
                    function success(response) {
                        $scope.responseData = JSON.parse(response.data.d);
                        if ($scope.responseData.success)
                            $scope.promos = $scope.responseData.output;    
                        //$scope.items = JSON.parse(response.data.d);
                        //$scope.itemActive = $scope.items[0];
                        //$scope.LoadActiveMenuDetails($scope.itemActive);
                        $scope.pageLoading = false;
                    }, function error() {
                        alert("failure response GetAllPromos");
                    }
                );
            } catch (e) {
                alert('GetAllItemDetails ' + e);
            }
        };
        $scope.LoadActiveMenuDetails = function (item) {
            if (item !== null) {
                $scope.collaspeIn = "collapse in";
                $scope.expandFirstMenu = true;
                $scope.itemActive = item;
                $scope.GetAllItemDetailsActive(item.MainItemNo, item.StoreID);
            }
        }
        CheckStoreOpen = function (storeId) {
            $scope.responseData = {};
            try {
                var Param = { intStoreId: storeId };
                $scope.pageLoading = true;
                httpService.post("NewMenuDetails.aspx/checkIsStoreOpen", Param).then(
                    function success(response) {
                        $scope.pageLoading = false;
                        if (response.data.d === "False") {
                            swal({
                                title: "Store is Closed Now!!",
                                text: "Would you like place Order for Future?",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "Yes, Continue!",
                                closeOnConfirm: false
                            },
                                function (isConfirm) {
                                    swal("Continue!", "You can place order for Future", "success");
                                    if (isConfirm) {
                                        swal("Continue!", "You can place order for Future", "success");
                                    } else {
                                        //swal("Cancelled", "Your imaginary file is safe :)", "error");
                                        window.location = '/Default.aspx';
                                    }
                                });
                        }
                    }, function error(xhr, textStatus, error) {
                        sweetAlert("Oops...", "Something went wrong!" + xhr.statusText + "||" + textStatus + error, "error");
                        $scope.pageLoading = false;
                    }
                );      
            } catch (e) {
                sweetAlert("Oops...", "Something went wrong!" + xhr.statusText + "||" + textStatus + error, "error");
                $scope.pageLoading = false;
            }   
        };
        function getUrlParameter(name) {
            name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
            var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
            var results = regex.exec(location.search);
            return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
        };

        
    }]);