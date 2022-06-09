import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvaluationOutletComponent } from './evaluation-outlet.component';

describe('EvaluationOutletComponent', () => {
    let component: EvaluationOutletComponent;
    let fixture: ComponentFixture<EvaluationOutletComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
        declarations: [ EvaluationOutletComponent ]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(EvaluationOutletComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
