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
    fromDate?: Date;
    toDate?: Date;
}

export class DashboardState implements IDashboardState{
    private storeService : DsStorageService;
    readonly rootscope_key:string =  "DASHBOARD_SEARCH_OPTIONS_KEY";

    constructor(storeSvc: DsStorageService){
        this.storeService = storeSvc;
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
        this.storeInstance = k;
    }

    get searchText(): string {
        let k = this.storeInstance;
        return k.searchText;
    }
    set searchText(value: string) {
        let k = this.storeInstance;
        k.searchText = value;
		this.storeInstance = k;
    }

    get searchWord(): string {
        let k = this.storeInstance;
        return k.searchWord;
    }
    set searchWord(value: string) {
        let k = this.storeInstance;
        k.searchWord = value;
		this.storeInstance = k;
    }

    get searchClientId(): number {
        let k = this.storeInstance;
        return k.searchClientId;
    }
    set searchClientId(value: number) {
        let k = this.storeInstance;
        k.searchClientId = value;
		this.storeInstance = k;
    }

    get relatedClientId(): number {
        let k = this.storeInstance;
        return k.relatedClientId;
    }
    set relatedClientId(value: number) {
        let k = this.storeInstance;
        k.relatedClientId = value;
		this.storeInstance = k;
    }

    get displayValue(): string {
        let k = this.storeInstance;
        return k.displayValue;
    }
    set displayValue(value: string) {
        let k = this.storeInstance;
        k.displayValue = value;
		this.storeInstance = k;
    }

    get sortByValue(): string {
        let k = this.storeInstance;
        return k.sortByValue;
    }
    set sortByValue(value: string) {
        let k = this.storeInstance;
        k.sortByValue = value;
		this.storeInstance = k;
    }

    get sortByOrder(): string {
        let k = this.storeInstance;
        return k.sortByOrder;
    }
    set sortByOrder(value: string) {
        let k = this.storeInstance;
        k.sortByOrder = value;
		this.storeInstance = k;
    }

    get selectedClientId(): number {
        let k = this.storeInstance;
        return k.selectedClientId;
    }
    set selectedClientId(value: number) {
        let k = this.storeInstance;
        k.selectedClientId = value;
		this.storeInstance = k;
    }

    get selectedClientName(): string {
        let k = this.storeInstance;
        return k.selectedClientName;
    }
    set selectedClientName(value: string) {
        let k = this.storeInstance;
        k.selectedClientName = value;
		this.storeInstance = k;
    }

    get fromDate(): Date {
        let k = this.storeInstance;
        return k.fromDate;
    }
    set fromDate(value: Date) {
        let k = this.storeInstance;
        k.fromDate = value;
		this.storeInstance = k;
    }
    get toDate(): Date {
        let k = this.storeInstance;
        return k.toDate;
    }
    set toDate(value: Date) {
        let k = this.storeInstance;
        k.toDate = value;
		this.storeInstance = k;
    }

    /********** End of Properties *******************/
}