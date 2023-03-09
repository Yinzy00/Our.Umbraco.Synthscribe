(function () {
    'use strict';

    angular.module("umbraco")
        .controller("Our.Umbraco.Synthscribe.Backoffice.DictionaryController",
            function ($scope, $http, notificationsService, appState) {
                var vm = this;
                vm.isLoading = false;
                //vm.alert = null;
                vm.active = false;
                vm.overwrite = false;
                vm.languages = [];
                vm.defaultLanguage = null;
                vm.langTo = '';
                vm.nodeId = null;
                vm.dictionaryKey = null;
                vm.translateDescendants = false;

                //TODO
                //Populate languages
                //

                //$http("/umbraco/backoffice/Synthscribe/Translation/TranslateDictionary",)


                //diploTranslateResources.getLanguages().then(function (data) {
                //    vm.languages = umbRequestHelper.resourcePromise($http.get(translateApiUrl + "GetLanguages"));
                //    vm.langFrom = vm.languages.filter(lang => lang.IsDefault)[0];
                //});

                // Checks the config is OK

                //diploTranslateResources.checkConfiguration().then(function (response) {
                //    if (!response.Ok) {
                //        vm.alert = { alertType: "error", message: response.Message };
                //        vm.active = false;
                //        return;
                //    }
                //    else {
                //        Init();
                //    }
                //});

                // Run after config is checked

                //Get Node Id From Menu Actions
                const initAdditionalData = async () => {

                    appState.getMenuState("menuActions")?.forEach(action => {
                        if (action.alias === "translate")
                            vm.nodeId = action.metaData.nodeId;
                            vm.dictionaryKey = action.metaData.dictionaryKey;
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

                // Handles translate button click

                //vm.translate = function () {

                //    if (vm.overwrite) {
                //        if (!window.confirm("You have chosen to overwrite existing dictionary values with new translations. Are you sure?")) {
                //            return;
                //        }
                //    }

                //    vm.buttonState = "busy";
                //    vm.isLoading = true;

                //    // calls API service

                //    //diploTranslateResources.translateAllDictionary(clientId, vm.langFrom.IsoCode, vm.overwrite).then(function (response) {

                //    //    vm.isLoading = false;
                //    //    vm.buttonState = "success";

                //    //    if (response.ErrorCount > 0) {
                //    //        notificationsService.warning(response.Message);
                //    //    }
                //    //    else { // OK
                //    //        notificationsService.success(response.Message);

                //    //        // reload

                //    //        setTimeout(function () {
                //    //            window.location.reload(true);
                //    //        }, 2000);
                //    //    }
                //    //});
                //}

                vm.translate = async () => {

                    enableLoading();

                    var response = await $http({
                        method: "POST",
                        url: "/umbraco/backoffice/Synthscribe/Translation/TranslateDictionary",
                        data: {
                            dictionaryId: vm.nodeId,
                            languageTo: vm.langTo,
                            overwrite: vm.overwrite,
                            translateDescendants: vm.translateDescendants
                        },
                        headers: {
                            "Content-Type": "application/json"
                        }
                    });
                    
                    if (response.status == 200) {
                        notificationsService.success("Translation succesfull");
                    }
                    else {
                        notificationsService.error("Translation failed");
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