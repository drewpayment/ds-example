export class DsInlineValidatedInputService {
	static readonly SERVICE_NAME = 'DsInlineValidatedInputService';
	static readonly $inject = ['$rootScope', '$q'];

	static readonly NOTIFY_DS_INPUTS_FOR_FORM = 'NOTIFY_DS_INPUTS_FOR_FORM';
	static readonly NOTIFY_DS_INPUTS_ALL = 'NOTIFY_DS_INPUTS_ALL';

	registrationsArr = [];      //[{frmName: '', doesInputHaveErrors: validationFunc(), promise: promise, isCustom: bool}]
	registeredSuppressed = [];  //['frmName']

	constructor(private $rootScope: ng.IRootScopeService, private $q: ng.IQService) {
		//---------------------------------------------------
		//CLEAR ALL REGISTRATIONS ON ROUTE CHANGE
		//---------------------------------------------------
		$rootScope.$on("$stateChangeStart", (event, current, previous) => {
			this.clearAllRegistrations();
		});
	}

	/**
	 * ADD A DIRECTIVE TO THE SERVICE
	 */
	registerDirective(directiveMetadata) {
		this.registrationsArr.push(directiveMetadata);
	}

	executeDirectiveValidationPromise(metaData) {
		var p = this.$q.when(metaData.validationPromise());

		//wrote a handler to handle the non-promise validators that don't reject but return true or false;
		return p.then((wereThereErrors) => {
				if (wereThereErrors) {
					return this.$q.reject();
				}
			}
		);
	}
	
	validateAll(formName?) {
		var promises = [];

		//loop through all the registered input directives
		for (var i = 0; i < this.registrationsArr.length; i++) {
			var metaData = this.registrationsArr[i];

			//check the to see if the form name matches; if it was passed in. could be null or undefined
			if (formName) {
				if (metaData.frmName != formName) {
					continue;
				}
			}

			promises.push(this.executeDirectiveValidationPromise(metaData));
		}

		//returning promise
		return this.$q.all(promises).then(
			function (data) {
			},
			(data) => {
				return this.$q.reject();
			}
		);
	}

	/**
	 * CLEAR ALL THE REGISTERED DIRECTIVES
	 */
	clearAllRegistrations() {
		this.registrationsArr = [];
		this.registeredSuppressed = [];      
	}

	/**
	 * GET A BUILT IN (PRE-DEFINED) REGEX PATTERN
	 * @param key 
	 */
	resolveBuiltInRegexPattern(key) {
		
		switch (key) {
			case "phone":
				return /^\d{3}-\d{3}-\d{4}$/;
			case "email":
				return /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;
			case "ssn":
				return /^\d{3}-\d{2}-\d{4}$/;
			case "mmddyyyy":
				return /^\d{2}\/\d{2}\/\d{4}$/;
			case "usZip":
				return /(^\d{4}\d$|^\d{4}\d-\d{4}$)/;
			case "nonNegInt":
				return /^([^.0-]\d+|\d)$/;
			case "money":
				return /^[0-9]\d{0,9}(\.\d{2,2})?%?$/;
		}

		return null;
	}
	
	/**
	 * GET A BUILT IN (PRE-DEFINED) REGEX PATTERN
	 * @param key 
	 */
	resolveBuiltInHelpMessage(key) {

		switch (key) {
			case "phone":
				return 'Use format xxx-xxx-xxxx.';
			case "email":
				return 'Use format email@email.com.';
			case "ssn":
				return 'Use format xxx-xx-xxxx.';
			case "mmddyyyy":
				return 'Use format MM/DD/YYYY.';
			case "usZip":
				return 'Use Format xxxxx or xxxxx-xxxx.';
			case "nonNegInt":
				return 'Must be a non negative integer value.';
			case "money":
				return 'Use format 0.00';
		}

		return null;
	}
	
	/**
	 * GET A BUILT IN (PRE-DEFINED) MASKED EDIT STRING
	 */
	resolveBuiltInMakedEdit(key) {

		switch (key) {
			case "phone":
				return '000-000-0000';
			case "ssn":
				return '000-00-0000';
			case 'mmddyyyy':
				return '00/00/0000';
			case 'usZip':
				return 'ZZZZZZZZZZ';
		}

		return null;
	}	

	//---------------------------------------------------
	//VALIDATION SUPPRESSION
	//---------------------------------------------------
	registerSuppressed(frmName) {
		if (!this.isSuppressed(frmName)) {
			this.registeredSuppressed.push(frmName);
		}
	}
	
	isSuppressed(frmName) {
		var result = false;
		for (var i = this.registeredSuppressed.length - 1; i >= 0; i--) {
			if (this.registeredSuppressed[i] === frmName) {
				result = true;
			}
		}
		return result;
	}
	
	clearSuppressed(frmName) {
		if (arguments.length > 0) {
			for (var i = this.registeredSuppressed.length - 1; i >= 0; i--) {
				if (this.registeredSuppressed[i] === frmName) {
					this.registeredSuppressed.splice(i, 1);
				}
			}
			
		} else {
			this.registeredSuppressed = [];
		}
	}
}