angular.module('umbraco').controller('G42UmbracoGreaseErrorReportingDashboardController', function ($scope, $routeParams, greaseErrorReportingService, notificationsService) {

    $scope.model = {};
    $scope.model.Name = decodeURIComponent($routeParams.id);

    $scope.isSaving = true;

    greaseErrorReportingService.getConfig().then(function(config) {
        $scope.model.config = config;

        $scope.isSaving = false;
    });

    $scope.save = function () {

        $scope.isSaving = true;

        greaseErrorReportingService.save($scope.model.config).then(function (result) {
            $scope.isSaving = false;
            notificationsService.success("Configuration saved.");
        });
    }
});