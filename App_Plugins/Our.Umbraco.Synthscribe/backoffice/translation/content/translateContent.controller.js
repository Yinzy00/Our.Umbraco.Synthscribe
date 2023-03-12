(function () {
    'use strict';

    angular.module("umbraco")
        .controller("Our.Umbraco.Synthscribe.Backoffice.DictionaryController",
            function ($scope, $http, notificationsService, appState) {
                var vm = this;
                vm.isLoading = false;
                vm.alert = null;
                //Check if 
                vm.active = false;
                vm.overwrite = false;
                vm.languages = [];
                vm.defaultLanguage = null;
                vm.langTo = '';
                vm.nodeId = null;
                vm.pageName = null;
                vm.translateDescendants = false;
                vm.translationFailed = null;

                //Get Node Id From Menu Actions
                const initAdditionalData = async () => {
debugger;
                    appState.getMenuState("menuActions")?.forEach(action => {
                        if (action.alias === "translate")
                            vm.nodeId = action.metaData.nodeId;
                            vm.pageName = action.metaData.pageName;
                    });
                }

                //Get Language data from API
                const initLanguages = async () => {

                    var response = await $http({
                        method: "GET",
                        url: "/umbraco/backoffice/Synthscribe/Translation/Languages"
                    });

                    if (response.status == 200) {
                        var languages = await response.data.languages;
                        vm.languages = ["", ...languages];
                        vm.defaultLanguage = await response.data.defaultLanguage;
                    }
                    else {
                        console.log("Error: " + response.status);
                    }
                }

                //Translate selected dictionary & descendants (if selected)
                vm.translate = async () => {

                    enableLoading();

                    var response = null;

                    if (vm.nodeId == "-1") {
                        response = await $http({
                            method: "POST",
                            url: "/umbraco/backoffice/Synthscribe/Translation/TranslateAllContent",
                            data: {
                                nodeId: vm.nodeId,
                                languageTo: vm.langTo,
                                overwrite: vm.overwrite,
                                translateDescendants: vm.translateDescendants
                            },
                            headers: {
                                "Content-Type": "application/json"
                            }
                        });
                    }
                    else {
                        response = await $http({
                            method: "POST",
                            url: "/umbraco/backoffice/Synthscribe/Translation/TranslateContent",
                            data: {
                                nodeId: vm.nodeId,
                                languageTo: vm.langTo,
                                overwrite: vm.overwrite,
                                translateDescendants: vm.translateDescendants
                            },
                            headers: {
                                "Content-Type": "application/json"
                            }
                        });
                    }

                    if (response?.status == 200) {
                        notificationsService.success("Translation succesfull");
                        vm.alert = {
                            alertType: 'success',
                            message: "Dictionary / dictionaries translated successfully! (Click escape to exit)"
                        };
                        vm.translationFailed = false;
                    }
                    else {
                        notificationsService.error("Translation failed");
                        vm.alert = {
                            alertType: 'error',
                            message: "Something went wrong! Try again...!"
                        };
                        vm.translationFailed = true;
                    }

                    disableLoading();

                    $scope.$apply();
                }

                vm.toggleOverwrite = () => {
                    vm.overwrite = !vm.overwrite;
                }

                vm.toggleDescendants = () => {
                    vm.translateDescendants = !vm.translateDescendants;
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

                    if (!vm.nodeId)
                        return;

                    await initLanguages();

                    vm.active = true;

                    disableLoading();

                    $scope.$apply();
                }
                init();

            });
})();