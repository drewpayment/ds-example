declare var angular: ng.IAngularStatic;

export class Utilities {
    static getPageTypeByRoute(route: string) {
        let pageType = 1;
        if (route == 'Link')
          pageType = 2;
        else if (route == 'Video')
          pageType = 4;
  
        return pageType;  
    }

    static getLinkToStateByRoute(route: string) {
        let linkToState = 'ess.onboarding.document';
        if (route == 'Link')
          linkToState = 'ess.onboarding.link';
        else if (route == 'Video')
          linkToState = 'ess.onboarding.video';
  
        return linkToState;  
    }

    static getRouteByPageType(pageType: number) {
        let route = 'Document';
        if (pageType == 2)
          route = 'Link';
        else if (pageType == 4)
          route = 'Video';
    
        return route;  
    }
    
    static getLinkToStateByPageType(pageType: number) {
        let linkToState = 'ess.onboarding.document';
        if (pageType == 2)
            linkToState = 'ess.onboarding.link';
        else if (pageType == 4)
            linkToState = 'ess.onboarding.video';

        return linkToState;  
    }

    static insertAtCaret(text, input) {
      if (input == undefined) { return; }
      var scrollPos = input.scrollTop;
      var pos = 0;
      var browser = ((input.selectionStart || input.selectionStart == "0") ?
          "ff" : (document.selection ? "ie" : false));
      if (browser == "ie") {
          input.focus();
          var range = document.selection.createRange();
          range.moveStart("character", -input.value.length);
          pos = range.text.length;
      }
      else if (browser == "ff") { pos = input.selectionStart };

      var front = (input.value).substring(0, pos);
      var back = (input.value).substring(pos, input.value.length);
      input.value = front + text + back;
      pos = pos + text.length;
      if (browser == "ie") {
          input.focus();
          var range = document.selection.createRange();
          range.moveStart("character", -input.value.length);
          range.moveStart("character", pos);
          range.moveEnd("character", 0);
          range.select();
      }
      else if (browser == "ff") {
          input.selectionStart = pos;
          input.selectionEnd = pos;
          input.focus();
      }

      input.scrollTop = scrollPos;
      angular.element(input).trigger('input');
  }
}