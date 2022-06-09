import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAccrualsComponent } from './client-accruals.component';

describe('ClientAccrualsComponent', () => {
    let component: ClientAccrualsComponent;
    let fixture: ComponentFixture<ClientAccrualsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ ClientAccrualsComponent ]
        })
        .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(ClientAccrualsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
