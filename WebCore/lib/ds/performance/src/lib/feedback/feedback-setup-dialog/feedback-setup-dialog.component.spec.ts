import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedbackSetupDialogComponent } from './feedback-setup-dialog.component';

describe('FeedbackSetupDialogComponent', () => {
    let component: FeedbackSetupDialogComponent;
    let fixture: ComponentFixture<FeedbackSetupDialogComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
        declarations: [ FeedbackSetupDialogComponent ]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(FeedbackSetupDialogComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
