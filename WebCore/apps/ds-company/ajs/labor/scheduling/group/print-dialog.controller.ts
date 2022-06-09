export class GroupSchedulePrintDialogController {
    static readonly $inject = [
        '$scope',
        '$timeout',
        '$modalInstance',
        'settings'
    ];

    constructor ($scope, $timeout, $modalInstance, settings) {
        $scope.settings = settings;

        $scope.cancel = function () {
            $modalInstance.dismiss();
        };

        $scope.print = function () {
            $modalInstance.close($scope.settings);
        };
    }
}