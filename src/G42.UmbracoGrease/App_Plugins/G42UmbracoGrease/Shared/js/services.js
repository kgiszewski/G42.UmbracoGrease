angular.module('umbraco.services').factory('greaseReportsService', function ($http, $q, umbRequestHelper) {
    return {
        get404s: function (countFilter) {
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/G42UmbracoGrease/g42404reportsapi/get404s?countFilter=" + countFilter), 'Failed to get 404 data'
            );
        },
        getKeywords: function (countFilter) {
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/G42UmbracoGrease/g42searchreportsapi/getkeywords?countFilter=" + countFilter), 'Failed to get keyword data'
            );
        }
    }
});