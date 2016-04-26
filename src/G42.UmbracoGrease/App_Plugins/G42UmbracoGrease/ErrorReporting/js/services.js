angular.module('umbraco.services').factory('greaseErrorReportingService', function ($http, $q, umbRequestHelper) {
    return {
        save: function (config) {
            return umbRequestHelper.resourcePromise(
                $http.post("/umbraco/backoffice/G42UmbracoGrease/g42errorreportingapi/save", config), 'Failed to save data'
            );
        },
        getConfig: function () {
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/G42UmbracoGrease/g42errorreportingapi/getConfig"), 'Failed to get config'
            );
        }
    }
});