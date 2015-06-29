angular.module('umbraco.services').factory('greaseAppSettingsService', function ($http, $q, umbRequestHelper) {
    return {
        getAppSetting: function (key) {
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/G42UmbracoGrease/appsettingsapi/get?key=" + key), 'Failed to get app setting Umbraco'
            );
        },
        setAppSetting: function (key, value) {
            return umbRequestHelper.resourcePromise(
                $http.post("/umbraco/backoffice/G42UmbracoGrease/appsettingsapi/set", { key: key, value: value }), 'Failed to set app setting Umbraco'
            );
        }
    }
});