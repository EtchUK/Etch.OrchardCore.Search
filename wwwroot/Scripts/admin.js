/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./Assets/Admin/js/index.ts");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./Assets/Admin/js/components/contentTypesEditor/components/contentTypeSettings/index.ts":
/*!***********************************************************************************************!*\
  !*** ./Assets/Admin/js/components/contentTypesEditor/components/contentTypeSettings/index.ts ***!
  \***********************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! vue */ \"vue\");\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(vue__WEBPACK_IMPORTED_MODULE_0__);\n\r\n/* harmony default export */ __webpack_exports__[\"default\"] = (vue__WEBPACK_IMPORTED_MODULE_0___default.a.extend({\r\n    props: {\r\n        item: {\r\n            type: Object,\r\n        },\r\n    },\r\n    template: '#content-type-settings',\r\n}));\r\n\n\n//# sourceURL=webpack:///./Assets/Admin/js/components/contentTypesEditor/components/contentTypeSettings/index.ts?");

/***/ }),

/***/ "./Assets/Admin/js/components/contentTypesEditor/index.ts":
/*!****************************************************************!*\
  !*** ./Assets/Admin/js/components/contentTypesEditor/index.ts ***!
  \****************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! vue */ \"vue\");\n/* harmony import */ var vue__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(vue__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var _components_contentTypeSettings__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./components/contentTypeSettings */ \"./Assets/Admin/js/components/contentTypesEditor/components/contentTypeSettings/index.ts\");\n\r\n\r\n/* harmony default export */ __webpack_exports__[\"default\"] = ((initialData, element) => {\r\n    return new vue__WEBPACK_IMPORTED_MODULE_0___default.a({\r\n        el: element,\r\n        components: {\r\n            ContentTypeSettings: _components_contentTypeSettings__WEBPACK_IMPORTED_MODULE_1__[\"default\"],\r\n        },\r\n        data: {\r\n            items: [],\r\n        },\r\n        computed: {\r\n            value() {\r\n                return JSON.stringify(this.items);\r\n            },\r\n        },\r\n        mounted: function () {\r\n            this.items = initialData;\r\n        },\r\n        methods: {},\r\n    });\r\n});\r\n\n\n//# sourceURL=webpack:///./Assets/Admin/js/components/contentTypesEditor/index.ts?");

/***/ }),

/***/ "./Assets/Admin/js/index.ts":
/*!**********************************!*\
  !*** ./Assets/Admin/js/index.ts ***!
  \**********************************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var _components_contentTypesEditor__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./components/contentTypesEditor */ \"./Assets/Admin/js/components/contentTypesEditor/index.ts\");\n\r\nwindow.initializeContentTypesEditor = (el) => {\r\n    const $hiddenDataField = document.getElementById($(el).data('for'));\r\n    if (!$hiddenDataField) {\r\n        return;\r\n    }\r\n    Object(_components_contentTypesEditor__WEBPACK_IMPORTED_MODULE_0__[\"default\"])($($hiddenDataField).data('init'), el);\r\n};\r\n\n\n//# sourceURL=webpack:///./Assets/Admin/js/index.ts?");

/***/ }),

/***/ "vue":
/*!**********************!*\
  !*** external "Vue" ***!
  \**********************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("module.exports = Vue;\n\n//# sourceURL=webpack:///external_%22Vue%22?");

/***/ })

/******/ });