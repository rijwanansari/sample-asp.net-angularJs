app.service('httpService', ['$http', function ($http) {
    var httpService = {};
    var apiServiceUl = '';
    var config = 'application/json; charset=utf-8';
    httpService.get = function (url, data) {
        return $http.get(url
            , data
            , config);
    }

    httpService.post = function (url, data) {
        var returnData = "=" + JSON.stringify(data);
        return $http.post(url
            , data
            , config);
    }
    httpService.postNew = function (url, data) {
        var returnData = "=" + JSON.stringify(data);
        return $http.post(url
            , data
            , config);
    }

    httpService.upload = function (url, data) {
        return $http.post(apiServiceUl + url
            , data
            , config);
    }

    return httpService;
}]);