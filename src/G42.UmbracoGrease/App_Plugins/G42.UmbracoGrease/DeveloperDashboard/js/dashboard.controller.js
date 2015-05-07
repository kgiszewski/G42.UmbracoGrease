angular.module('umbraco').controller('DeveloperDashboardController', function ($scope, developerDashboardService) {
    
    $scope.clearNodeHelper = function() {
        developerDashboardService.clearNodeHelper().then(function(data) {
            console.log(data);
        });
    }
});