export function capitalize(input:string) {
    return (!!input) ? input.charAt(0).toUpperCase() + input.substr(1).toLowerCase() : '';
}

export class CapitalizeFilter {
    static FILTER_NAME = 'capitalize';

    static $filter() {
        let filter = () => capitalize;
        filter.$inject = [];
        return filter;
    }
}