angular.module('umbraco.services').factory('greaseAppSettingsService', function ($http, $q, umbRequestHelper) {
    return {
        getAppSetting: function (key) {
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/G42UmbracoGrease/appsettingsapi/get?key=" + key), 'Failed to get app setting from Umbraco'
            );
        },
        getAll: function () {
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/G42UmbracoGrease/appsettingsapi/getall"), 'Failed to get app settings from Umbraco'
            );
        },
        saveAppSetting: function (key, value) {
            return umbRequestHelper.resourcePromise(
                $http.post("/umbraco/backoffice/G42UmbracoGrease/appsettingsapi/save", { key: key, value: value }), 'Failed to save app setting Umbraco'
            );
        },
        addAppSetting: function (key, value) {
            return umbRequestHelper.resourcePromise(
                $http.post("/umbraco/backoffice/G42UmbracoGrease/appsettingsapi/add", { key: key, value: value }), 'Failed to add app setting Umbraco'
            );
        },
        removeAppSetting: function (id) {
            return umbRequestHelper.resourcePromise(
                $http.post("/umbraco/backoffice/G42UmbracoGrease/appsettingsapi/remove", id), 'Failed to remove app setting Umbraco'
            );
        }
    }
});