export class PhoneNumber {
    $formatted:string;
    $numbersOnly:string;
    $isValid:boolean;

    constructor(private phone:string){
        this.$onInit();
    }

    $onInit(){
        this.$formatted   = null;
        this.$numbersOnly = null;
        this.$isValid     = false;

        if(this.phone){
            this.$numbersOnly = this.phone.replace(/[\D]/g,"");

            //right now we only allow standard US phone numbers
            if(this.$numbersOnly.length === 10) {
                let area   = this.$numbersOnly.substr(0,3),
                    prefix = this.$numbersOnly.substr(3,3),
                    suffix = this.$numbersOnly.substr(6,4);
                
                this.$formatted = `(${area}) ${prefix}-${suffix}`;
                this.$isValid   = true;
            }
        }
    }
}