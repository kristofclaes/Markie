/*jslint browser: true*/
/*global jQuery*/

var Markie = Markie || {};

Markie.alerter = (function ($) {
    'use strict';
    var containerElement = $('#container'),
        addAlert = function (text, alertType) {
            var newAlertBox = $('<div />').addClass('alert').html(text).prepend($('<button type="button" class="close" data-dismiss="alert">×</button>')).hide();
            if (alertType) {
                newAlertBox.addClass('alert-' + alertType);
            }
            containerElement.prepend(newAlertBox);
            newAlertBox.fadeIn();
        },
        addError = function (text) {
            addAlert(text, 'error');
        },
        addSuccess = function (text) {
            addAlert(text, 'success');
        },
        addInformation = function (text) {
            addAlert(text, 'info');
        },
        addMessage = function (text) {
            addAlert(text);
        };

    return {
        addError: addError,
        addSuccess: addSuccess,
        addInformation: addInformation,
        addMessage: addMessage
    };
}(jQuery));