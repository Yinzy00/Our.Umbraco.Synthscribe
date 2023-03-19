(function () {
    'use strict';

    angular.module("umbraco")
        .controller("Our.Umbraco.Synthscribe.Backoffice.DoctypeController",
            function ($scope, $http, notificationsService, appState) {
                var vm = this;
                vm.isLoading = false;
                vm.alert = null;
                vm.active = false;
                vm.context = null;
                vm.title = null;
                vm.generationFailed = null;

                //Get Node Id From Menu Actions
                const initAdditionalData = async () => {

                    appState.getMenuState("menuActions")?.forEach(action => {
                        if (action.alias === "quickcreate")
                            vm.title = action.metaData.title;
                    });
                }


                //Generate doctype using context
                vm.generate = async () => {

                    if (vm.context && vm.context !== "") {
                        enableLoading();

                        var response = null;

                        response = await $http({
                            method: "POST",
                            url: "/umbraco/backoffice/Synthscribe/GenerateDoctype/GenerateDoctype",
                            data: {
                                context: vm.context
                            },
                            headers: {
                                "Content-Type": "application/json"
                            }
                        });

                        var message = response.data.message;

                        if (response?.status == 200) {

                            vm.alert = {
                                alertType: 'success',
                                message: message
                            };
                            vm.generationFailed = false;
                        }
                        else {

                            vm.alert = {
                                alertType: 'error',
                                message: message
                            };

                            vm.generationFailed = true;
                        }

                        disableLoading();

                        $scope.$apply();
                    }
                    else {
                        notificationsService.error("Context is empty!");
                    }
                }

                const enableLoading = () => {
                    vm.isLoading = true;
                }

                const disableLoading = () => {
                    vm.isLoading = false;
                }

                const init = async () => {

                    //Todo
                    enableLoading();

                    initAdditionalData();

                    vm.active = true;

                    disableLoading();

                    //$scope.$apply();
                }
                init();

            });
})();