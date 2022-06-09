import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvaluationHeaderComponent } from './evaluation-header.component';

describe('EvaluationHeaderComponent', () => {
    let component: EvaluationHeaderComponent;
    let fixture: ComponentFixture<EvaluationHeaderComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
        declarations: [ EvaluationHeaderComponent ]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(EvaluationHeaderComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
