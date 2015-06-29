angular.module('umbraco').controller('G42UmbracoGreaseAppSettingsDashboardController', function ($scope, $routeParams, greaseAppSettingsService) {

    $scope.model = {};
    $scope.model.Name = decodeURIComponent($routeParams.id);

    $scope.model.data = [];
    $scope.model.new = {};

    greaseAppSettingsService.getAll().then(function(data) {
        $scope.model.data = data;
    });

    $scope.add = function() {
        greaseAppSettingsService.addAppSetting($scope.model.new.key, $scope.model.new.value).then(function (data) {
            console.log(data);
            window.location.reload();
        });
    }

    $scope.save = function(setting) {
        greaseAppSettingsService.saveAppSetting(setting.key, setting.value).then(function(data) {
            console.log(data);
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