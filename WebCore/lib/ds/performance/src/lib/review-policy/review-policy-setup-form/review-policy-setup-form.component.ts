import { 
    Component, 
    OnInit
} from '@angular/core';
import { 
    FormGroup} from '@angular/forms';
import * as _ from 'lodash';

import { IReviewTemplate } from '../../../../../core/src/lib/groups/shared/review-template.model';
import { Observable, combineLatest } from 'rxjs';
import { map, finalize, tap } from 'rxjs/operators';
import { ReferenceDate } from '../../../../../core/src/lib/groups/shared/schedule-type.enum';
import { Pipe, PipeTransform } from '@angular/core';
import { Maybe } from '@ds/core/shared/Maybe';
import { ReviewPolicyService } from './review-policy.service';
import { GroupService } from '@ds/core/groups/group.service';
import { Group } from '@ds/core/groups/shared/group.model';
import { sortTemplates } from '@ds/core/groups/shared/sort-templates.pipe';

export type AllReviewTemplates = {
    active: IReviewTemplate[],
    archived: IReviewTemplate[]
};

type FormData = {
    templates: AllReviewTemplates,
    groups: Group[]
};
type FormStream = Observable<FormData>;


@Component({
    selector: 'ds-review-policy-setup-form',
    templateUrl: './review-policy-setup-form.component.html',
    styleUrls: ['./review-policy-setup-form.component.scss']
})
export class ReviewPolicySetupFormComponent implements OnInit {

    f: FormGroup = new FormGroup({});
    submitted = false;
    reviewProfiles = [];
    data$: FormStream;
    viewArchive: boolean = false;
    readonly addGroup: (data: IReviewTemplate[]) => void;
    readonly deleteGroup: (id: number) => void;
    readonly updateGroup: (id: number) => void;

    constructor(
        private reviewPolicySvc: ReviewPolicyService,
        private groupSvc: GroupService){
            this.addGroup = () => groupSvc.AddGroup(reviewPolicySvc);
            this.deleteGroup = (id) => groupSvc.DeleteGroup(id, reviewPolicySvc);
            this.updateGroup = (id) => groupSvc.UpdateGroup(id, reviewPolicySvc);
        }

    ngOnInit(): void {
        this.data$ = this.attachSelector(
            this.getTemplates(),
            this.getGroups());
    }

    private getTemplates(): Observable<IReviewTemplate[]> {
        return this.reviewPolicySvc.allTemplatesForClient$;
    }

    private getGroups(): Observable<Group[]> {
        return this.groupSvc.groups$
    }



    private attachSelector(rawTemps: Observable<IReviewTemplate[]>, rawGroups: Observable<Group[]>) : FormStream {
        return combineLatest(rawTemps, rawGroups).pipe(
            map(data => ({
                archived: data[0].filter(template => template.isArchived), 
                active: data[0].filter(template => !template.isArchived),
            groups: data[1]})),
            map(templates => {
                const activeTemplates = {};
                new Maybe(templates.active).map(activeTemps => {
                    activeTemps.forEach(template => {
                        activeTemplates[template.reviewTemplateId] = null;
                    })
                })

                return {
                    templates: {
                        active: sortTemplates(templates.active), 
                        archived: sortTemplates(templates.archived)
                    },
                    groups: templates.groups
                }
            }),
            map(x => (<FormData>{
            templates: x.templates,
            groups: x.groups
        })))
    }

    filterOut

    toggleTemplateList(): void {
        this.viewArchive = !this.viewArchive;
    }
}

@Pipe({ name: 'reviewTypeToString' })
export class ReviewTypeToStringPipe implements PipeTransform {
    transform(value: ReferenceDate): any {
        switch (value) {
            case ReferenceDate.HardCodedRange:
                return 'Recurring';
            case ReferenceDate.CalendarYear:
                return 'Recurring';
            case ReferenceDate.DateOfHire:
                return 'New Hire';
            default:
                throw new InvalidReferenceDateException();
        }
    }
}

@Pipe({ name: 'isRecurringToString' })
export class IsRecurringToStringPipe implements PipeTransform {
    transform(value: Boolean): any {
        switch (value) {
            case true:
                return 'Recurring';
            case false:
                return 'One Time';
            default:
                return 'One Time';
        }
    }
}

@Pipe({name: 'toggleTemplates'})
export class ToggleTemplatesPipe implements PipeTransform {
    transform(templates: FormData, viewArchive: boolean): any {
        const safeTemplates = new Maybe(templates).map(x => x.templates);
        const result = viewArchive ? safeTemplates.map(x => x.archived) : safeTemplates.map(x => x.active);
        return result.value();
    }
}

@Pipe({name: 'filterTemplatesNotInList'})
export class filterTemplatesNotInListPipe implements PipeTransform {
    transform(templateIds: Maybe<number[]>, templates: Maybe<IReviewTemplate[]>): number[] {
        const templateFilter = {};
                templates.map(activeTemps => {
                    activeTemps.forEach(template => {
                        templateFilter[template.reviewTemplateId] = null;
                    })
                })
                return templateIds.map(x => x.filter(id => templateFilter[id] !== undefined)).valueOr([]);
        
    }
}

export class InvalidReferenceDateException extends Error {
constructor(message?:string) {
    const defaultMsg = 'Invalid Reference Date!';
    super(message ? defaultMsg + ': ' + message : defaultMsg);
    Object.setPrototypeOf(this, new.target.prototype);
}
}