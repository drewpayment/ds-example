import {Moment} from 'moment';
import * as moment from 'moment';

export interface Moment extends Moment {
    toApiString?():string
}

export class MomentExtensions {
    
    constructor() {
        (<any>moment.fn).toApiString = this.toApiString;
    }
    
    /**
     * Extension method available to the moment.js library that formats a Moment object to 
     * a ISO8601 string that is will be parsed by the .NET WebApi without extraneous JSON parser serialization 
     * configuration, and also without using a momentjs built-in utility that will automatically convert 
     * the Moment object to UTC time. 
     */
    toApiString = function():string {
        return moment(this).clone().format('YYYY-MM-DD HH:mm:ss');
    }
    
    static initialize = () => new MomentExtensions();
    
}