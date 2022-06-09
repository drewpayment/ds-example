import { DsStorageService } from '@ds/core/storage/storage.service';

export interface IDashboardState {
    isOnboardingCompleted: boolean;
    searchText: string;
    searchWord: string;
    searchClientId?: number;
    relatedClientId?: number;
    displayValue: string;
    sortByValue: string;
    sortByOrder: string;
    selectedClientId?: number;
    selectedClientName: string;
}

export class DashboardState implements IDashboardState{
    private storeService : DsStorageService;
    private $rootScope : any;
    readonly rootscope_key:string =  "DASHBOARD_SEARCH_OPTIONS_KEY";

    constructor(storeSvc: DsStorageService, scopeHolder: any = null){
        this.storeService = storeSvc;
        this.$rootScope = scopeHolder ? scopeHolder : {};
    }

    get storeInstance():IDashboardState{
        let storeResult = this.storeService.get(this.rootscope_key);
        if(storeResult.success) return storeResult.data;
        else                    return <IDashboardState>{};
    }
    set storeInstance(value :IDashboardState){
        let storeResult = this.storeService.set(this.rootscope_key, value);
    }

    /********** Properties *******************/

    get isOnboardingCompleted(): boolean {
        let k = this.storeInstance;
        return k.isOnboardingCompleted;
    }
    set isOnboardingCompleted(value: boolean) {
        let k = this.storeInstance;
        k.isOnboardingCompleted = value;
        this.$rootScope.isOnboardingCompleted = value;
        this.storeInstance = k;
    }

    get searchText(): string {
        let k = this.storeInstance;
        return k.searchText;
    }
    set searchText(value: string) {
        let k = this.storeInstance;
        k.searchText = value;
		this.$rootScope.searchText = value;
        this.storeInstance = k;
    }

    get searchWord(): string {
        let k = this.storeInstance;
        return k.searchWord;
    }
    set searchWord(value: string) {
        let k = this.storeInstance;
        k.searchWord = value;
		this.$rootScope.searchWord = value;
        this.storeInstance = k;
    }

    get searchClientId(): number {
        let k = this.storeInstance;
        return k.searchClientId;
    }
    set searchClientId(value: number) {
        let k = this.storeInstance;
        k.searchClientId = value;
		this.$rootScope.searchClientId = value;
        this.storeInstance = k;
    }

    get relatedClientId(): number {
        let k = this.storeInstance;
        return k.relatedClientId;
    }
    set relatedClientId(value: number) {
        let k = this.storeInstance;
        k.relatedClientId = value;
		this.$rootScope.relatedClientId = value;
        this.storeInstance = k;
    }

    get displayValue(): string {
        let k = this.storeInstance;
        return k.displayValue;
    }
    set displayValue(value: string) {
        let k = this.storeInstance;
        k.displayValue = value;
		this.$rootScope.displayValue = value;
        this.storeInstance = k;
    }

    get sortByValue(): string {
        let k = this.storeInstance;
        return k.sortByValue;
    }
    set sortByValue(value: string) {
        let k = this.storeInstance;
        k.sortByValue = value;
		this.$rootScope.sortByValue = value;
        this.storeInstance = k;
    }

    get sortByOrder(): string {
        let k = this.storeInstance;
        return k.sortByOrder;
    }
    set sortByOrder(value: string) {
        let k = this.storeInstance;
        k.sortByOrder = value;
		this.$rootScope.sortByOrder = value;
        this.storeInstance = k;
    }

    get selectedClientId(): number {
        let k = this.storeInstance;
        return k.selectedClientId;
    }
    set selectedClientId(value: number) {
        let k = this.storeInstance;
        k.selectedClientId = value;
		this.$rootScope.selectedClientId = value;
        this.storeInstance = k;
    }

    get selectedClientName(): string {
        let k = this.storeInstance;
        return k.selectedClientName;
    }
    set selectedClientName(value: string) {
        let k = this.storeInstance;
        k.selectedClientName = value;
		this.$rootScope.selectedClientName = value;
        this.storeInstance = k;
    }

    /********** End of Properties *******************/

    public bindStorageState(){
        let k = this.storeInstance;
        if(this.$rootScope.selectedClientId)
        {
            k.isOnboardingCompleted = this.$rootScope.isOnboardingCompleted;
            k.searchText 			= this.$rootScope.searchText;
            k.searchWord 			= this.$rootScope.searchWord;
            k.searchClientId 		= this.$rootScope.searchClientId;
            k.relatedClientId 		= this.$rootScope.relatedClientId;
            k.displayValue 			= this.$rootScope.displayValue;
            k.sortByValue 			= this.$rootScope.sortByValue;
            k.sortByOrder 			= this.$rootScope.sortByOrder;
            k.selectedClientId 		= this.$rootScope.selectedClientId;
            k.selectedClientName 	= this.$rootScope.selectedClientName;
            this.storeInstance = k;
        }
        else if(!!k && Object.keys.length > 0 )
        {
            this.$rootScope.isOnboardingCompleted 	= k.isOnboardingCompleted;
            this.$rootScope.searchText 				= k.searchText;
            this.$rootScope.searchWord 				= k.searchWord;
            this.$rootScope.searchClientId 			= k.searchClientId;
            this.$rootScope.relatedClientId 		= k.relatedClientId;
            this.$rootScope.displayValue 			= k.displayValue;
            this.$rootScope.sortByValue 			= k.sortByValue;
            this.$rootScope.sortByOrder 			= k.sortByOrder;
            this.$rootScope.selectedClientId 		= k.selectedClientId;
            this.$rootScope.selectedClientName 		= k.selectedClientName;
        }
    }
}