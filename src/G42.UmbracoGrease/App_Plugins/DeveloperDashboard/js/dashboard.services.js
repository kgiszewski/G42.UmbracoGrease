angular.module('umbraco.services').factory('developerDashboardService', function ($q, $http, umbRequestHelper) {
    return {
        clearNodeHelper: function () {
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/developerdashboardapi/developerdashboard/clearnodehelper"), 'Failed to clear node helper'
            );
        }
    }
});