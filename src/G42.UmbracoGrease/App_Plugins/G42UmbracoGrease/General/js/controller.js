angular.module('umbraco').controller('G42UmbracoGreaseGeneralDashboardController', function ($scope, $routeParams, greaseGeneralService, notificationsService) {

    $scope.model = {};
    $scope.model.Name = decodeURIComponent($routeParams.id);

    $scope.isSaving = true;

    greaseGeneralService.getConfig().then(function (config) {
        $scope.model.config = config;

        $scope.isSaving = false;
    });

    $scope.save = function () {

        $scope.isSaving = true;

        greaseGeneralService.save($scope.model.config).then(function (result) {
            $scope.isSaving = false;
            notificationsService.success("Configuration saved.");
        });
    }
});