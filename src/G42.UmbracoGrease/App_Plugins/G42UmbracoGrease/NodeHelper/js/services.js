angular.module('umbraco.services').factory('greaseNodeHelperService', function ($http, $q, umbRequestHelper) {
    return {
        getNodeHelper: function () {
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/G42UmbracoGrease/nodehelperapi/get"), 'Failed to get nodeHelper from Umbraco'
            );
        },
        resetNodeHelper: function () {
            return umbRequestHelper.resourcePromise(
                $http.post("/umbraco/backoffice/G42UmbracoGrease/nodehelperapi/reset", {}), 'Failed to reset nodeHelper in Umbraco'
            );
        }
    }
});