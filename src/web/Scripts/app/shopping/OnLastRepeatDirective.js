﻿/* jshint -W099 */
/* global jQuery:false */
/* global angular:false */
(function($, productApp) {

	"use strict";

	productApp.directive('onLastRepeat', function () {
		return function (scope, element, attrs) {
		    if (scope.$last) {
				setTimeout(setEqualHeights, 50);

			}
		};

		function setEqualHeights() {
			if ($(window).width() > 737) {
				$('#equalHeights .tile').equalHeights();
			}
		}
	});

})(jQuery, window.productApp);