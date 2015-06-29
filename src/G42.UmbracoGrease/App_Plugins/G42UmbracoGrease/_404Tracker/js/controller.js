angular.module('umbraco').controller('_404TrackerDashboardController', function ($scope, $routeParams, greaseReportsService) {

    $scope.model = {};
    $scope.model.Name = decodeURIComponent($routeParams.id);

    $scope.model.countFilter = 5;

    $scope.getResults = function () {
        $scope.isLoading = true;

        greaseReportsService.get404s($scope.model.countFilter).then(function (results) {
            console.log(results);
            $scope.model.data = results.data;
            $scope.isLoading = false;
        });
    }
});