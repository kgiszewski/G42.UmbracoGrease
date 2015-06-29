angular.module('umbraco.services').factory('ndAppSettingsService', function ($http, $q, umbRequestHelper) {
    return {
        getAppSetting: function (key) {
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/ndapplication/sharedapi/getAppSetting?key=" + key), 'Failed to get app setting Umbraco'
            );
        },
        setAppSetting: function (key, value) {
            return umbRequestHelper.resourcePromise(
                $http.post("/umbraco/backoffice/ndapplication/sharedapi/setAppSetting", {key: key, value: value}), 'Failed to set app setting Umbraco'
            );
        }
    }
});