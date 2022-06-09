import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewProfilesListComponent } from './review-profiles-list.component';

describe('ReviewProfilesListComponent', () => {
    let component: ReviewProfilesListComponent;
    let fixture: ComponentFixture<ReviewProfilesListComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
        declarations: [ ReviewProfilesListComponent ]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(ReviewProfilesListComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
