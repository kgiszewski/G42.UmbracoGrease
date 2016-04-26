angular.module('umbraco').controller('G42UmbracoGrease404TrackerDashboardController', function ($scope, $routeParams, grease404Service, notificationsService) {

    $scope.model = {};
    $scope.model.Name = decodeURIComponent($routeParams.id);

    $scope.model.countFilter = 50;
    $scope.showSettings = false;
    $scope.isSaving = true;

    $scope.getResults = function () {
        $scope.isLoading = true;

        grease404Service.get404s($scope.model.countFilter).then(function (results) {
            $scope.model.data = results.data;
            $scope.isLoading = false;
        });
    }

    $scope.toggleSettings = function() {
        $scope.showSettings = !$scope.showSettings;
    }

    $scope.save = function() {
        $scope.isSaving = true;

        grease404Service.save($scope.model.config).then(function() {
            $scope.isSaving = false;

            notificationsService.success("Configuration saved!");
        });
    }

    grease404Service.getConfig().then(function (config) {
        $scope.model.config = config;

        $scope.isSaving = false;
    });
});