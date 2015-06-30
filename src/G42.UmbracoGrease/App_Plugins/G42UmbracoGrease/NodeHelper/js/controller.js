angular.module('umbraco').controller('G42UmbracoGreaseNodeHelperDashboardController', function ($scope, $routeParams, greaseNodeHelperService) {

    $scope.model = {};
    $scope.model.Name = decodeURIComponent($routeParams.id);

    $scope.model.data = [];
    $scope.model.new = {};

    $scope.isLoading = true;

    greaseNodeHelperService.getNodeHelper().then(function (data) {
        $scope.model.data = data;
        $scope.isLoading = false;
    });

    $scope.reset = function() {
        if (confirm("Are you sure you want to reset Node Helper?")) {
            greaseNodeHelperService.resetNodeHelper().then(function(data) {
                console.log(data);
            });
        }
    }
});