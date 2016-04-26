angular.module('umbraco.services').factory('greaseGeneralService', function ($http, $q, umbRequestHelper) {
    return {
        save: function (config) {
            return umbRequestHelper.resourcePromise(
                $http.post("/umbraco/backoffice/G42UmbracoGrease/g42generalapi/save", config), 'Failed to save data'
            );
        },
        getConfig: function () {
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/G42UmbracoGrease/g42generalapi/getConfig"), 'Failed to get config'
            );
        }
    }
});