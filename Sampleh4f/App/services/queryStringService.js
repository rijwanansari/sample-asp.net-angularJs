app.service('queryStringService', ['$location', function ($location) {
    this.getFilters = function (filterObj) {
        var queryString = $location.search();
        for (var param in filterObj) {
            if (param in queryString) {
                filterObj[param] = queryString[param];
            }
        }
        return filterObj;
    };
    this.getFiltersNew = function (filterObj) {

        for (var param in filterObj) {
            name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
            var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
            var results = regex.exec(location.search);
            var value = results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
            filterObj[param] = value
        }
    };
}]);