export class DsDesignModuleConfig {
    constructor(private $locProv:ng.ILocationProvider, private $urlProv) {
        this.$config();
    }

    $config(){
        this.$locProv.html5Mode(true).hashPrefix("!");
        this.$urlProv.otherwise('/');
    }
}