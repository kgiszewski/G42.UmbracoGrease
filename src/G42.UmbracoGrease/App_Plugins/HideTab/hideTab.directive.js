angular.module('umbraco.directives').directive('hideTab', function($timeout) {
    var linker = function (scope, element, attrs) {

        $timeout(function() {
            hideTab(element);
        }, 10);
    }

    function hideTab(element) {

        var $umbPanel = element.closest('.umb-panel');

        var $tabs = $umbPanel.find(".umb-nav-tabs li");

        var $thisPane = element.closest('.umb-tab-pane');

        if ($tabs.length > 1) {

            $thisPane.hide();
        }
        else {
            var $properties = $thisPane.find(".umb-control-group");
            $properties.hide();
        }
        var tab = $tabs.find("a[href=#" + $thisPane.attr('id') + "]");

        tab.hide();

    }

    return {
        restrict: "E",
        link: linker
    }
});