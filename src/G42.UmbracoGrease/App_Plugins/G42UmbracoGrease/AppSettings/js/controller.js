angular.module('umbraco').controller('G42UmbracoGreaseAppSettingsDashboardController', function ($scope, $routeParams, $timeout, greaseAppSettingsService) {

    $scope.model = {};
    $scope.model.Name = decodeURIComponent($routeParams.id);

    $scope.model.data = [];
    $scope.model.new = {};

    $scope.isLoading = true;

    greaseAppSettingsService.getAll().then(function(data) {
        $scope.model.data = data;
        $scope.isLoading = false;
    });

    $scope.add = function() {
        greaseAppSettingsService.addAppSetting($scope.model.new.key, $scope.model.new.value).then(function (data) {
            console.log(data);
            window.location.reload();
        });
    }

    $scope.save = function (setting) {
        setting.isSaving = true;

        greaseAppSettingsService.saveAppSetting(setting.key, setting.value).then(function(data) {
            console.log(data);
            $timeout(function() {
                setting.isSaving = false;
            }, 500);
        });
    }

    $scope.remove = function(id) {
        if (confirm("Are you sure you want to remove this?")) {
            greaseAppSettingsService.removeAppSetting(id).then(function(data) {
                console.log(data);
                $scope.model.data = _.without($scope.model.data, _.findWhere($scope.model.data, { id: id }));
            });
        }
    }
});