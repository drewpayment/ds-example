export class CompetencyGroupItem {
    _numberCompleted: number;
    _totalCompetencies: number;
    _groupName: string;
    constructor(numComplete: number, totalComps: number, grpName: string) {
        this._numberCompleted = numComplete;
        this._totalCompetencies = totalComps;
        this._groupName = grpName;
    }
    set numberCompleted(completed: number) {
        this._numberCompleted = completed;
    }
    set totalCompetencies(totalCompetencies: number) {
        this._totalCompetencies = totalCompetencies;
    }
    set groupName(name: string) {
        this._groupName = name;
    }
    get groupName() {
        return this._groupName;
    }
    get totalCompetencies() {
        return this._totalCompetencies;
    }
    get numberCompleted() {
        return this._numberCompleted;
    }
}
