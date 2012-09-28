/*jslint browser: true*/
/*global jQuery*/

var Markie = Markie || {};

Markie.draftSaver = (function ($) {
    'use strict';
    var textbox,
        addButton,
        addAndEditButton,
        pubSubKey = 'draftAdded',
        subscribe = function (callback) {
            $.subscribe(pubSubKey, callback);
        },
        publish = function (result) {
            $.publish(pubSubKey, [result.id, result.title, result.url]);
        },
        addDraftSuccess = function (data, callback) {
            Markie.alerter.addSuccess('The draft was successfully created...');

            textbox.val('');
            publish(data);

            if (typeof callback === 'function') {
                callback(data);
            }
        },
        addDraftError = function () {
            Markie.alerter.addError('Something went wrong while creating the draft...');
        },
        addDraft = function (successCallback) {
            var newDraftTitle = textbox.val();
            $.post('/admin/posts/add', { title: newDraftTitle }, function (data) {
                if (data.Success) {
                    addDraftSuccess(data, successCallback);
                } else {
                    addDraftError();
                }
            }).error(function () {
                addDraftError();
            });
        },
        addDraftEvent = function (e) {
            e.preventDefault();
            addDraft();
        },
        addDraftAndEditEvent = function (e) {
            e.preventDefault();
            addDraft(function (data) {
                document.location = data.Url;
            });
        },
        init = function (textboxSelector, addButtonSelector, addAndEditButtonSelector) {
            textbox = $(textboxSelector);
            addButton = $(addButtonSelector);
            addAndEditButton = $(addAndEditButtonSelector);

            addButton.on('click', addDraftEvent);
            addAndEditButton.on('click', addDraftAndEditEvent);
        };

    return {
        init: init,
        subscribe: subscribe
    };
}(jQuery));