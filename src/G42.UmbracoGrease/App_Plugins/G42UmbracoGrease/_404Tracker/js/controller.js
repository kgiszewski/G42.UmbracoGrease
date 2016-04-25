angular.module('umbraco').controller('G42UmbracoGrease404TrackerDashboardController', function ($scope, $routeParams, grease404Service) {

    $scope.model = {};
    $scope.model.Name = decodeURIComponent($routeParams.id);

    $scope.model.countFilter = 50;

    $scope.getResults = function () {
        $scope.isLoading = true;

        grease404Service.get404s($scope.model.countFilter).then(function (results) {
            $scope.model.data = results.data;
            $scope.isLoading = false;
        });
    }
});