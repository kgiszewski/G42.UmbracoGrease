angular.module('umbraco').controller('G42UmbracoGreaseSearchTrackerDashboardController', function ($scope, $routeParams, greaseReportsService) {

    $scope.model = {};
    $scope.model.Name = decodeURIComponent($routeParams.id);

    $scope.model.data = [];
    $scope.model.countFilter = 5;

    $scope.getResults = function() {
        $scope.isLoading = true;

        greaseReportsService.getKeywords($scope.model.countFilter).then(function (results) {
            $scope.model.data = results.data;
            $scope.isLoading = false;
        });
    }
});