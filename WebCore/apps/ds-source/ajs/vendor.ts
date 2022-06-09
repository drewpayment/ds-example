//IMPORTANT: see angular.json "scripts" property of ds-source for
//additional global vendor scripts being loaded.
//not added here as they need to be pulled in as a 
//separate bundle loaded in the <head> instead of appended to the <body>
//since other scripts on the ASPX pages need them (e.g. JQuery, etc)

//general 3rd party libs
import "moment"; //<-- this is also included in the angular.json "scripts", but needed here for type compilation. Ideally we would remove here, but don't know how to get it working yet.
import "localforage";
import "lodash";
import "spin.js";
import "chart.js";

//ajs vendors
import "angular-animate";
import "angular-aria";
import "angular-material";
import "angular-material-data-table";
import "angular-file-saver";
import "angular-hotkeys";
import "restangular";
import "angular-messages";
import "angular-ui-router";
import "angular-localforage";
import "angular-presence";
import "angular-ui-bootstrap";
import "angular-ui-sortable";
import "angular-spinner";
import "ng-file-upload";
import "angular-sanitize";

