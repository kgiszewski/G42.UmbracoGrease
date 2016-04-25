angular.module('umbraco.services').factory('greaseSearchTrackerService', function ($http, $q, umbRequestHelper) {
    return {
        getKeywords: function (countFilter) {
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/G42UmbracoGrease/g42searchreportsapi/getkeywords?countFilter=" + countFilter), 'Failed to get keyword data'
            );
        }
    }
});